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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Yo_Tuk_Tuk_Epos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //RestaurantMenu menu = new RestaurantMenu();
            //this.Close();
            //menu.Show();
            //RestaurantLayout layout = new RestaurantLayout();
            //this.Close();
            //layout.Show();
        }
        RestaurantLayout layout = null;
        private void Signup_btn_Click(object sender, RoutedEventArgs e)
        {
            SignUpForm sign = new SignUpForm();
            sign.Show();
        }      
        
        
        private void Login_btn_Click(object sender, RoutedEventArgs e)
        {
            string userId = Password_box.Password;
            bool userExist = false;
            try
            {
                if(File.Exists("UserDetails.txt")==false)
                {
                    StreamWriter writer = new StreamWriter("UserDetails.txt");
                    writer.Close();
                }
                StreamReader logIn = new StreamReader("UserDetails.txt");
                string read = logIn.ReadToEnd();
                string[] logInId = read.Split(',');

                logInId = logInId.Take(logInId.Count() - 1).ToArray();

                for (int i = 0; i < logInId.Length; i++)
                {

                    if (userId == logInId[i])
                    {
                        userExist = true;
                        break;
                    }
                }
                if (userExist == true)
                {
                    
                    Password_box.Clear();
                    try
                    {
                        
                        layout.ShowDialog();
                        if(DialogResult==true)
                        {
                            this.Show();
                        }
                    }
                    catch
                    {
                        layout= new RestaurantLayout();                       
                        layout.ShowDialog();
                        if (DialogResult == true)
                        {
                            this.Show();
                        }
                    }
                                       
                   

                }
                else
                {
                    prompt_label.Content = "Incorrect ID";
                    Password_box.Clear();
                }
            }
            catch (Exception ex)
            {
                prompt_label.Content = ex;
                Password_box.Clear();
            }

        }

    }
}
