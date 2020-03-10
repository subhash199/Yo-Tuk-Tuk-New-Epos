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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Yo_Tuk_Tuk_Epos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ServerClass server = null;
        public MainWindow()
        {

            InitializeComponent();
            server = new ServerClass();

        }
        public RestaurantLayout layout;
        private void Signup_btn_Click(object sender, RoutedEventArgs e)
        {
            SignUpForm sign = new SignUpForm(this);
            sign.Show();            
        }      
        
        
        private void Login_btn_Click(object sender, RoutedEventArgs e)
        {
            string userId = "";
            bool userExist = false;
            if (!(string.IsNullOrEmpty(Password_box.Password)))
            {
                userId = Password_box.Password;
                try
                {
                    string serverReply = (server.read("logIn," + userId));
                    if (serverReply == "exist") 
                    {
                        Password_box.Clear();
                        try
                        {
                            this.Hide();
                            layout.Show();

                        }
                        catch
                        {

                            layout = new RestaurantLayout(this);
                            this.Hide();
                            layout.Show();
                        }
                    }
                   


                    //if (File.Exists("UserDetails.txt") == false)
                    //{
                    //    StreamWriter writer = new StreamWriter("UserDetails.txt");
                    //    writer.Close();
                    //}
                    //StreamReader logIn = new StreamReader("UserDetails.txt");
                    //string read = logIn.ReadToEnd();
                    //string[] logInId = read.Split(',');

                    //logInId = logInId.Take(logInId.Count() - 1).ToArray();

                    //for (int i = 0; i < logInId.Length; i++)
                    //{

                    //    if (userId == logInId[i])
                    //    {
                    //        userExist = true;
                    //        break;
                    //    }
                    //}
                    //if (userExist == true)
                    //{

                    //    Password_box.Clear();
                    //    try
                    //    {
                    //        this.Hide();
                    //        layout.Show();

                    //    }
                    //    catch
                    //    {

                    //        layout = new RestaurantLayout(this);
                    //        this.Hide();
                    //        layout.Show();
                    //    }



                
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
            else
            {
                prompt_label.Content = "Please Enter Your LogIn ID!";

            }
            
           
         

        }

    }
}
