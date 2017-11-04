using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadanie2
{
    class Zadanie2
    {
        static void Main(string[] args)
        {
            //dodanie serwera do puli wątków
            ThreadPool.QueueUserWorkItem(Server);
            //dodanie dwóch klientów do kolejki obsługującej wątki
            ThreadPool.QueueUserWorkItem(Klient, "Klient_1");
            ThreadPool.QueueUserWorkItem(Klient, "Klient_2");

            Thread.Sleep(1000);
        }

        static void Server(Object stateInfo)
        {

            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 2048);
            server.Start();

            byte[] buffer = new byte[256];
            String data = null;
            string server_hello = "Hello (od servere)";
            Console.WriteLine("Czekanie na połączenie... ");
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Połączono!");

                NetworkStream stream = client.GetStream();

                int n;
                while ((n = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(buffer, 0, n);
                    Console.WriteLine("Server dostaje " + data);


                    Console.WriteLine("Server wysyła " + server_hello);
                    byte[] wiadomość = System.Text.Encoding.ASCII.GetBytes(server_hello.ToCharArray());
                    stream.Write(wiadomość, 0, wiadomość.Length);
                }

                client.Close();
            }

        }

        static void Klient(Object stateInfo)
        {

            String klient_hello = "Hello (od klienta)";


            TcpClient client = new TcpClient();

            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            NetworkStream stream = client.GetStream();
            byte[] buffer = System.Text.Encoding.ASCII.GetBytes(klient_hello);

            Console.WriteLine(stateInfo.ToString() + " wysyła " + klient_hello);
            stream.Write(buffer, 0, buffer.Length);

            buffer = new byte[256];


            int i = stream.Read(buffer, 0, buffer.Length);

            String data = System.Text.Encoding.ASCII.GetString(buffer);

            Console.WriteLine(stateInfo.ToString() + " dostaje " + data);

            stream.Close();
            //uśpienie klienta
            Thread.Sleep(10000);


        }

    }
}

/****************WNIOSKI****************
 * W takim rozwiązaniu w tym samym czasie serwer może
 * nawiązać połączenie z jednym klientem. Dopiero gdy ten
 * zostanie uśpiony serwer może nawiązać połączenie z kolejnym
 * klientem.
 * 
 * 
 * 
 */
