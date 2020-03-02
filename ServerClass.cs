using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Yo_Tuk_Tuk_Epos
{
    class ServerClass
    {

        string hostName = "150.237.48.235";
        int hostNumber = 5002;
        
        internal void printReceipt()
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.NoDelay = true;

            IPAddress ip = IPAddress.Parse("192.168.192.6");
            IPEndPoint ipep = new IPEndPoint(ip, 9100);

            clientSocket.Connect(ipep);
            byte[] fileToSent = File.ReadAllBytes("printDoc");
            clientSocket.Send(fileToSent);
            clientSocket.Close();
        }

       
        internal void write(string fileName, List<string> items)
        {
            TcpClient client = new TcpClient();
            try
            {
                client.Connect(hostName, hostNumber);
                StreamWriter sw = new StreamWriter(client.GetStream());
                sw.AutoFlush = true;
                sw.WriteLine("write," + fileName + "," + string.Join(",", items.ToArray()));
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }      
            finally
            {
                client.Close();
            }
            
      
        }
        internal string read(string fileName)
        {
            TcpClient client = new TcpClient();
            string read="";
            try
            {
                client.Connect(hostName, hostNumber);
                StreamWriter sw = new StreamWriter(client.GetStream());
                sw.AutoFlush = true;
                StreamReader sr = new StreamReader(client.GetStream());
                if(fileName.Contains("itemsList"))
                {
                    sw.WriteLine("itemsList,");
                    read = sr.ReadLine();
                }
                if(fileName.Contains(".txt"))
                {
                    sw.WriteLine("read," + fileName);
                    read = sr.ReadLine();
                }
                else if(fileName.Contains("xread"))
                {
                    sw.WriteLine("requestXRead,");
                    read = sr.ReadLine();
                }
                else if(fileName==("reset"))
                {
                    sw.WriteLine(fileName + ",");

                }
              
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                
            }
            finally
            {
                client.Close();
            }
            return read;



        }
        internal void create(string fileName)
        {
            TcpClient client = new TcpClient();
            try
            {
                client.Connect(hostName, hostNumber);
                StreamWriter sw = new StreamWriter(client.GetStream());
                sw.AutoFlush = true;
                sw.WriteLine("create," + fileName);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                
            }
            finally
            {
                client.Close();
            }



        }
        internal void paid(string fileName, string info)
        {
            TcpClient client = new TcpClient();
            try
            {
                client.Connect(hostName, hostNumber);
                StreamWriter sw = new StreamWriter(client.GetStream());
                sw.AutoFlush = true;
                sw.WriteLine("paid," + fileName+","+info);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());

            }
            finally
            {
                client.Close();
            }
        }
       

    }
}
