using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Sockets;

namespace Yo_Tuk_Tuk_Epos
{
    class Client
    {
        private TcpClient TcpClient;
        private static Client Singleton = null;

        private Client()
        {
            TcpClient = new TcpClient();
                TcpClient.Connect("localhost", 8000);
                TcpClient.ReceiveTimeout = 1000;
                TcpClient.SendTimeout = 1000;
        }

        internal static Client Get()
        {
            if (Singleton is null)
            {
                Singleton = new Client();
            }

            return Singleton;
        }
    }
}
