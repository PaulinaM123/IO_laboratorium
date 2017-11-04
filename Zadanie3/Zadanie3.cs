using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadanie3
{
    class Zadanie3
    {
        static void Main(string[] args)
        {
            //dodanie serwera do puli wątków
            ThreadPool.QueueUserWorkItem(Server);
            //dodanie dwóch klientów
            ThreadPool.QueueUserWorkItem(Klient, "Klient_1");
            ThreadPool.QueueUserWorkItem(Klient, "Klient_2");
            // ThreadPool.QueueUserWorkItem(Klient, "Klient_3");


            Thread.Sleep(10000);
        }


        static void Server(Object stateInfo)
        {

            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 2048);
            server.Start();
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(NowyWątek, client);
                Console.WriteLine("Połączono!");
            }

        }

        static void NowyWątek(Object stateInfo)
        {
            TcpClient client = (TcpClient)stateInfo;

            byte[] buffer = new byte[256];
            String data = null;
            string server_hello = "Hello(od server)";

            NetworkStream stream = client.GetStream();
            int i;

            while ((i = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                data = System.Text.Encoding.ASCII.GetString(buffer, 0, buffer.Length);
                writeConsoleMessage("Serwer:  Otrzymałem wiadomość:  " + data, ConsoleColor.Red);

                Console.WriteLine("Server wysyła " + server_hello);
                byte[] wiadomość = System.Text.Encoding.ASCII.GetBytes(server_hello.ToCharArray());
                stream.Write(wiadomość, 0, wiadomość.Length);
            }
            stream.Close();
        }

        private static void writeConsoleMessage(string v, ConsoleColor kolor)
        {
            Console.ForegroundColor = kolor;
            Console.WriteLine(v);
            Console.ResetColor();
        }

        static void Klient(Object stateInfo)
        {

            String klient_hello = "Hello(od klient)";


            TcpClient client = new TcpClient();

            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            NetworkStream stream = client.GetStream();
            byte[] buffer = System.Text.Encoding.ASCII.GetBytes(klient_hello);

            Console.WriteLine(stateInfo.ToString() + " wysyła " + klient_hello);
            stream.Write(buffer, 0, buffer.Length);

            buffer = new byte[256];


            int bytes = stream.Read(buffer, 0, buffer.Length);

            String data = System.Text.Encoding.ASCII.GetString(buffer);
            writeConsoleMessage(stateInfo.ToString() + ": Otrzymałem wiadomość:  " + data, ConsoleColor.Green);

            client.Close();

        }



    }
}
/****************WNIOSKI****************
 * W takim rozwiązaniu w tym samym czasie serwer może
 * nawiązać połączenie wieloma klientem. Połączenie z 
 * danym klientem wykonywane jest w nowym wątku, co nie
 * przeszkadza serwerowi w nawiązaniu kolejnego połączenia.
 *
 ** Kolorowanie 
 * W przeprowadzonych przeze mnie kilku testach ujawnia się błąd synchronizacji kolorowania. 
 * Kolory nie są nadawane poprwanie.
 * Metoda kolorująca nie jest zsynchronizowana z działaniami klienta i serwera.
 * 
 * 
 */

