using System;

namespace Opgave5_TCPserver
{
    class Program
    {
        static void Main(string[] args)
        {


            Console.WriteLine("Starting Server");
            Server server = new Server();
            server.StartServer();

            Console.ReadKey();
        }
    }
}
