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
using System.Windows.Shapes;

namespace Yo_Tuk_Tuk_Epos
{
    /// <summary>
    /// Interaction logic for SignUpForm.xaml
    /// </summary>
    public partial class SignUpForm : Window
    {
        ServerClass server = null;
        public SignUpForm(Window main)
        {
            InitializeComponent();
            server = new ServerClass();
        }

        private void Signup_btn_Click(object sender, RoutedEventArgs e)
        {
           
            string accessCode = "Yo!TukTuk";
            string name = "";
            string id="";

            
            if (accessBox.Password == accessCode&& name_box.Text.Trim()!="" &&logInBox.Text.Trim()!="")
            {

                if (name_box.Text.All(char.IsDigit) == true )
                {
                    errorPrompt_label.Content = "Invaild Name";
                }
                else if(logInBox.Text.All(char.IsLetter)==true)
                {
                    errorPrompt_label.Content = "Invaild ID! Please Enter Numbers";
                }
                else
                {
                    name = name_box.Text;
                    id = logInBox.Text;
                  
                    string serverRespond = server.read("signUp," + name + "," + id);
                    if(serverRespond == "OK")
                    {
                        this.Close();
                    }
                    else
                    {
                        errorPrompt_label.Content = "User ID already exists!";
                    }
                }
               
                
            }
            else
            {
                errorPrompt_label.Content = "Invalid Details";
            }

        }

    }
}
