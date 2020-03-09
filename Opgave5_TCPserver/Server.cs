using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ClassLibrary;
using Newtonsoft.Json;

namespace Opgave5_TCPserver
{
    public class Server
    {
        private static List<Bog> Library = new List<Bog>
        {
            new Bog("UML","Larman",654,"9780133594140"),
            new Bog("The Grass is Always Greener","Jeffrey Archer",200 ,"1-86092-049-7"),
            new Bog("Murder!","Arnold Bennett (1867-1931)",700,"1-86092-012-8")
        };


        public void DoClient(TcpClient socket)
        {
            using (socket)
            {
                NetworkStream ns = socket.GetStream();
                StreamReader sr = new StreamReader(ns);
                StreamWriter wr = new StreamWriter(ns);
                wr.AutoFlush = true;

                string line;

                while (true)
                {
                    line = sr.ReadLine();

                    string[] SplitStr = line.Split(" ");

                    if (SplitStr[0] == "GetAll")
                    {
                        wr.WriteLine(JsonConvert.SerializeObject(Library));
                    }

                    if (SplitStr[0] == "Get")
                    {
                        if(SplitStr.Length == 2)
                        {
                           Bog book = Library.Find(x => x.ISBN13 == SplitStr[1]);
                           wr.WriteLine(JsonConvert.SerializeObject(book));
                        }
                    }

                    if (SplitStr[0] == "Save")
                    {
                        if (SplitStr.Length >= 2)
                        {
                            Bog newBook = JsonConvert.DeserializeObject<Bog>(SplitStr[1]);
                            Console.WriteLine("Adding new book: " + newBook.Title);
                            Library.Add(newBook);

                            
                        }
                    }



                    Console.WriteLine("Client: "+ line);

                    

                }

            }
        }

        public void StartServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, 4646);
            listener.Start();
            Console.WriteLine("Server has started");


            while(true)
            {
                TcpClient socket = listener.AcceptTcpClient();
                Console.WriteLine("Client Attepted");

                Task.Run(() =>
                {
                    TcpClient tempSocket = socket;
                    DoClient(tempSocket);
                });
            }
        }
    }
}
