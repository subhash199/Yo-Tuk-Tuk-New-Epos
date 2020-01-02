using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
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
       
        public RestaurantLayout()
        {
            InitializeComponent();
        }
        string fileName = "";
        private void _1btn_Click(object sender, RoutedEventArgs e)
        {
            //RestaurantMenu Table_1 = new RestaurantMenu();
            //TableNumber("Table_1");               
            //Table_1.FolderFileName(folderName, "Table_1.txt");
            //Table_1.ShowDialog();
            TableNumber("Table_1");
            isFileExist(fileName, _1btn);
            

        }

        private void _2btn_Click(object sender, RoutedEventArgs e)
        {
            //RestaurantMenu Table_2 = new RestaurantMenu();
            //string folderName = TableNumber("Table_2");
            //Table_2.FolderFileName(folderName, "Table_2.txt");
            //Table_2.Show();
            //if (File.Exists("Table_2.txt"))
            //{
            //    if (new FileInfo("Table_2.txt").Length == 0)
            //    {
            //        _1btn.Background = (Brush)new BrushConverter().ConvertFrom("#FF4EE715");
            //    }
            //    else
            //    {
            //        _1btn.Background = Brushes.Red;
            //    }

            //}
            //else
            //{
            //    _1btn.Background = (Brush)new BrushConverter().ConvertFrom("#FF4EE715");
            //}
            TableNumber("Table_2");
            isFileExist(fileName, _2btn);


        }

        private void _3btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Table_3 = new RestaurantMenu();
           // string folderName = TableNumber("Table_3");
           // Table_3.FolderFileName(folderName, "Table_3.txt");
            Table_3.Show();
            if (File.Exists("Table_1.txt"))
            {
                if (new FileInfo("Table_1.txt").Length == 0)
                {
                    _1btn.Background = (Brush)new BrushConverter().ConvertFrom("#FF4EE715");
                }
                else
                {
                    _1btn.Background = Brushes.Red;
                }

            }
            else
            {
                _1btn.Background = (Brush)new BrushConverter().ConvertFrom("#FF4EE715");
            }


        }

        private void _4btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Table_4 = new RestaurantMenu();
            //string folderName = TableNumber("Table_4");
            //Table_4.FolderFileName(folderName, "Table_4.txt");
            Table_4.Show();

        }

        private void _5btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Table_5 = new RestaurantMenu();
            //string folderName = TableNumber("Table_5");
            //Table_5.FolderFileName(folderName, "Table_5.txt");
            Table_5.Show();
        }

        private void _6btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Table_6 = new RestaurantMenu();
            //string folderName = TableNumber("Table_6");
            //Table_6.FolderFileName(folderName, "Table_6.txt");
            Table_6.Show();
        }

        private void _7btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Table_7 = new RestaurantMenu();
            //string folderName = TableNumber("Table_7");
            //Table_7.FolderFileName(folderName, "Table_7.txt");
            Table_7.Show();
        }

        private void b1btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Bar_1 = new RestaurantMenu();
            //string folderName = TableNumber("Bar_1");
            //Bar_1.FolderFileName(folderName, "Bar_1.txt");
            Bar_1.Show();
        }

        private void b2btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Bar_2 = new RestaurantMenu();
            //string folderName = TableNumber("Bar_2");
            //Bar_2.FolderFileName(folderName, "Bar_2.txt");
            Bar_2.Show();
        }

        private void b3btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Bar_3 = new RestaurantMenu();
            //string folderName = TableNumber("Bar_3");
            //Bar_3.FolderFileName(folderName, "Bar_3.txt");
            Bar_3.Show();
        }

        private void b4btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Bar_4 = new RestaurantMenu();
            //string folderName = TableNumber("Bar_4");
            //Bar_4.FolderFileName(folderName, "Bar_4.txt");
            Bar_4.Show();
        }

        private void b5btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Bar_5 = new RestaurantMenu();
            //string folderName = TableNumber("Bar_5");
            //Bar_5.FolderFileName(folderName, "Bar_5.txt");
            Bar_5.Show();
        }

        private void b6btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Bar_6 = new RestaurantMenu();
            //string folderName = TableNumber("Bar_6");
            //Bar_6.FolderFileName(folderName, "Bar_6.txt");
            Bar_6.Show();
        }

        private void b7btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Bar_7 = new RestaurantMenu();
            //string folderName = TableNumber("Bar_7");
            //Bar_7.FolderFileName(folderName, "Bar_7.txt");
            Bar_7.Show();
        }

        private void _8btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Bar_8 = new RestaurantMenu();
            //string folderName = TableNumber("Bar_8");
            //Bar_8.FolderFileName(folderName, "Table_2.txt");
            Bar_8.Show();
        }

        private void _9btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Table_9 = new RestaurantMenu();
            //string folderName = TableNumber("Table_9");
            //Table_9.FolderFileName(folderName, "Table_9.txt");
            Table_9.Show();
        }

        private void _10btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Table_10 = new RestaurantMenu();
            //string folderName = TableNumber("Table_10");
            //Table_10.FolderFileName(folderName, "Table_10.txt");
            Table_10.Show();
        }

        private void _11btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Table_11 = new RestaurantMenu();
            //string folderName = TableNumber("Table_11");
            //Table_11.FolderFileName(folderName, "Table_11.txt");
            Table_11.Show();
        }

        private void _12btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Table_12 = new RestaurantMenu();
            //string folderName = TableNumber("Table_12");
            //Table_12.FolderFileName(folderName, "Table_12.txt");
            Table_12.Show();
        }

        private void _13btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Table_13 = new RestaurantMenu();
            //string folderName = TableNumber("Table_13");
            //Table_13.FolderFileName(folderName, "Table_13.txt");
            Table_13.Show();
        }

        private void _14btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Table_14 = new RestaurantMenu();
            //string folderName = TableNumber("Table_14");
            //Table_14.FolderFileName(folderName, "Table_14.txt");
            Table_14.Show();
        }

        private void _15btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Table_15 = new RestaurantMenu();
            //string folderName = TableNumber("Table_15");
            //Table_15.FolderFileName(folderName, "Table_15.txt");
            Table_15.Show();
        }

        private void _16btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Table_16 = new RestaurantMenu();
            //string folderName = TableNumber("Table_16");
            //Table_16.FolderFileName(folderName, "Table_16.txt");
            Table_16.Show();
        }

        private void _17btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Table_17 = new RestaurantMenu();
            //string folderName = TableNumber("Table_17");
            //Table_17.FolderFileName(folderName, "Table_17.txt");
            Table_17.Show();
        }

        private void _18btn_Click(object sender, RoutedEventArgs e)
        {

            RestaurantMenu Table_18 = new RestaurantMenu();
            //string folderName = TableNumber("Table_18");
            //Table_18.FolderFileName(folderName, "Table_18.txt");
            Table_18.Show();
        }
        private void TableNumber(string TableNumber)
        {
            string table = TableNumber;           
            DateTime Date = DateTime.Now;
            string folderName = Date.ToString("d/M/yyyy");
            folderName = folderName.Replace('/', ('.'));
            string folderPath = Directory.GetCurrentDirectory();
            string path = System.IO.Path.Combine(folderPath, folderName);
            Directory.CreateDirectory("..//..//Bills");
            Directory.CreateDirectory("..//..//Bills//" + folderName);
            StreamWriter writer = new StreamWriter(table+".txt", true);
            writer.Close();
            fileName = table + ".txt";

            RestaurantMenu menu = new RestaurantMenu();          
            menu.FolderFileName(folderName, fileName);
            menu.Show();


        }
        private void isFileExist(string fileName, Button pName)
        {
            if (File.Exists(fileName))
            {
                if (new FileInfo(fileName).Length == 0)
                {
                    pName.Background = (Brush)new BrushConverter().ConvertFrom("#FF4EE715");
                }
                else
                {
                    pName.Background = Brushes.Red;
                }

            }
            else
            {
                pName.Background = (Brush)new BrushConverter().ConvertFrom("#FF4EE715");
            }


        }
    }
}
