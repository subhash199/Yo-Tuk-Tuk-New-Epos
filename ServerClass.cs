using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Yo_Tuk_Tuk_Epos
{
    class ServerClass
    {

        string hostName = "localhost";
        int hostNumber = 5002;
        TcpClient client = new TcpClient();

       
        internal void write(string fileName, List<string> items)
        {
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
            string read="";
            try
            {
                client.Connect(hostName, hostNumber);
                StreamWriter sw = new StreamWriter(client.GetStream());
                sw.AutoFlush = true;
                StreamReader sr = new StreamReader(client.GetStream());
                sw.WriteLine("read," + fileName);
                read = sr.ReadLine();
              
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
        internal void close()
        {
            client.Close();
        }

    }
}
