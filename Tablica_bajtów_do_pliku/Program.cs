using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tablica_bajtów_do_pliku
{
    class Program
    {

        public static async Task Zapisz_do_pliku_Async(string path, byte[] buffer)
        {
            using (FileStream stream = File.Open(path, FileMode.OpenOrCreate))
            {
                stream.Seek(0, SeekOrigin.End);
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
        }


        static void Main(string[] args)
        {

            string plik = "przykładowy_tekst.txt";
            string tekst = "To jest przykładowy tekst.";

            Encoding u = Encoding.Unicode;

            byte[] buffer = u.GetBytes(tekst);

            Task t = Zapisz_do_pliku_Async(plik, buffer);
            t.Wait();

            Console.ReadKey();
        }


    }
}
