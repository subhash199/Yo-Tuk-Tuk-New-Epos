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
    /// Interaction logic for settings.xaml
    /// </summary>
    public partial class settings : Window
    {
        ServerClass serverRequest = new ServerClass();
        public settings()
        {
            InitializeComponent();
        }

        private void DayEnd_Click(object sender, RoutedEventArgs e)
        {
            serverRequest.read("xread");
        }
    }
}
