using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Net.Sockets;


namespace Yo_Tuk_Tuk_Epos
{
    /// <summary>
    /// Interaction logic for settings.xaml
    /// </summary>
    public partial class settings : Window
    {
        static double cash = 0;
        static double card = 0;
        static double Discount = 0;
        static double total = 0;
        ServerClass serverRequest = new ServerClass();
        RestaurantLayout currentLayout;
        public settings(RestaurantLayout layout)
        {
            currentLayout = layout;
            InitializeComponent();
        }

        private void DayEnd_Click(object sender, RoutedEventArgs e)
        {
            string[] read = (serverRequest.read("xread")).Split(',');


            for (int i = 0; i < read.Length; i++)
            {
                if (read[i] == "cash")
                {
                    cash += double.Parse(read[i - 4]);


                }
                else if (read[i] == "card")
                {
                    card += double.Parse(read[i - 4]);
                }
                else if (read[i] == "discount")
                {
                    Discount += double.Parse(read[i + 1]);
                }
            }
            total = cash + card;
            printXread();
            if (MessageBox.Show("Would you like to Reset X-Read?", "Reset", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                serverRequest.read("reset");
            }



        }

        private void printXread()
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(xPrinter);
            printDocument.Print();

        }

        private void xPrinter(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Font font = new Font("Arial", 12);

            int startX = 0;
            int startY = 0;
            int offSet = 20;

            graphics.DrawString("Day End".PadRight(10) + DateTime.Now, font, new SolidBrush(System.Drawing.Color.Black), 100, 0 + 0);
            offSet += 20;
            graphics.DrawString("Cash = ".PadRight(10) + cash, font, new SolidBrush(System.Drawing.Color.Black), 0, 0 + 0);
            offSet += 20;
            graphics.DrawString("Card = ".PadRight(10) + card, font, new SolidBrush(System.Drawing.Color.Black), 100, 0 + 0);
            offSet += 20;
            graphics.DrawString("Discount = ".PadRight(10) + Discount, font, new SolidBrush(System.Drawing.Color.Black), 100, 0 + 0);
            offSet += 20;
            graphics.DrawString("Total Sales = ".PadRight(10) + total, font, new SolidBrush(System.Drawing.Color.Black), 100, 0 + 0);
            offSet += 20;

        }
    }
}
