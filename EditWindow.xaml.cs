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
        Window layoutWindow;
        Window foodLayoutWindow;
        public EditWindow(MenuItems details, Window window, Window foodWindow)
        {
            layoutWindow = window;
            foodLayoutWindow = foodWindow;
            InitializeComponent();
            Id_label.Content = details.ID;
            category_box.Text = details.Category;
            dish_box.Text = details.Dish;
            price_box.Text = details.Price;        
        }

        private void Update_Button_Click(object sender, RoutedEventArgs e)
        {
            double p;
            if (double.TryParse(price_box.Text, out p))
            {
                string updateDetails = Id_label.Content + "," + category_box.Text + "," + dish_box.Text + "," + p;
                ServerClass server = new ServerClass();
                server.create("updateDetails," + updateDetails);
                this.Close();
                FoodPricing pricing = new FoodPricing(layoutWindow);
                pricing.Show();
                

            }
            else
            {
                MessageBox.Show("Price entered is Not Vaild");
            }

        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            foodLayoutWindow.Show();
        }
    }
}
