using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            Regex pattern = new Regex("[{=,}]");
            details = pattern.Replace(details, "");
            string[] split = details.Split(' ');
            split = split.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            for (int i = 0; i < split.Length; i++)
            {
                if (split[i] == "ID")
                {
                    Id_label.Content = split[i + 1];
                    i++;
                }
                else if (split[i] == "Category")
                {
                    category_box.Text = split[i + 1];
                    i++;
                }
                else if (split[i] == "Dish")
                {
                    for (int j = i+1; j < split.Length; j++)
                    {                        
                        if (split[j] == "Price")
                        {
                            break;
                        }
                        dish_box.Text += split[j] + " ";
                    }
                }
                else if (split[i] == "Price")
                {
                    price_box.Text = split[i + 1];
                }


            }
            


        }

        private void Update_Button_Click(object sender, RoutedEventArgs e)
        {
            double p;
            if (double.TryParse(price_box.Text, out p))
            {
                string updateDetails = Id_label + "," + category_box.Text + "," + dish_box.Text + "," + p;
                ServerClass server = new ServerClass();
                server.create("updateDetails," + updateDetails);

            }
            else
            {
                MessageBox.Show("Price entered is Not Vaild");
            }

        }
    }
}
