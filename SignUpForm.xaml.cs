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
using System.Windows.Shapes;

namespace Yo_Tuk_Tuk_Epos
{
    /// <summary>
    /// Interaction logic for SignUpForm.xaml
    /// </summary>
    public partial class SignUpForm : Window
    {
        public SignUpForm()
        {
            InitializeComponent();
        }

        private void Signup_btn_Click(object sender, RoutedEventArgs e)
        {
            bool idExist = false;
            string accessCode = "Yo!079me";
            string name = "";
            string id="";

            
            if (accessBox.Password == accessCode&& name_box.Text.Trim()!="" &&logInBox.Text.Trim()!="")
            {

                if (name_box.Text.All(char.IsDigit) == true )
                {
                    errorPrompt_label.Content = "Invaild Name";
                }
                else
                {
                    name = name_box.Text;
                    id = logInBox.Text;
                }
                StreamReader reader = new StreamReader("../../UserDetails.txt");
                string copyRead = reader.ReadToEnd();
                //string[] read = File.ReadAllLines("../../UserDetails.txt");

                //string copyRead = "";
                //for (int i = 0; i < read.Length; i++)
                //{
                //    copyRead += read[i];
                //}
                reader.Close();
                string[] splitDetails = copyRead.Split(',');                
                for (int i = 0; i < splitDetails.Length; i++)
                {
                    if(splitDetails[i]==id)
                    {
                        idExist = true;
                        errorPrompt_label.Content = "User Id already exists!";
                    }
                }
              
                if(idExist==false)
                {
                    StreamWriter write = new StreamWriter("../../UserDetails.txt",true);
                    write.Write(id + "," + name+",");
                    write.Close();
                    this.Close();
                }           
                
            }
            else
            {
                errorPrompt_label.Content = "Invalid Details";
            }

        }

    }
}
