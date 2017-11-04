using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Testy
{
    class Testy
    {
        static void Main(string[] args)
        {
            //1. wątek główny
            //2. dodanie do kolejki obsługującej wątki nowej pozycji
            //ThreadPool.QueueUserWorkItem(ThreadProc);
            //Przykładowy sposób przekazania danych do wątku
            ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { "wiadomosc 1", 2, 'a' });

            //3. kolejka obsługująca wątki uruchamia dodany wątek
            //5. wątek główny nie czeka na zakończenie dodanego wątku i zamyka program
            Thread.Sleep(1000);

        }

        static void ThreadProc(Object stateInfo)
        {

            //4. Rozpoczęcie wykonywania nowego wątku
            Thread.Sleep(500);
            Console.WriteLine("hello");

            //Pobierając dane nie zawsze musimy jawnie deklarować ich typ
            var message = ((object[])stateInfo)[0];
            var integer = ((object[])stateInfo)[1];
            var character = ((object[])stateInfo)[2];
            //ale musimy być świadomi, że zmienne mają typ przypisany
            Console.WriteLine(character.GetType());
            //mimo wszystko dane są przekazane do wątku poprawnie
            Console.WriteLine(message);




        }
    }
}
