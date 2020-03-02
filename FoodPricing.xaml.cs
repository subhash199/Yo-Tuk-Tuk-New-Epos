﻿using System;
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
    /// Interaction logic for FoodPricing.xaml
    /// </summary>
    public partial class FoodPricing : Window
    {
        
        public FoodPricing()
        {
            InitializeComponent();
            ServerClass server = new ServerClass();
            string[] list = (server.read("listAll")).Split(',');
            for (int i = 0; i < list.Length-1; i=i+4)
            {
                displayListView.Items.Add(new { ID = list[i], Category = list[i + 1], Dish = list[i + 2], Price = list[i + 3] });
                
            }
           
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if(displayListView.SelectedItems.Count==2)
            {
                EditWindow window = new EditWindow(displayListView.SelectedItem.ToString());
                window.Show();
                
            }
            
        }
    }
}