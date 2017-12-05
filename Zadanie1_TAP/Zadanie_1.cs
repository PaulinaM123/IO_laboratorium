using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Zadanie1_TAP
{
    class Zadanie_1
    {

        public struct TResultDataStructure
        {
            //zadanie 1
            int l1, l2;

            public int L1 { get => l1; set => l1 = value; }
            public int L2 { get => l2; set => l2 = value; }

            public TResultDataStructure(int x1, int x2)
            {
                l1 = x1;
                l2 = x2;
            }
        }
        public Task<TResultDataStructure> AsyncMethod1(byte[] buffer)
        {
            TaskCompletionSource<TResultDataStructure> tcs = new TaskCompletionSource<TResultDataStructure>();
            Task.Run(() =>
            {
                tcs.SetResult(new TResultDataStructure(12, 5));
            });
            return tcs.Task;
        }
        public TResultDataStructure Zadanie1()
        {
            var task = AsyncMethod1(null);
            task.Wait();
            return task.GetAwaiter().GetResult();
        }

        static void Main(string[] args)
        {
            /*Zadanie1_testowanie*/
            Zadanie_1 z = new Zadanie_1();
            Tuple<int, int> twoIntegerResult = new Tuple<int, int>(3, 3);
            object[] p1 = twoIntegerResult.GetType().GetProperties();
            object[] p2 = z.Zadanie1().GetType().GetProperties();
            Console.WriteLine(p1.Length +" # "+ p2.Length);
            Console.WriteLine(p1[0].GetType().ToString() +" # "+ p2[0].GetType().ToString());
            Console.WriteLine(p1[1].GetType().ToString() +" # "+ p2[1].GetType().ToString());

           
            var p = z.Zadanie1();
            Console.WriteLine(p.L1 +" # " + p.L2);
        }
    }
}
