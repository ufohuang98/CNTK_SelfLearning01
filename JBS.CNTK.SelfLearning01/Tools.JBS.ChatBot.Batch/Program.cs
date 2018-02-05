using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JBS.ChatBot.Batch.Common;

namespace JBS.ChatBot.Batch
{
    public static class Program
    {


        public static void Main(string[] args)
        {
            Console.WriteLine("begin.");
            var task = new CreateCntkTrainDataTasks();
            task.Run();
            Console.WriteLine("completed...");
            Console.WriteLine("hit any key...");
            Console.ReadKey();
        }
    }
}