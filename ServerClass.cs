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

        string hostName = "150.237.68.206";
        int hostNumber = 43;
        

       
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
       

    }
}
