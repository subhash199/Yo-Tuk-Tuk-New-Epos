﻿using Habanero.Faces.Base;
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

        string hostName = File.ReadAllText("host.txt").Trim();        
        int hostNumber = int.Parse(File.ReadAllText("port.txt").Trim());
        
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
        internal void print(string list)
        {
            TcpClient client = new TcpClient();
            try
            {
                client.Connect(hostName, hostNumber);
                StreamWriter sw = new StreamWriter(client.GetStream());
                
                client.SendTimeout = 1000;
                sw.AutoFlush = true;
                sw.WriteLine(list);
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

       
        internal string write(string fileName, List<string> items)
        {
            string read = "";
            TcpClient client = new TcpClient();
            try
            {
                client.Connect(hostName, hostNumber);
                StreamWriter sw = new StreamWriter(client.GetStream());
                StreamReader sr = new StreamReader(client.GetStream());
                client.ReceiveTimeout = 1000;
                client.SendTimeout = 1000;
                sw.AutoFlush = true;
                sw.WriteLine("write," + fileName + "," + string.Join(",", items.ToArray()));
                read = sr.ReadLine();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }      
            finally
            {
                client.Close();
                
            }
            return read;


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
                //client.ReceiveTimeout = 1000;
                //client.SendTimeout = 1000;
                if (fileName.Contains("itemsList"))
                {
                    sw.WriteLine("itemsList,");
                    read = sr.ReadLine();
                    
                }
                else if(fileName.Contains(".txt"))
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
                else if (fileName.Contains("listAll"))
                {
                    sw.WriteLine(fileName+",");
                    read = sr.ReadLine();
                }
               else
                {
                    sw.WriteLine(fileName);
                    read = sr.ReadLine();
                }

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
               if(MessageBox.Show("Is the server active?", "Server-Connection",MessageBoxButton.YesNo)==MessageBoxResult.Yes)
                {
                    HostWindow window = new HostWindow();
                    window.ShowDialog();       
                }
                
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
                string read = "";
                client.Connect(hostName, hostNumber);
                StreamWriter sw = new StreamWriter(client.GetStream());
                StreamReader sr = new StreamReader(client.GetStream());
                client.SendTimeout = 1000;
                sw.AutoFlush = true;
                if (fileName.Contains(".txt"))
                {
                    sw.WriteLine("create," + fileName);
                }
                else
                {
                   sw.WriteLine(fileName);
                   if( sr.ReadLine()=="OK")
                    {
                        MessageBox.Show("Updated successfully!");
                    }
                   
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
