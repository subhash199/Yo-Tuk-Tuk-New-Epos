using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;


namespace Yo_Tuk_Tuk_Epos
{
    /// <summary>
    /// Interaction logic for RestaurantLayout.xaml
    /// </summary>
    public partial class RestaurantLayout : Window
    {
        private MainWindow window = null;
        ServerClass server = new ServerClass();
        public RestaurantLayout(MainWindow mainWindow)
        {
            
            InitializeComponent();
            window = mainWindow;
            

        }
        string fileName = "";       
        private void _1btn_Click(object sender, RoutedEventArgs e)
        {           
            TableNumber("Table_1", _1btn);     
           
        }

        private void _2btn_Click(object sender, RoutedEventArgs e)
        {
         
            TableNumber("Table_2", _2btn);
        }

        private void _3btn_Click(object sender, RoutedEventArgs e)
        {
            TableNumber("Table_3", _3btn);

        }

        private void _4btn_Click(object sender, RoutedEventArgs e)
        {
            TableNumber("Table_4", _4btn);

        }

        private void _5btn_Click(object sender, RoutedEventArgs e)
        {
            TableNumber("Table_5", _5btn);
        }

        private void _6btn_Click(object sender, RoutedEventArgs e)
        {
            TableNumber("Table_6", _6btn);
        }

        private void _7btn_Click(object sender, RoutedEventArgs e)
        {
            TableNumber("Table_7", _7btn);
        }

        private void b1btn_Click(object sender, RoutedEventArgs e)
        {
            TableNumber("Bar_1", b1btn);
        }

        private void b2btn_Click(object sender, RoutedEventArgs e)
        {
            TableNumber("Bar_2", b2btn);
        }

        private void b3btn_Click(object sender, RoutedEventArgs e)
        {
            TableNumber("Bar_3", b3btn);
        }

        private void b4btn_Click(object sender, RoutedEventArgs e)
        {
            TableNumber("Bar_4", b4btn);
        }

        private void b5btn_Click(object sender, RoutedEventArgs e)
        {
            TableNumber("Bar_5", b5btn);
        }

        private void b6btn_Click(object sender, RoutedEventArgs e)
        {
            TableNumber("Bar_6", b6btn);
        }

        private void b7btn_Click(object sender, RoutedEventArgs e)
        {
            TableNumber("Bar_7", b7btn);
        }

        private void _8btn_Click(object sender, RoutedEventArgs e)
        {
            TableNumber("Table_8", _8btn);
        }

        private void _9btn_Click(object sender, RoutedEventArgs e)
        {
            TableNumber("Table_9", _9btn);
        }

        private void _10btn_Click(object sender, RoutedEventArgs e)
        {
            TableNumber("Table_10", _10btn);
        }

        private void _11btn_Click(object sender, RoutedEventArgs e)
        {
            TableNumber("Table_11", _11btn);
        }

        private void _12btn_Click(object sender, RoutedEventArgs e)
        {
            TableNumber("Table_12", _12btn);
        }

        private void _13btn_Click(object sender, RoutedEventArgs e)
        {
            TableNumber("Table_13", _13btn);
        }

        private void _14btn_Click(object sender, RoutedEventArgs e)
        {
            TableNumber("Table_14", _14btn);
        }

        private void _15btn_Click(object sender, RoutedEventArgs e)
        {
            TableNumber("Table_15", _15btn);
        }

        private void _16btn_Click(object sender, RoutedEventArgs e)
        {
            TableNumber("Table_16", _16btn);
        }

        private void _17btn_Click(object sender, RoutedEventArgs e)
        {
            TableNumber("Table_17", _17btn);
        }

        private void _18btn_Click(object sender, RoutedEventArgs e)
        {

            TableNumber("Table_18",_18btn);
            
        }
        private void TableNumber(string TableNumber, Button pButton)
        {
            fileName = TableNumber + ".txt";
            server.create(fileName);        

            RestaurantMenu menu = new RestaurantMenu();          
            menu.FolderFileName(fileName);
            menu.ShowDialog();
            if(menu.DialogResult==true)
            {
                pButton.Background = Brushes.Red;
                window.layout = this;
                window.Show();                
                this.Hide();
                
            }
                  
           

        }
        //private void isFileExist(string fileName, Button pName)
        //{
        //    if (File.Exists(fileName))
        //    {
        //        if (new FileInfo(fileName).Length == 0)
        //        {
        //            pName.Background = (Brush)new BrushConverter().ConvertFrom("#FF4EE715");
        //        }
        //        else
        //        {
        //            pName.Background = Brushes.Red;
        //        }

        //    }
        //    else
        //    {
        //        pName.Background = (Brush)new BrushConverter().ConvertFrom("#FF4EE715");
        //    }


        //}
    }
}
