using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JBS.ChatBot.Service;

namespace JBS.ChatBot
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            { 
                Console.Write("質問してください:");
                var boot = new DocumentAnalysisService();
                var question = Console.ReadLine();
                if (question == "exit") break;
                var answer = boot.Analyze(question);
                Console.WriteLine("Answer：" + (answer!=null ? answer.Label:"ごめんなさい。質問がわかりませんでした。"));
                Console.WriteLine("Score:" + (answer != null ? answer.Score.ToString(): "--"));
                Console.WriteLine("");
                Console.WriteLine("ボットの利用終了するにはexitを入力してくだい。");
            }
        }
    }
}
