using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IO_4
{
    class Program
    {

        class Server
        {
            TcpListener server;
            int port;
            IPAddress address;
            CancellationTokenSource cts = new CancellationTokenSource();
            bool running = false;
            Task serverTask;

            public Task ServerTask
            {
                get { return serverTask; }
            }
            public IPAddress Address
            {
                get { return address; }
                set
                {
                    if (!running) address = value;
                    else;
                }
            }
            public int Port
            {
                get { return port; }
                set
                {
                    if (!running)
                        port = value;
                    else;
                }
            }

            public Server()
            {
                Address = IPAddress.Any;
                port = 2048;
            }

            public Server(int port)
            {
                this.port = port;
            }

            public Server(IPAddress address)
            {
                this.address = address;
            }


            private async Task RunAsync(CancellationToken ct)
            {
                TcpListener server = new TcpListener(address, port);
                try
                {
                    server.Start();
                    running = true;
                }
                catch (Exception e)
                {
                    throw (e);

                }

                while (true && !ct.IsCancellationRequested)
                {
                    TcpClient client = await server.AcceptTcpClientAsync();
                    byte[] buffer = new byte[1024];
                    using (ct.Register(() => client.GetStream().Close()))
                    {
                        await client.GetStream().ReadAsync(buffer, 0, buffer.Length).ContinueWith(
                           async (t) =>
                           {
                               int i = t.Result;
                               while (true)
                               {
                                   client.GetStream().WriteAsync(buffer, 0, i);
                                   try
                                   {
                                       i = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
                                   }
                                   catch
                                   {
                                       break;
                                   }
                               }
                           });
                    }
                }
            }

            public void RequestCancellation()
            {
                cts.Cancel();
                server.Stop();
            }
            public void Run()
            {
                serverTask = RunAsync(cts.Token);
                //Task.WaitAll(serverTask1(cts.Token));  
                //jeżeli tu to zrobimy to zablokujemy programiscie dostep , trzeba zrobić w Main
            }
            public void StopRunning()
            {
                RequestCancellation();
            }

        }




        class Client
        {

            TcpClient client;

            public void Connect()
            {
                client = new TcpClient();
                //client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
                client.Connect(IPAddress.Loopback, 2048);
            }

            public async Task<string> Ping(string message)
            {
                byte[] buffer = new ASCIIEncoding().GetBytes(message);
                await client.GetStream().WriteAsync(buffer, 0, buffer.Length);
                buffer = new byte[1024];
                var t = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);

                return Encoding.UTF8.GetString(buffer, 0, t);
            }

            public async Task<IEnumerable<string>> keepPinging(string message, CancellationToken token)
            {
                List<string> messages = new List<string>();
                bool done = false;
                while (!done)
                {
                    if (token.IsCancellationRequested)
                        done = true;
                    messages.Add(await Ping(message));
                    Console.WriteLine(message);

                }

                return messages;
            }
        }


        static void Main(string[] args)
        {
            Server s = new Server();
            s.Run();
            //Task.WaitAll(s.ServerTask);
            Client c1 = new Client();
            Client c2 = new Client();

            c1.Connect();
            c2.Connect();

            CancellationTokenSource ct1 = new CancellationTokenSource();
            CancellationTokenSource ct2 = new CancellationTokenSource();

            var client1T = c1.keepPinging("here_client1", ct1.Token);
            var client2T = c2.keepPinging("here_client2", ct2.Token);

            ct1.CancelAfter(2000);
            ct2.CancelAfter(3000);

            Task.WaitAll(client1T);
            Task.WaitAll(client2T);
         
            s.StopRunning();

        }
    }
}