using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadanie4
{
    class Zadanie4
    {
        private static readonly Object thisLock = new Object();
        static void Main(string[] args)
        {
            //dodanie serwera do puli wątków
            ThreadPool.QueueUserWorkItem(Server);

            //dodanie dwóch klientów
            ThreadPool.QueueUserWorkItem(Klient, "Klient_1");
            ThreadPool.QueueUserWorkItem(Klient, "Klient_2");
            ThreadPool.QueueUserWorkItem(Klient, "Klient_3");
            ThreadPool.QueueUserWorkItem(Klient, "Klient_4");

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
                writeConsoleMessage("Serwer - Otrzymałem wiadomość:  " + data, ConsoleColor.Red);

                Console.WriteLine("Server wysyła " + server_hello);
                byte[] wiadomość = System.Text.Encoding.ASCII.GetBytes(server_hello.ToCharArray());
                stream.Write(wiadomość, 0, wiadomość.Length);
            }
            stream.Close();
        }

        private static void writeConsoleMessage(string v, ConsoleColor kolor)
        {
            lock (thisLock)
            {
                Console.ForegroundColor = kolor;
                Console.WriteLine(v);
                Console.ResetColor();
            }
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
 * W takim rozwiązaniu "lock" blokuje dostęp do części 
 * kodu znajdującej się w klamrach innym wątkom do momentu
 * aż wątek znajdujący się w tej sekcji jej nie zakończy.
 * Pozostałe wątki czekają aż dostęp zostanie odblokowany.
 * W sekcji może w tym samym czasie przebywać tylko jeden wątek.
 * 
 */
