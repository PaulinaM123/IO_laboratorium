using System;
using System.IO;

namespace Zadanie7
{
    class Zadanie7
    {
        static void Main(string[] args)
        {

            FileStream fs = new FileStream("tekst.txt", FileMode.Open);

            byte[] buffer = new byte[fs.Length]; 


            var ar = fs.BeginRead(buffer, 0, buffer.Length, null, null);
            //tutaj wykonywane sa obliczenia asynchronicznie 

            int i = fs.EndRead(ar);
            fs.Close();

            Console.WriteLine(System.Text.Encoding.ASCII.GetString(buffer, 0, i));

        }
    }
}

/****************WNIOSKI****************
 * Wątek główny czeka na zakończenie operacji czytania.
 * Dzięki takiemu rozwiązaniu mamy pewność, że wątek poboczny 
 * zostanie wykonany w całości.
 */
