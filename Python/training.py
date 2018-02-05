
import sys
import os
import time
import json
from datetime import datetime
import cntk as C
import pdb

abs_path = os.path.dirname(os.path.abspath(__file__))
sys.path.append(os.path.join(abs_path,".","common"))

print(C.__version__)
#トレーニングデータの取り込み 
def create_reader(path, is_training, input_dim, label_dim):
    return C.io.MinibatchSource(C.io.CTFDeserializer(path, C.io.StreamDefs(
        features = C.io.StreamDef(field="CW", shape=input_dim,   is_sparse=True),
        labels   = C.io.StreamDef(field="CT", shape=label_dim,   is_sparse=True)
    )), randomize=is_training, max_sweeps = C.io.INFINITELY_REPEAT if is_training else 1)

# ニューラルネットワークの定義
def lstm_sequence_classifier(features, num_classes, embedding_dim, hidden_dim):
    classifier = C.layers.Sequential([C.layers.Embedding(embedding_dim), #
                                      bi_recurrence(C.layers.LSTM(hidden_dim//2), C.layers.LSTM(hidden_dim//2)), #
                                      C.layers.Dropout(0.2),#
                                      C.sequence.last,        #
                                      C.layers.Dense(num_classes,activation=C.softmax)]) #
                
    return classifier(features)

#双方向LSTMレイヤーの定義
def bi_recurrence(fwd, bwd):
    F = C.layers.Recurrence(fwd)
    G = C.layers.Recurrence(bwd, go_backwards=True)
    x = C.placeholder()
    apply_x = C.splice(F(x), G(x))
    
    return apply_x 


def train_sequence_classifier():
    hidden_dim = 300
    embedding_dim = 300
    
    #設定ファイルの取り込み
    with open("./data/export_info.json","r") as json_file:
        json_data = json.load(json_file)
        input_dim = int(json_data["wordDimension"]) #onehotのindex数
        num_output_classes=int(json_data["labelDimension"])#topic数
        print("input_dim:",input_dim)          
    
    rel_path = r"data\cntk_train_data.tsv"
    path = os.path.join(os.path.dirname(os.path.abspath(__file__)), rel_path)
    print("filepath:",path)
    
    #トレーニングデータのフォーマット、ラベル設定。 
    features = C.sequence.input_variable(input_dim,is_sparse=True,name="features")       # word sequence (one-hot vectors)
    label = C.input_variable(num_output_classes,is_sparse=True, name="label",dynamic_axes=C.Axis.default_batch_axis())
    reader = create_reader(path, True, input_dim, num_output_classes)
    
    #訓練関数(Trainer)に渡すパラメータの作成。
    #分類器の作成(LSTMのLayer構造)
    classifier_output=lstm_sequence_classifier(features,num_output_classes,embedding_dim,hidden_dim)
    #損失関数の設定
    ce = C.cross_entropy_with_softmax(classifier_output, label)
    #エラー率の設定
    pe = C.classification_error(classifier_output, label)
    #学習率の設定
    lr_per_sample = C.learning_rate_schedule(0.05, C.UnitType.sample)        
    
    # トレーナーの構築 SGD(勾配法の一つ)。
    trainer = C.Trainer(classifier_output, (ce, pe),C.sgd(classifier_output.parameters, lr=lr_per_sample))
    
    #設定
    minibatch_size = 512 #一回トレーニング導入するデータ量 (センテンスの数ではない)
    training_progress_output_freq = 20 #何回loopしたら進捗表示する。
    loop_count = 0 #ループの数
    epoch=1 #epochの初期値
    epoch_max=10 #Maxのepoch数、Maxになるとトレーニング終了
    epoch_size=5000 #epochのサイズ(ここでは5000センテンス1 epoch)
    samples=0 #トレーニングしたセンテンス合計

    #トレーニングのループ
    while True:
        mb = reader.next_minibatch(minibatch_size, {
            features : reader.streams.features,
            label   : reader.streams.labels
        })
        samples+=mb[label].num_samples #今までトレーニングしたセンテンス数
        trainer.train_minibatch(mb)           #mbのデータをトレーニング
        training_loss, eval_crit = print_training_progress(trainer, loop_count, training_progress_output_freq,samples,epoch) #トレーニング進捗の表示       
        if samples >= epoch_size*epoch:           #毎epochトレーニング完了後モデルを保存
            classifier_output.save(os.path.join(abs_path,".","Models","lstm_model_epoch{}.dnn".format(epoch)))
            epoch += 1
        if epoch >epoch_max+1:
            break
        loop_count += 1

    import copy
    #前minibatchの精度と損失関数の計算
    evaluation_average = copy.copy(trainer.previous_minibatch_evaluation_average)
    loss_average = copy.copy(trainer.previous_minibatch_loss_average)

    return evaluation_average, loss_average

#トレーニングの進捗状況の表示
def print_training_progress(trainer, mb, frequency,samples,epoch):    
    training_loss = 100
    eval_crit = 100
    if mb % frequency == 0:
        training_loss = trainer.previous_minibatch_loss_average
        eval_crit = trainer.previous_minibatch_evaluation_average
        with open("./train_result.log","a") as f:
            if mb == 0:
                f.write("Minibatch")
                f.write("\t")
                f.write("Train Loss")
                f.write("\t")
                f.write("Train Evaluation Criterion")
                f.write("\n")

            f.write(str(mb))
            f.write("\t")
            f.write(str(training_loss))
            f.write("\t")
            f.write(str(eval_crit))
            f.write("\n")

        print("epoch:{},total samples:{},Train Loss: {}, Train Evaluation Criterion: {}".format(
                epoch,samples, training_loss, eval_crit))

    return training_loss, eval_crit

if __name__ =="__main__":
    print("CNTK version:",C.__version__)
    #学習で使用しているデバイスの表示(CPU/GPU)
    if C.device.DeviceDescriptor.type == 0:
        print("running on CPU")
    else:
        print("running on GPU")
    #開始時間
    start = time.time()
    print("Start:" + datetime.now().strftime("%H:%M:%S"))
    #トレーニング
    error, _ = train_sequence_classifier()
    #最後のエラー率
    print("Error: %f" % error)
    #終了時間
    elapsed_time = time.time() - start
    print("End:" + datetime.now().strftime("%H:%M:%S"))
    print("Elapsed Time:{0} [sec]".format(elapsed_time))
