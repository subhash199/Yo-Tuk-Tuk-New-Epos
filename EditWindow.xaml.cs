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
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public EditWindow(string details)
        {
            InitializeComponent();

        }

        private void Update_Button_Click(object sender, RoutedEventArgs e)
        {
            double p;
            if(double.TryParse(price_box.Text,out p))
            {
                string updateDetails = Id_label + "," + category_box.Text + "," + dish_box.Text + "," + p;

            }
            else
            {
                MessageBox.Show("Price entered is Not Vaild");
            }
            
        }
    }
}
