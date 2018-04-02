# Copyright (c) Microsoft. All rights reserved.

# Licensed under the MIT license. See LICENSE.md file in the project root
# for full license information.
# ==============================================================================

import sys
import os
import time
import json
from datetime import datetime
import cntk
from cntk.layers import *
import pdb

abs_path = os.path.dirname(os.path.abspath(__file__))
sys.path.append(os.path.join(abs_path, ".", "common"))

# Creates the reader
def create_reader(path, is_training, input_dim, label_dim):
    return cntk.io.MinibatchSource(cntk.io.CTFDeserializer(path, cntk.io.StreamDefs(
        features = cntk.io.StreamDef(field='CW', shape=input_dim,   is_sparse=True),
        labels   = cntk.io.StreamDef(field='CT', shape=label_dim,   is_sparse=True)
    )), randomize=is_training, max_sweeps = cntk.io.INFINITELY_REPEAT if is_training else 1)


def load_eval_data():
    with open('./data/export_info.json', 'r') as json_file:
            json_data = json.load(json_file)
            input_dim = int(json_data['wordDimension'])
            num_output_classes=int(json_data['labelDimension'])
            print("input_dim:",input_dim)
            
    rel_path = r"data\cntk_eval_data.tsv"
    path = os.path.join(os.path.dirname(os.path.abspath(__file__)), rel_path)
    print("filepath:",path)
    reader = create_reader(path, False, input_dim, num_output_classes)
    print("read eval_data done")
    return reader,input_dim,num_output_classes


def load_model():
    model=cntk.Function.load(os.path.join(abs_path, ".", "Models","lstm_model_epoch10.dnn"))
    return model

def evaluate(reader, model,input_dim,num_output_classes):
    label_to_dense = create_sparse_to_dense(num_output_classes)
    
    count=0
    num_total=0
    err_total=0
    
    outputs=[]
    confidences=[]
    labels=[]
    print(reader.get_next_minibatch)
    while True:
       mb = reader.next_minibatch(1000)
       if not mb: 
           break
       outputs.extend([np.argmax(output) for output in model.eval(mb[reader.streams.features])])
       confidences.extend([np.max(output) for output in model.eval(mb[reader.streams.features])])
       labels.extend([np.argmax(label) for label in label_to_dense(mb[reader.streams.labels])])
    #Accuracy=TP/TP+FP+N
    #Precision=TP/TP+FP
    #Recall=TP/TP+N
    for i in [0.1,0.2,0.3,0.4,0.5,0.6,0.7,0.8,0.9]:
        TP=sum([label == output for output, confidence,label in zip(outputs,confidences,labels) if confidence >=i])
        FP=sum([label != output for output, confidence,label in zip(outputs,confidences,labels) if confidence >=i])
        N=sum([label for output, confidence,label in zip(outputs,confidences,labels) if confidence<i])
        Accuracy=100*TP/(TP+FP+N)
        Recall=100*TP/(TP+N)
        Precision=100*TP/(TP+FP)
        print("Confidence {} Accuracy:{:.2f}% Recall:{:.2f}% Precisoin:{:.2f}%".format(i,Accuracy,Recall,Precision))
    err_total = sum([label != output for output, confidence,label in zip(outputs,confidences,labels)])
    num_total = len(outputs)
    rate = 100*err_total/num_total
    print("error rate of {:.2f}%. {} error in {} samples".format(rate,err_total,num_total))
    return count

# dummy for printing the input sequence below. Currently needed because input is sparse.
def create_sparse_to_dense(input_vocab_dim):
    inputAxis = C.Axis('inputAxis')
    InputSequence = C.layers.SequenceOver[inputAxis]
    I = C.Constant(np.eye(input_vocab_dim))
    @C.Function
    @C.layers.Signature(InputSequence[C.layers.SparseTensor[input_vocab_dim]])
    def no_op(input):
        return C.times(input, I)
    return no_op

if __name__ == '__main__':

    start = time.time()
    print('Start:' + datetime.now().strftime("%H:%M:%S"))
    
    eval_data,input_dim,num_output_classes=load_eval_data()
    model=load_model()
    e=evaluate(eval_data,model,input_dim,num_output_classes)
    elapsed_time = time.time() - start
    print('End:' + datetime.now().strftime("%H:%M:%S"))
    print('Elapsed Time:{0} [sec]'.format(elapsed_time))
