using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadanie1
{
    class Zadanie1
    {
        static void Main(string[] args)
        {
            //Dodanie do kolejki obsługującej wątki dwóch nowych pozycji
            ThreadPool.QueueUserWorkItem(ThreadWait, new object[] { 1100 });
            ThreadPool.QueueUserWorkItem(ThreadWait, new object[] { 300 });


            Thread.Sleep(1000);
        }

        static void ThreadWait(Object stateInfo)
        {
            var czasCzekania = (int)((object[])stateInfo)[0];
            Console.WriteLine("Rozpoczęcie działania wątku.");
            Thread.Sleep(czasCzekania);
            Console.WriteLine("Czas czekania: " + czasCzekania + " ms");
        }

    }
}

/****************WNIOSKI****************
 * Czas uśpienia wątku głównego decyduje o tym, czy poczeka on na 
 * wykonanie dodanego wątku. Jeżeli jego czas czekania jest krótszy
 * niż czas wykonaywania wątku, zakończy on działanie programu przd 
 * całkowitym wykonaniem dodanego wątku ( wątek wykona tyle zadań ile
 * zdąży w tym czasie). 
 * 
 * 
 * 
 * 
 */
