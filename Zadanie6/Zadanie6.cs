using System;
using System.IO;
using System.Text;
using System.Threading;


namespace Zadanie6
{
    class Zadanie6
    {
        static AutoResetEvent event_1 = new AutoResetEvent(false);
        

        static void Main(string[] args)
        {
            FileStream fs = new FileStream("tekst.txt", FileMode.Open);

            byte[] buffer = new byte[fs.Length];
            fs.BeginRead(buffer, 0, buffer.Length, myAsyncCallback, new object[] { fs, buffer });
            //ostatni parametr- co przekazujemy do callback'a
            //jak skończy czytać, to zrobi to co jest w callback'u
            event_1.WaitOne();
        }
          
        static void myAsyncCallback(IAsyncResult state)
        {
            FileStream fs = ((object[])(state.AsyncState))[0] as FileStream;
            byte[] buffer = (byte[])((object[])state.AsyncState)[1];


            int bytesRead = fs.EndRead(state);

            Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, bytesRead));

             event_1.Set();
            fs.Close();
        }

    }
}

/****************WNIOSKI****************
 * Wątek główny nie czeka na zakończenie operacji callback.
 * W związku z tym konieczne jest wykorzystanie metody AutoResetEvent.
 */
