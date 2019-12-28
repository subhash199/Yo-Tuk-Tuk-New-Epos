using System;
using System.Collections.Generic;
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
        private void _1btn_Click(object sender, RoutedEventArgs e)
        {
            RestaurantMenu Table1 = new RestaurantMenu();
            DateTime Date = DateTime.Now;
            string folderName = Date.ToString("d/M/yyyy");
            string folderPath = System.IO.Directory.GetCurrentDirectory();
            string path = System.IO.Path.Combine(folderPath,folderName);
            Table1.Show();

        }
    }
}
