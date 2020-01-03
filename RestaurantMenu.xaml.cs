using Habanero.Faces.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
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
using Image = System.Drawing.Image;

namespace Yo_Tuk_Tuk_Epos
{
    /// <summary>
    /// Interaction logic for RestaurantMenu.xaml
    /// </summary>
    /// 
    public partial class RestaurantMenu : Window
    {
        decimal payment = 0;
        decimal mainCurryCount = 0;
        int padWidth = 18;
        decimal valueVariable = 0;
        public RestaurantMenu()
        {
            InitializeComponent();
            TableFile();
        }

        List<string> currentList = new List<string>();
        List<string> sortedList = new List<string>();

        decimal totalValue = 0;

        string folderName = "";
        string txtFileName = "";
        
        

        public void FolderFileName(string folder, string txtfile)
        {
            folderName = folder;
            txtFileName = txtfile;
            TableFile();

        }
       
        private void Bombay_bnt_Click(object sender, RoutedEventArgs e)
        {
            lowStarters(bombay_bnt.Content.ToString());
        }


        
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (order_Box.SelectedItem == null)
            {
                MessageBox.Show("Select a dish to add!");
            }
            else
            {
                string dish = order_Box.SelectedItem.ToString();
                var output = Regex.Replace(dish, @"[\d-]", string.Empty);

                output = output.Replace(".", "");

                output = output.Trim();

                for (int i = 0; i < currentList.Count; i++)
                {
                    if (currentList[i].Contains(output))
                    {
                        currentList.Add(currentList[i]);
                        break;
                    }
                }
                TotalUp();
            }

        }
        private void TableFile()
        {


            try
            {
                StreamReader reader = new StreamReader(txtFileName);
                string[] splitArray = Regex.Split(reader.ReadToEnd(), "\r\n");
                reader.Close();
                for (int i = 0; i < splitArray.Length; i++)
                {
                    currentList.Add(splitArray[i]);

                }
                TotalUp();

            }
            catch
            {

            }


        }
        private void TotalUp()
        {
            mainCurryCount = 0;
            order_Box.Items.Clear();
            sortedList.Clear();
            totalValue = 0;
            try
            {

                string readAll = string.Join(",", currentList.ToArray());

                string dish = "";
                decimal price = 0;
                int count = 0;

                int start = 0;
                int stIndex = 0;
                int mIndex = 0;
                int sdIndex = 0;
                int cIndex = 0;



                bool visitedDish = false;

                string[] splitlines = readAll.Split(',');
                splitlines = splitlines.Take(splitlines.Count() - 1).ToArray();

                decimal value = 0;


                List<string> visitedStrings = new List<string>();


                for (int i = 0; i < splitlines.Length; i++)
                {

                    switch (splitlines[i])
                    {

                        case "St":

                            for (int z = 0; z < visitedStrings.Count; z++)
                            {
                                if (splitlines[i + 1] == visitedStrings[z])
                                {
                                    visitedDish = true;
                                    break;
                                }
                            }
                            if (visitedDish == false)
                            {
                                visitedStrings.Add(splitlines[i + 1]);

                                for (int x = 0; x < splitlines.Length; x++)
                                {
                                    if (splitlines[i + 1] == splitlines[x])
                                    {
                                        dish = splitlines[i + 1];
                                        value = decimal.Parse(splitlines[i + 2]);
                                        count++;
                                    }
                                }
                                price = value * count;
                                totalValue += price;
                                sortedList.Insert(start, count.ToString().PadRight(4) + dish.PadRight(padWidth) + price);
                                stIndex = stIndex + 1;

                            }

                            break;
                        case "M":
                            for (int z = 0; z < visitedStrings.Count; z++)
                            {
                                if (splitlines[i + 1] == visitedStrings[z])
                                {
                                    visitedDish = true;
                                    break;
                                }
                            }
                            if (visitedDish == false)
                            {
                                visitedStrings.Add(splitlines[i + 1]);

                                for (int x = 0; x < splitlines.Length; x++)
                                {
                                    if (splitlines[i + 1] == splitlines[x])
                                    {
                                        dish = splitlines[i + 1];
                                        value = decimal.Parse(splitlines[i + 2]);
                                        count++;
                                    }
                                }
                                mainCurryCount += count;
                                price = value * count;
                                totalValue += price;
                                sortedList.Insert(stIndex, count.ToString().PadRight(4) + dish.PadRight(padWidth) + price.ToString());
                                visitedDish = false;
                                mIndex = mIndex + 1;
                            }

                            break;
                        case "Sd":
                            for (int z = 0; z < visitedStrings.Count; z++)
                            {
                                if (splitlines[i + 1] == visitedStrings[z])
                                {
                                    visitedDish = true;
                                    break;
                                }
                            }
                            if (visitedDish == false)
                            {
                                visitedStrings.Add(splitlines[i + 1]);

                                for (int x = 0; x < splitlines.Length; x++)
                                {
                                    if (splitlines[i + 1] == splitlines[x])
                                    {
                                        dish = splitlines[i + 1];
                                        value = decimal.Parse(splitlines[i + 2]);
                                        count++;
                                    }
                                }
                                if(price > 5)
                                {
                                    mainCurryCount += count;
                                }
                                price = value * count;
                                totalValue += price;
                                sortedList.Insert(stIndex + mIndex, count.ToString().PadRight(4) + dish.PadRight(padWidth) + price.ToString());
                                sdIndex = sdIndex + 1;
                            }

                            break;
                        case "C":
                            for (int z = 0; z < visitedStrings.Count; z++)
                            {
                                if (splitlines[i + 1] == visitedStrings[z])
                                {
                                    visitedDish = true;
                                    break;
                                }
                            }
                            if (visitedDish == false)
                            {
                                visitedStrings.Add(splitlines[i + 1]);

                                for (int x = 0; x < splitlines.Length; x++)
                                {
                                    if (splitlines[i + 1] == splitlines[x])
                                    {
                                        dish = splitlines[i + 1];
                                        value = decimal.Parse(splitlines[i + 2]);
                                        count++;
                                    }
                                }
                                price = value * count;
                                totalValue += price;
                                sortedList.Insert(stIndex + mIndex + sdIndex, count.ToString().PadRight(4) + dish.PadRight(padWidth) + price.ToString());
                                cIndex = cIndex + 1;
                            }

                            break;
                        case "D":
                            for (int z = 0; z < visitedStrings.Count; z++)
                            {
                                if (splitlines[i + 1] == visitedStrings[z])
                                {
                                    visitedDish = true;
                                    break;
                                }
                            }
                            if (visitedDish == false)
                            {
                                visitedStrings.Add(splitlines[i + 1]);

                                for (int x = 0; x < splitlines.Length; x++)
                                {
                                    if (splitlines[i + 1] == splitlines[x])
                                    {
                                        dish = splitlines[i + 1];
                                        value = decimal.Parse(splitlines[i + 2]);
                                        count++;
                                    }
                                }
                                price = value * count;
                                totalValue += price;
                                sortedList.Insert(stIndex + mIndex + sdIndex + cIndex, count.ToString().PadRight(4) + dish.PadRight(padWidth) + price.ToString());
                            }

                            break;


                    }
                    count = 0;
                    visitedDish = false;
                }

                sortedList.Insert(stIndex, "-----------------------------------------");
                sortedList.Insert(mIndex + stIndex + 1, "-----------------------------------------");
                sortedList.Insert(stIndex + mIndex + sdIndex + 2, "-----------------------------------------");
                sortedList.Insert(stIndex + mIndex + sdIndex + cIndex + 3, "-----------------------------------------");

                for (int i = 0; i < sortedList.Count; i++)
                {
                    order_Box.Items.Add(sortedList[i]);
                }
                order_Box.Items.Insert(0, "QTY".PadRight(4) + "Dish".PadRight(padWidth) + "Price");               
                total_box.Text = "£ " + totalValue.ToString();
                valueVariable = totalValue;


            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        private void Minus_btn_Click(object sender, RoutedEventArgs e)
        {
            if (order_Box.SelectedItem == null)
            {
                MessageBox.Show("Select an item to void!");
            }
            else
            {
                string dish = order_Box.SelectedItem.ToString();
                var output = Regex.Replace(dish, @"[\d-]", string.Empty);

                output = output.Replace(".", "");

                output = output.Trim();


                for (int i = 0; i < currentList.Count; i++)
                {
                    if (currentList[i].Contains(output))
                    {
                        currentList.RemoveAt(i);
                        break;
                    }

                }
                
                TotalUp();
            }

        }
        private void Save_btn_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter writer = new StreamWriter( txtFileName);
            for (int i = 0; i < currentList.Count; i++)
            {
                writer.WriteLine(currentList[i]);
            }
            writer.Close();
            this.DialogResult = true;
            this.Close();
        }


        private void Cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lowStarters(string Name)
        {
            currentList.Add("St" + "," + Name + ",3.95,");
            TotalUp();
        }
        private void MidStarters(String Name)
        {
            currentList.Add("St" + "," + Name + ",4.95,");
            TotalUp();
        }
        private void HighStarters(string Name)
        {
            currentList.Add("St" + "," + Name + ",5.95,");
            TotalUp();
        }

        private void MainCourse(String Name, double value)
        {
            currentList.Add("M" + "," + Name + "," + value + ",");
            TotalUp();
        }


        private void sideOrders(string Name, double Value)
        {
            currentList.Add("Sd" + "," + Name + "," + Value + ",");
            TotalUp();
        }

        private void Carbs(string Name, double Value)
        {
            currentList.Add("C" + "," + Name + "," + Value + ",");
            TotalUp();
        }

        private void Desserts(string Name, Double Value)
        {
            currentList.Add("D" + "," + Name + "," + Value + ",");
            TotalUp();
        }



        private void FishLeek_btn_Click(object sender, RoutedEventArgs e)
        {
            HighStarters(FishLeek_btn.Content.ToString());
        }

        private void Fish_btn_Click(object sender, RoutedEventArgs e)
        {
            MidStarters(Fish_btn.Content.ToString());
        }

        private void Keema_btn_Click(object sender, RoutedEventArgs e)
        {
            MidStarters(Keema_btn.Content.ToString());
        }
        private void Goat_btn_Click_1(object sender, RoutedEventArgs e)
        {
            lowStarters(Goat_btn.Content.ToString());
        }

        private void Bangers_btn_Click(object sender, RoutedEventArgs e)
        {
            MidStarters(bangers_btn.Content.ToString());
        }

        private void Dirty_btn_Click(object sender, RoutedEventArgs e)
        {
            MidStarters(dirty_btn.Content.ToString());
        }

        private void Sweet_btn_Click(object sender, RoutedEventArgs e)
        {
            MidStarters(sweet_btn.Content.ToString());
        }


        private void Samosas_btn_Click(object sender, RoutedEventArgs e)
        {
            lowStarters(Samosas_btn.Content.ToString());
        }

        private void Bombay_btn_Click(object sender, RoutedEventArgs e)
        {
            lowStarters(Bombay_btn.Content.ToString());
        }

        private void Railway_btn_Click(object sender, RoutedEventArgs e)
        {
            MainCourse(Railway_btn.Content.ToString(), 11.95);
        }
        private void Goan_btn_Click_1(object sender, RoutedEventArgs e)
        {
            MainCourse(Goan_btn.Content.ToString(), 12.95);
        }

        private void Butter_btn_Click(object sender, RoutedEventArgs e)
        {
            MainCourse(Butter_btn.Content.ToString(), 9.95);
        }

        private void Staff_btn_Click(object sender, RoutedEventArgs e)
        {
            MainCourse(Coconut_btn.Content.ToString(), 11.95);
        }


        private void Coconut_btn_Click(object sender, RoutedEventArgs e)
        {
            MainCourse(Coconut_btn.Content.ToString(), 9.95);
        }

        private void Mango_btn_Click(object sender, RoutedEventArgs e)
        {
            MainCourse(Mango_btn.Content.ToString(), 12.95);
        }

        private void Lemon_btn_Click(object sender, RoutedEventArgs e)
        {
            MainCourse(Lemon_btn.Content.ToString(), 12.95);
        }

        private void Modu_btn_Click(object sender, RoutedEventArgs e)
        {
            MainCourse(Modu_btn.Content.ToString(), 9.95);
        }

        private void Bengal_btn_Click(object sender, RoutedEventArgs e)
        {
            MainCourse(Bengal_btn.Content.ToString(), 9.95);
        }

        private void Mixed_btn_Click(object sender, RoutedEventArgs e)
        {
            MainCourse(Mixed_btn.Content.ToString(), 12.95);
        }

        private void Korai_btn_Click(object sender, RoutedEventArgs e)
        {
            MainCourse(Korai_btn.Content.ToString(), 12.95);
        }

        private void Garlic_btn_Click(object sender, RoutedEventArgs e)
        {
            sideOrders("Garlic Dhall S", 3.95);

        }
        private void GarlicMain_btn_Click(object sender, RoutedEventArgs e)
        {
            sideOrders("Garlic Dhall L", 9.95);
        }

        private void BengalPotato_btn_Click(object sender, RoutedEventArgs e)
        {
            sideOrders("Bengal Potato", 3.95);

        }
        private void BengalPotatoMain_btn_Click(object sender, RoutedEventArgs e)
        {
            sideOrders("Bengal Potato L", 9.95);
        }
        private void Spinach_btn_Click(object sender, RoutedEventArgs e)
        {
            sideOrders("Coconut Spinach", 4.95);
        }

        private void Chick_btn_Click(object sender, RoutedEventArgs e)
        {
            sideOrders("Chick Pea Mush", 3.95);
        }

        private void Veg_btn_Click(object sender, RoutedEventArgs e)
        {
            sideOrders("Mixed Vegetables S", 3.95);
        }
        private void SpinachMain_btn_Copy_Click(object sender, RoutedEventArgs e)
        {
            sideOrders("Coconut cream Spinach", 10.95);
        }

        private void ChickMain_btn_Click(object sender, RoutedEventArgs e)
        {
            sideOrders("Chick Pea Mush L", 9.95);
        }

        private void VegMain_btn_Click(object sender, RoutedEventArgs e)
        {
            sideOrders("Mixed Vegetable L", 9.95);
        }

        private void Aloo_btn_Click(object sender, RoutedEventArgs e)
        {
            Carbs("Aloo Puri", 2.75);
        }

        private void Noon_btn_Click(object sender, RoutedEventArgs e)
        {
            Carbs("Noon Bora", 2.75);
        }

        private void Chapatti_btn_Click(object sender, RoutedEventArgs e)
        {
            Carbs("Elephant Chapatti", 2.20);
        }

        private void Porota_btn_Click(object sender, RoutedEventArgs e)
        {
            Carbs("Porata", 2.75);
        }

        private void Bread_btn_Click(object sender, RoutedEventArgs e)
        {
            Carbs("Bread Basket", 5.95);
        }

        private void _1pt_btn_Click(object sender, RoutedEventArgs e)
        {
            Carbs("Perfumed Rice pt", 4.50);
        }

        private void Half_btn_Click(object sender, RoutedEventArgs e)
        {
            Carbs("Perfumed Rice halfpt", 2.50);
        }

        private void Gulab_btn_Click(object sender, RoutedEventArgs e)
        {
            Desserts("Warm Gulab Jamun", 3.95);
        }

        private void Macaroons_btn_Click(object sender, RoutedEventArgs e)
        {
            Desserts("Indian Macaroons", 3.95);
        }

        private void Sorbet_btn_Click(object sender, RoutedEventArgs e)
        {
            Desserts("Mixed Sorbet", 3.95);
        }

        private void Vanillia_btn_Click(object sender, RoutedEventArgs e)
        {
            Desserts("Finest Vanilla Ice Cream", 1.95);
        }

        private void Kulfi_btn_Click(object sender, RoutedEventArgs e)
        {
            Desserts("Kulfi", 1.95);
        }



        private void OfficeWorker_btn_Click(object sender, RoutedEventArgs e)
        {
            MainCourse("Office Workers Tiffin", 12.95);
        }

        private void SchoolBoy_btn_Click(object sender, RoutedEventArgs e)
        {
            MainCourse("School Boy Tiffin", 11.95);
        }

        private void VegThali_btn_Click(object sender, RoutedEventArgs e)
        {
            MainCourse("Veg Thali", 13.95);
        }

        private void NonVeg_btn_Click(object sender, RoutedEventArgs e)
        {
            MainCourse("Non Veg Thali", 13.95);
        }

        private void Taster_Button_Click(object sender, RoutedEventArgs e)
        {
            MainCourse("Taster Menu", 16.95);
        }

        private void Print_btn_Click(object sender, RoutedEventArgs e)
        {

            TotalUp();
            var printDocument = new PrintDocument();

            printDocument.PrintPage += new PrintPageEventHandler(PrintReceipt);

            PrinterSettings printerSettings = new PrinterSettings();
            //printerSettings.PrinterName = "smartprinter";
            printDocument.PrinterSettings = printerSettings;
            printDocument.Print();


        }
        

        private void PrintReceipt(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Font font = new Font("Courier New", 12);

            float fontHeight = font.GetHeight();

            int startX = 0;
            int startY = 0;
            int offSet = 20;

            //e.PageSettings.PaperSize.Width = 50;

            //Image newImage = Image.FromFile("Tuk-TUk.jpg");

            //graphics.DrawImage( newImage, 100, 100);
            graphics.DrawString("Yo TUk Tuk \n 53 Lairgate \n Beverley \n HU17 8ET", new Font("Courier New", 12), new SolidBrush(System.Drawing.Color.Black),60, 55+-35);
            offSet += 80;
            for (int i = 0; i < sortedList.Count; i++)
            {
                graphics.DrawString(sortedList[i].ToString()+"\n", new Font("Courier New", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
                offSet += 20;
            }
            graphics.DrawString("MembersDiscount".PadRight(padWidth+4)+ mainCurryCount.ToString()+".00", new Font("Courier New", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
            offSet += 20;
            graphics.DrawString("GrandTotal".PadRight(padWidth+4) + totalValue.ToString(), new Font("Courier New", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
            offSet += 20;



        }
        int loop = 0;
        private void MemeberDiscount_btn_Checked(object sender, RoutedEventArgs e)
        {
           if(MemeberDiscount_btn.IsChecked==true)
            {
                
                if (MemeberDiscount_btn.IsChecked == true)
                {
                    if(loop!=0)
                    {
                        TotalUp();
                    }                    
                    mainCurryCount = mainCurryCount * 2;
                    totalValue -= mainCurryCount;
                    discountBox.Text = "- "+ mainCurryCount.ToString()+".00 ";
                    total_box.Text = totalValue.ToString();
                    loop++;
                }    
                else
                {
                    
                    TotalUp();
                }
                
                
            }
        }
        
        private void MemeberDiscount_btn_Unchecked(object sender, RoutedEventArgs e)
        {
            discountBox.Text = discountBox.Text.Remove(0);
            TotalUp();
        }

        private void Btn_0_Click(object sender, RoutedEventArgs e)
        {
            paymentMeth(0);
        }
        private void paymentMeth(decimal value)
        {            
            userInput.Text += value.ToString();
        }

        private void Btn_dot_Click(object sender, RoutedEventArgs e)
        {
            userInput.Text += ".";
        }

        private void Btn_1_Click(object sender, RoutedEventArgs e)
        {
            paymentMeth(1);
        }

        private void Btn_2_Click(object sender, RoutedEventArgs e)
        {
            paymentMeth(2);
        }

        private void Btn_3_Click(object sender, RoutedEventArgs e)
        {
            paymentMeth(3);
        }

        private void Btn_4_Click(object sender, RoutedEventArgs e)
        {
            paymentMeth(4);
        }

        private void Btn_5_Click(object sender, RoutedEventArgs e)
        {
            paymentMeth(5);
        }

        private void Btn_6_Click(object sender, RoutedEventArgs e)
        {
            paymentMeth(6);
        }

        private void Btn_7_Click(object sender, RoutedEventArgs e)
        {
            paymentMeth(7);
        }

        private void Btn_8_Click(object sender, RoutedEventArgs e)
        {
            paymentMeth(8);
        }

        private void Btn_9_Click(object sender, RoutedEventArgs e)
        {
            paymentMeth(9);
        }

        private void Btn_CE_Click(object sender, RoutedEventArgs e)
        {
            userInput.Text = userInput.Text.Remove(0, userInput.Text.Length);
        }

        private void Pay_btn_Click(object sender, RoutedEventArgs e)
        {
            PaymentGrid.Visibility = Visibility.Visible;
            toPay_btn.Text = totalValue.ToString();
            StreamWriter writer = new StreamWriter(txtFileName);
            for (int i = 0; i < currentList.Count; i++)
            {
                writer.WriteLine(currentList[i]);
            }
            writer.Close();
        }

        private void Cash_btn_Click(object sender, RoutedEventArgs e)
        {
            payMethod("Cash");

        }

        private void Card_btn_Click(object sender, RoutedEventArgs e)
        {
            payMethod("Card");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PaidReceipt()
        {

           // TotalUp();
            
            StreamWriter writer = new StreamWriter( txtFileName);               

            writer.Write("Yo TUk Tuk \n 53 Lairgate \n Beverley \n HU17 8ET\n", new Font("Courier New", 12), new SolidBrush(System.Drawing.Color.Black));
           
            for (int i = 0; i < sortedList.Count; i++)
            {
                writer.Write(sortedList[i].ToString() + "\n", new Font("Courier New", 12), new SolidBrush(System.Drawing.Color.Black));
                
            }
            writer.Write("MembersDiscount".PadRight(padWidth + 4) + mainCurryCount.ToString() + ".00\n", new Font("Courier New", 12), new SolidBrush(System.Drawing.Color.Black));
           
            writer.Write("GrandTotal".PadRight(padWidth + 4) + valueVariable.ToString(), new Font("Courier New", 12), new SolidBrush(System.Drawing.Color.Black));
           
            writer.Close();
        }

        private void payMethod(string methodOfPay)
        {
           
            payment = decimal.Parse(userInput.Text);
            totalValue -= payment;
            toPay_btn.Text = totalValue.ToString();
            userInput.Text = userInput.Text.Remove(0);
            if (totalValue == 0 || totalValue < 0)
            {
                PaidReceipt();
                toPay_btn.Text = "0";
                change_btn.Text = totalValue.ToString();
                PaidReceipt();
                DateTime date = DateTime.Now;
                string pDate = date.ToString("HH:mm");
                pDate = pDate.Replace(':', ' ');
                File.Move(txtFileName, "Bills\\" + folderName + "\\" + "paid "+methodOfPay + pDate + " " + txtFileName);

            }
        }

    
    }
}
