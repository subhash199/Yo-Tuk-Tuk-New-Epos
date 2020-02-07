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
        

        internal void write(string fileName,List <string> items)
        {            
            client.Connect(hostName, hostNumber);
            StreamWriter sw = new StreamWriter(client.GetStream());            
            sw.AutoFlush = true;         
            sw.WriteLine("write," + fileName +","+ string.Join(",",items.ToArray()));
        }
        internal string read(string fileName)
        {
            client.Connect(hostName, hostNumber);
            StreamWriter sw = new StreamWriter(client.GetStream());
            sw.AutoFlush = true;
            StreamReader sr = new StreamReader(client.GetStream());
            sw.WriteLine("read," + fileName);
            string read = sr.ReadLine();
            return read;


        }
        internal void create(string fileName) 
        {
            client.Connect(hostName, hostNumber);
            StreamWriter sw = new StreamWriter(client.GetStream());
            sw.AutoFlush = true;
            sw.WriteLine("create,"+fileName);                  
            sw.Close();

        }
    }
}
