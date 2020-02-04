using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Yo_Tuk_Tuk_Epos
{
    class ServerClass
    {
        string hostName = "localhost";
        int hostNumber = 5002;
        TcpClient client = new TcpClient();
        

        internal void write()
        {            
            client.Connect(hostName, hostNumber);
            StreamWriter sw = new StreamWriter(client.GetStream());            
            sw.AutoFlush = true;
        }
        internal string [] read(string fileName)
        {
            return File.ReadAllLines(fileName);
            
        }
        internal void create(string folderName, string fileName) 
        {
            client.Connect(hostName, hostNumber);
            StreamWriter sw = new StreamWriter(client.GetStream());
            sw.AutoFlush = true;
           
            sw.WriteLine("create"+","+folderName+","+ fileName);           

        }
    }
}
