using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for FoodPricing.xaml
    /// </summary>
    public partial class FoodPricing : Window
    {
        CollectionView view;
        Window layoutWindow;
        public FoodPricing(Window window)
        {
            
            layoutWindow = window;
            InitializeComponent();
            ServerClass server = new ServerClass();
            string[] list = (server.read("listAll")).Split(',');
            List <MenuItems> menuList = new List<MenuItems>();
            for (int i = 0; i < list.Length-1; i=i+4)
            {               
                menuList.Add(new MenuItems() { ID = list[i], Category= list[i + 1], Dish= list[i + 2], Price= list[i + 3] });
            }
            displayListView.ItemsSource = menuList;
            view = (CollectionView)CollectionViewSource.GetDefaultView(displayListView.ItemsSource);
            FocusManager.SetFocusedElement(this, Search_box);
            Keyboard.Focus(Search_box);

        }

   

        private void Edit_btn_Click(object sender, RoutedEventArgs e)
        {

            MenuItems details = (MenuItems) displayListView.SelectedItem;
            this.Hide();
            EditWindow window = new EditWindow(details, layoutWindow, this);
            window.Show();
        }

        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            layoutWindow.Show();
        }

        private void Search_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            view.Filter = new Predicate<object>(o => ((MenuItems)o).Dish.Contains(Search_box.Text));
            
        }
    }
}
