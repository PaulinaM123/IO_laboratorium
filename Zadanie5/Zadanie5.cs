using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadanie5
{
    class Zadanie5
    {
        private static readonly Object thisLock = new Object();
        static int suma = 0;
        static int[] tablica;

        static void zadanie(int rozmiar, int part_size)
        {
           
            tablica = new int[rozmiar];


            Random rand = new Random();
            for (int i = 0; i < tablica.Length; ++i)
            {
                tablica[i] = rand.Next(0, 20);
                
            }

            int wątki;
            int ostatni = 0;
            if (rozmiar % part_size == 0)
            {
                wątki = rozmiar / part_size;
            }
            else
            {
                wątki = rozmiar / part_size + 1;
                ostatni = rozmiar % part_size;
            }

            foreach (var a in tablica)
            {
                Console.Write(a + " ");
            }
            Console.Write("\n");

            AutoResetEvent[] events = new AutoResetEvent[wątki];

            for (int i = 0; i < wątki; i++)
            {

                events[i] = new AutoResetEvent(false);
                if (i == wątki - 1 && ostatni != 0)
                {
                    ThreadPool.QueueUserWorkItem(Dodaj, new object[] { i * part_size, ostatni, events[i], "Wątek_ " + i });
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(Dodaj, new object[] { i * part_size, part_size, events[i], "Wątek_ " + i });
                }
            }


            WaitHandle.WaitAll(events);
            Console.WriteLine("Wynik sumowania: " + suma.ToString());
            Console.WriteLine("Rozmiar tablicy: " + rozmiar);
            Console.WriteLine("Liczba wątków: " + wątki);
            Console.WriteLine("Rozmiar części: " + part_size);
        }

        static void Dodaj(Object StateInfo)
        {
            int pierwszy = (int)((object[])StateInfo)[0];
            int ostatni = (int)((object[])StateInfo)[1];
            AutoResetEvent event1 = (AutoResetEvent)((object[])StateInfo)[2];
            string numer = (string)((object[])StateInfo)[3];




            int liczba=0;

            for (int i = pierwszy; i < ostatni; i++)
            {
                lock (thisLock)
                {
                    liczba = tablica[i];
                }
                suma += liczba;
            }

            event1.Set();
        }



        static void Main(string[] args)
        {
            zadanie(750, 50);

        }

    }
}
