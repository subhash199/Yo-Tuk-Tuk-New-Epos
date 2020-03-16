using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Windows.Controls;

namespace Yo_Tuk_Tuk_Epos
{
    /// <summary>
    /// Interaction logic for RestaurantMenu.xaml
    /// </summary>
    /// 

    public partial class RestaurantMenu : Window
    {
        ServerClass server = new ServerClass();
        Dictionary<string, double> itemsDictionary = new Dictionary<string, double>();


        bool isSaved = false;
        string methodOfPay = "";
        int startersCount = 0;
        int MainCount = 0;
        int sideCount = 0;
        int desertsCount = 0;
        decimal unChangedTotal = 0;

        decimal payment = 0;
        decimal mainCurryCount = 0;
        decimal discountedValue = 0;
        int padWidth = 18;
        string fileName = "";
        bool isFinalReceipt = false;
        Socket socket = null;
        IPAddress ip = null;
        IPEndPoint ipep = null;


        public RestaurantMenu()
        {
            InitializeComponent();            
            string[] items = (server.read("itemsList")).Split(',');            
            items = items.Take(items.Count() - 1).ToArray();
            for (int i = 0; i < items.Length; i++)
            {
                itemsDictionary.Add(items[i], double.Parse(items[i + 1]));
                i = i + 1;
            }
            
            List<Button> butList = new List<Button> { _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20, _21, _22, _23, _24,
                _25, _26, _27, _28, _29, _30, _31, _32, _33, _34, _35, _36, _37, _38, _39, _40, _41, _42, _43, _44, _45, _46, _47, _48 };
            string s = "";
            for (int i = 0; i < butList.Count; i++)
            {
                butList[i].Content = items[i * 2];
            }
            

        }
        List<orderItemIdentify> holdPrint = new List<orderItemIdentify>();
        List<string> currentList = new List<string>();
        List<ItemList> sortedList = new List<ItemList>();

        string ItemName = "";
        Button button;

        decimal totalValue = 0;

        string folderName = "";
        string txtFileName = "";
        int tableNum = 0;
        #region
        //private void socketConnection()
        //{
        //    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //    socket.NoDelay = true;
        //    ip = IPAddress.Parse("192.168.1.12");
        //    ipep = new IPEndPoint(ip, 9100);
        //    socket.Connect(ipep);
        //}
        public void FolderFileName(string txtfile, int tableNumber)
        {
            txtFileName = txtfile;
            tableNum = tableNumber;
            TableFile();

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
                        startersCount = +1;
                        currentList.Add(currentList[i]);
                        break;
                    }
                }
                discountButtonCheck();
            }

        }
        private void TableFile()
        {


            try
            {

                string read = server.read(txtFileName);
                if (read.Length > 0)
                {
                    isSaved = true;
                }
                string[] splitArray = Regex.Split(read, "\r\n");
                for (int i = 0; i < splitArray.Length; i++)
                {
                    currentList.Add(splitArray[i]);

                }
                discountButtonCheck();



            }
            catch (Exception e)
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

                                sortedList.Insert(start, new ItemList(count, dish, price, "*s"));
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
                                sortedList.Insert(stIndex, new ItemList(count, dish, price, "*m"));
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
                                if (value > 5)
                                {
                                    mainCurryCount += count;
                                }
                                price = value * count;
                                totalValue += price;
                                sortedList.Insert(stIndex + mIndex, new ItemList(count, dish, price, "*sd"));
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
                                sortedList.Insert(stIndex + mIndex + sdIndex, new ItemList(count, dish, price, "*c"));
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
                                sortedList.Insert(stIndex + mIndex + sdIndex + cIndex, new ItemList(count, dish, price, "*d"));
                            }

                            break;


                    }
                    count = 0;
                    visitedDish = false;
                }

                //sortedList.Insert(stIndex, "-----------------------------------------");
                //sortedList.Insert(mIndex + stIndex + 1, "-----------------------------------------");
                //sortedList.Insert(stIndex + mIndex + sdIndex + 2, "-----------------------------------------");
                //sortedList.Insert(stIndex + mIndex + sdIndex + cIndex + 3, "-----------------------------------------");

                for (int i = 0; i < sortedList.Count; i++)
                {

                    order_Box.Items.Add(sortedList[i].qty.ToString().PadRight(4) + sortedList[i].name.PadRight(padWidth) + sortedList[i].value.ToString());

                }
                order_Box.Items.Insert(stIndex, "-----------------------------");
                order_Box.Items.Insert(mIndex + stIndex + 1, "-----------------------------");
                order_Box.Items.Insert(stIndex + mIndex + sdIndex + 2, "-----------------------------");
                order_Box.Items.Insert(stIndex + mIndex + sdIndex + cIndex + 3, "-----------------------------");
                order_Box.Items.Insert(0, "QTY".PadRight(4) + "Dish".PadRight(padWidth) + "Price");


                total_box.Text = "£ " + totalValue.ToString();
                unChangedTotal = totalValue;


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
                if (output != "")
                {
                    for (int i = 0; i < currentList.Count; i++)
                    {
                        if (currentList[i].Contains(output))
                        {
                            currentList.RemoveAt(i);
                            break;
                        }
                    }
                    for (int i = 0; i < holdPrint.Count; i++)
                    {
                        if (holdPrint[i].name.Contains(output))
                        {
                            startersCount = startersCount - 1;
                            holdPrint.RemoveAt(i);
                            break;
                        }
                    }
                    discountButtonCheck();
                }


            }

        }
        private void Save_btn_Click(object sender, RoutedEventArgs e)
        {
            

            if (holdPrint.Count != 0)
            {
               string readBack = (server.write(txtFileName, currentList));
                if(readBack == "OK")
                {
                    string sendPrint = "holdPrint," + tableNum + ",";
                    for (int i = 0; i < holdPrint.Count; i++)
                    {
                        sendPrint += holdPrint[i].section + "," + holdPrint[i].name + ",";
                    }
                    server.print(sendPrint);
                    //kitchenRecipt();
                    this.DialogResult = true;
                }
               
            }
            else if (currentList.Count != 0)
            {
                this.DialogResult = true;
            }

            this.Close();
        }


        private void Cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            if (order_Box.Items.Count > 6 && currentList.Count != 0 && isSaved == true)
            {
                this.DialogResult = true;
            }
            this.Close();
        }


        private void foodPrice(string category, string Name, double price)
        {
            if (category == "St")
            {
                currentList.Add(category + "," + Name + "," + price + ",");
                holdPrint.Insert(0, new orderItemIdentify("*s", Name));
                startersCount++;
                discountButtonCheck();
            }
            else if (category == "M")
            {
                currentList.Add(category + "," + Name + "," + price + ",");

                holdPrint.Insert(startersCount, new orderItemIdentify("*m", Name));
                MainCount += 1;
                discountButtonCheck();
            }
            else if (category == "Sd")
            {
                currentList.Add(category + "," + Name + "," + price + ",");
                holdPrint.Insert(startersCount, new orderItemIdentify("*sd", Name));
                sideCount += 1;
                discountButtonCheck();
            }
            else if (category == "C")
            {
                currentList.Add(category + "," + Name + "," + price + ",");
                holdPrint.Insert(startersCount, new orderItemIdentify("*c", Name));
                desertsCount += 1;
                discountButtonCheck();
            }
            else if (category == "D")
            {
                currentList.Add(category + "," + Name + "," + price + ",");
                holdPrint.Insert(startersCount, new orderItemIdentify("*d", Name));
                discountButtonCheck();
            }

        }
        #endregion
        private void Print_btn_Click(object sender, RoutedEventArgs e)
        {
            //receiptPrint();
            if(order_Box.Items.Count>5)
            {
                printReceipt();
            }
           

        }
        private void printReceipt()
        {
            string sendToServer = "printReceipt," + tableNum + ",";
            for (int i = 0; i < sortedList.Count; i++)
            {
                sendToServer += sortedList[i].identifier + ","+sortedList[i].qty+"," + sortedList[i].name + "," + sortedList[i].value + ",";

            }
            sendToServer += "membersDiscount," + discountedValue + ",";
            sendToServer += "grandTotal," + unChangedTotal + ",";

            if (isFinalReceipt == true)
            {
                string changeValue = totalValue.ToString();
                changeValue = changeValue.Replace('-', ' ');
                changeValue = changeValue.Trim();
                totalValue = decimal.Parse(changeValue);
                sendToServer += "paid" + (totalValue + unChangedTotal).ToString();
            }
            server.print(sendToServer);
        }
        //private void receiptPrint()
        //{
        //    var printDocument = new PrintDocument();

        //    printDocument.PrintPage += new PrintPageEventHandler(PrintReceipt);

        //    PrinterSettings printerSettings = new PrinterSettings();
        //    printDocument.PrinterSettings = printerSettings;
        //    printDocument.Print();
        //}

        //private void PrintReceipt(object sender, PrintPageEventArgs e)
        //{
        //    Graphics graphics = e.Graphics;
        //    Font font = new Font("Courier New", 12);

        //    float fontHeight = font.GetHeight();

        //    int startX = 0;
        //    int startY = 0;
        //    int offSet = 120;

        //    int mCount = 0;
        //    int sCount = 0;
        //    int cCount = 0;
        //    int dCount = 0;


        //    //e.PageSettings.PaperSize.Width = 50;

        //    System.Drawing.Image newImage = System.Drawing.Image.FromFile("YoTukTuk.png");

        //    graphics.DrawImage(newImage, 80, 0);

        //    graphics.DrawString("Yo Tuk Tuk \n 53 Lairgate \n Beverley \n HU17 8ET\n01482 881955", new Font("Calibri (Body)", 12), new SolidBrush(System.Drawing.Color.Black), 80, 0 + offSet);
        //    offSet += 100;
        //    graphics.DrawString("Table " + tableNum.ToString(), new Font("Calibri (Body)", 12), new SolidBrush(System.Drawing.Color.Black), 80, 0 + offSet);
        //    offSet += 40;
        //    for (int i = 0; i < sortedList.Count; i++)
        //    {
        //        switch (sortedList[i].identifier)
        //        {
        //            case "*m":
        //                if (mCount == 0)
        //                {
        //                    graphics.DrawString("--------------------------------------", new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
        //                    offSet += 20;
        //                    mCount += 1;
        //                }
        //                graphics.DrawString(sortedList[i].qty.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
        //                graphics.DrawString(sortedList[i].name.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 30, startY + offSet);
        //                graphics.DrawString("£ " + sortedList[i].value.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 220, startY + offSet);

        //                offSet += 20;
        //                break;
        //            case "*sd":
        //                if (sCount == 0)
        //                {
        //                    graphics.DrawString("--------------------------------------", new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
        //                    offSet += 20;
        //                    sCount += 1;
        //                }
        //                graphics.DrawString(sortedList[i].qty.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
        //                graphics.DrawString(sortedList[i].name.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 30, startY + offSet);
        //                graphics.DrawString("£ " + sortedList[i].value.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 220, startY + offSet);

        //                offSet += 20;
        //                break;
        //            case "*c":
        //                if (cCount == 0)
        //                {
        //                    graphics.DrawString("--------------------------------------", new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
        //                    offSet += 20;
        //                    cCount += 1;
        //                }
        //                graphics.DrawString(sortedList[i].qty.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
        //                graphics.DrawString(sortedList[i].name.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 30, startY + offSet);
        //                graphics.DrawString("£ " + sortedList[i].value.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 220, startY + offSet);

        //                offSet += 20;
        //                break;
        //            case "*d":
        //                if (dCount == 0)
        //                {
        //                    graphics.DrawString("--------------------------------------", new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
        //                    offSet += 20;
        //                    dCount += 1;
        //                }
        //                graphics.DrawString(sortedList[i].qty.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
        //                graphics.DrawString(sortedList[i].name.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 30, startY + offSet);
        //                graphics.DrawString("£ " + sortedList[i].value.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 220, startY + offSet);

        //                offSet += 20;
        //                break;
        //            default:

        //                graphics.DrawString(sortedList[i].qty.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
        //                graphics.DrawString(sortedList[i].name.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 30, startY + offSet);
        //                graphics.DrawString("£ " + sortedList[i].value.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 220, startY + offSet);

        //                offSet += 20;
        //                break;
        //        }



        //    }
        //    offSet += 20;
        //    graphics.DrawString("Members Discount", new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
        //    graphics.DrawString("£-" + discountedValue.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 220, startY + offSet);

        //    offSet += 20;
        //    if (isFinalReceipt == false)
        //    {
        //        graphics.DrawString("Grand Total", new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
        //        graphics.DrawString("£ " + totalValue.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 220, startY + offSet);
        //    }
        //    else
        //    {
        //        graphics.DrawString("Grand Total", new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
        //        graphics.DrawString("£ " + unChangedTotal.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 220, startY + offSet);
        //        offSet += 20;
        //        graphics.DrawString("Paid", new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
        //        string changeValue = totalValue.ToString();
        //        changeValue = changeValue.Replace('-', ' ');
        //        changeValue = changeValue.Trim();
        //        totalValue = decimal.Parse(changeValue);
        //        graphics.DrawString("£ " + (totalValue + unChangedTotal).ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 220, startY + offSet);
        //    }


        //    offSet += 40;

        //    graphics.DrawString("Thank You For Dining With Us!\n\n", new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);




        //}
        int loop = 0;
        private void MemeberDiscount_btn_Checked(object sender, RoutedEventArgs e)
        {
            discountButtonCheck();
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
            if (userInput.Text.Contains("."))
            {
                if (!(userInput.Text.Substring(userInput.Text.IndexOf(".") + 1).Length >= 2))
                {
                    userInput.Text += value.ToString();
                    payment = decimal.Parse(userInput.Text);
                }
            }
            else
            {
                userInput.Text += value.ToString();
                payment = decimal.Parse(userInput.Text);
            }


        }

        private void Btn_dot_Click(object sender, RoutedEventArgs e)
        {
            userInput.Text += ".";
            btn_dot.IsEnabled = false;
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
            if (userInput.Text.Length != 0)
            {
                userInput.Text = userInput.Text.Remove(0);
                btn_dot.IsEnabled = true;
            }

        }

        private void Pay_btn_Click(object sender, RoutedEventArgs e)
        {
            if (currentList.Count == 0 && holdPrint.Count == 0)
            {
                MessageBox.Show("No items in the order!");

            }
            else
            {
                PaymentGrid.Visibility = Visibility.Visible;
                toPay_btn.Text = totalValue.ToString();
                if (holdPrint.Count != 0)
                {
                    server.write(txtFileName, currentList);
                }

            }

        }

        private void Cash_btn_Click(object sender, RoutedEventArgs e)
        {
            methodOfPay += " Cash ";
            payMethod();

        }

        private void Card_btn_Click(object sender, RoutedEventArgs e)
        {
            methodOfPay += " Card ";
            payMethod();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (toPay_btn.Text == "0")
            {
                this.DialogResult = false;
            }
            else
            {
                this.DialogResult = true;
                this.Close();
            }

        }
        public void discountButtonCheck()
        {
            if (MemeberDiscount_btn.IsChecked == true)
            {
                if (loop != 0)
                {
                    TotalUp();
                }
                discountedValue = mainCurryCount * 2;
                totalValue -= discountedValue;
                discountBox.Text = "- " + discountedValue.ToString() + ".00 ";
                total_box.Text = totalValue.ToString();
                loop++;
            }
            else
            {

                TotalUp();
            }
        }
        private void payMethod()
        {

            void pay()
            {
                payment = decimal.Parse(userInput.Text);
                totalValue -= payment;
                toPay_btn.Text = totalValue.ToString();
                userInput.Text = userInput.Text.Remove(0);
                if (totalValue == 0 || totalValue < 0)
                {
                    printbtn.IsEnabled = true;
                    //try
                    //{
                    //    StreamWriter writer = new StreamWriter("X-read.txt",true);
                    //    writer.Write(methodOfPay + "\n" + unChangedTotal+ "\nTips\n" + totalValue+ "\n");
                    //    writer.Close();
                    //}
                    //catch
                    //{
                    //    MessageBox.Show("Couldnt store X-read details");
                    //}
                    StreamWriter Swriter = new StreamWriter("orderID.txt", true);
                    Swriter.Close();

                    string read = (File.ReadAllText("orderID.txt"));
                    if (string.IsNullOrWhiteSpace(read))
                    {
                        read = 0.ToString();
                    }
                    int orderID = int.Parse(read) + 1;

                    File.WriteAllText("orderID.txt", orderID.ToString());
                    this.payStackPanel.IsEnabled = false;

                    toPay_btn.Text = "0";
                    change_btn.Text = totalValue.ToString();
                    server.paid(txtFileName, orderID.ToString() + "," + DateTime.Now + "," + tableNum + "," + unChangedTotal + "," + discountedValue + "," + methodOfPay + "," + 0);
                }


            }
            if (userInput.Text == "")
            {

                if (MessageBox.Show("Would you like to pay the remaining amount?", "Pay ALL", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    userInput.Text = toPay_btn.Text;
                    pay();
                }
                else
                {

                }
            }
            else
            {
                if (userInput.Text.Length > 1)
                {
                    pay();
                }

            }



            btn_dot.IsEnabled = true;
        }
        private void printbtn_Click(object sender, RoutedEventArgs e)
        {
            isFinalReceipt = true;
            printReceipt();
            //receiptPrint();

        }

        private void kitchenRecipt()
        {
            var printDocument = new PrintDocument();

            printDocument.PrintPage += new PrintPageEventHandler(kitchenPrinter);

            printDocument.Print();


        }


        private void kitchenPrinter(object sender, PrintPageEventArgs e)
        {

            //socketConnection();
            List<byte> byteData = new List<byte>();

            List<string> visitedStrings = new List<string>();

            bool visited = false;
            int count = 0;

            Graphics graphics = e.Graphics;
            Font font = new Font("Arial", 12);

            float fontHeight = font.GetHeight();

            int startX = 0;
            int startY = 0;
            int offSet = 20;

            int loop = 0;
            int sdLoop = 0;
            int dloop = 0;
            int cLoop = 0;

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;


            graphics.DrawString("Table " + tableNum.ToString(), font, new SolidBrush(System.Drawing.Color.Black), 100, 0 + 0);
            offSet += 20;
            for (int i = 0; i < holdPrint.Count; i++)
            {

                switch (holdPrint[i].section)
                {
                    case "*s":

                        for (int z = 0; z < visitedStrings.Count; z++)
                        {
                            if (holdPrint[i].name == visitedStrings[z])
                            {
                                visited = true;
                                break;
                            }
                        }
                        if (visited == false)
                        {
                            for (int x = 0; x < holdPrint.Count; x++)
                            {
                                if (holdPrint[i].name == holdPrint[x].name)
                                {
                                    count++;
                                }
                            }
                            //byteData.Add(byte.Parse(count.ToString() + " x " + holdPrint[i].name+ "\n"));
                            graphics.DrawString(count.ToString() + " x " + holdPrint[i].name, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
                            offSet += 20;
                            visitedStrings.Add(holdPrint[i].name);

                        }
                        visited = false;
                        count = 0;
                        break;


                    case "*m":
                        {

                            if (loop == 0)
                            {
                                //byteData.Add(byte.Parse("----------------------------\n"));
                                graphics.DrawString("----------------------------", font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
                                offSet += 20;
                            }
                            loop++;

                            for (int z = 0; z < visitedStrings.Count; z++)
                            {
                                if (holdPrint[i].name == visitedStrings[z])
                                {
                                    visited = true;
                                    break;
                                }
                            }
                            if (visited == false)
                            {
                                for (int x = 0; x < holdPrint.Count; x++)
                                {
                                    if (holdPrint[i].name == holdPrint[x].name)
                                    {
                                        count++;
                                    }
                                }
                                //byteData.Add(byte.Parse(count.ToString() + " x " + holdPrint[i].name + "\n"));
                                graphics.DrawString(count.ToString() + " x " + holdPrint[i].name, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
                                offSet += 20;

                                visitedStrings.Add(holdPrint[i].name);

                            }
                            visited = false;
                            count = 0;

                            break;
                        }
                    case "*sd":
                        {
                            if (sdLoop == 0)
                            {
                                // byteData.Add(byte.Parse("----------------------------\n"));
                                graphics.DrawString("----------------------------", font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
                                offSet += 20;
                            }
                            sdLoop++;
                            loop = 0;
                            for (int z = 0; z < visitedStrings.Count; z++)
                            {
                                if (holdPrint[i].name == visitedStrings[z])
                                {
                                    visited = true;
                                    break;
                                }
                            }
                            if (visited == false)
                            {
                                for (int x = 0; x < holdPrint.Count; x++)
                                {
                                    if (holdPrint[i].name == holdPrint[x].name)
                                    {
                                        count++;
                                    }
                                }
                                //byteData.Add(byte.Parse(count.ToString() + " x " + holdPrint[i].name + "\n"));
                                graphics.DrawString(count.ToString() + " x " + holdPrint[i].name, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
                                offSet += 20;

                                visitedStrings.Add(holdPrint[i].name);

                            }
                            visited = false;
                            count = 0;
                            break;

                        }
                    case "*c":
                        {
                            if (cLoop == 0)
                            {
                                // byteData.Add(byte.Parse("----------------------------\n"));
                                graphics.DrawString("----------------------------", font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
                                offSet += 20;
                            }
                            cLoop++;
                            loop = 0;
                            for (int z = 0; z < visitedStrings.Count; z++)
                            {
                                if (holdPrint[i].name == visitedStrings[z])
                                {
                                    visited = true;
                                    break;
                                }
                            }
                            if (visited == false)
                            {
                                for (int x = 0; x < holdPrint.Count; x++)
                                {
                                    if (holdPrint[i].name == holdPrint[x].name)
                                    {
                                        count++;
                                    }
                                }
                                // byteData.Add(byte.Parse(count.ToString() + " x " + holdPrint[i].name + "\n"));
                                graphics.DrawString(count.ToString() + " x " + holdPrint[i].name, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
                                offSet += 20;

                                visitedStrings.Add(holdPrint[i].name);

                            }
                            visited = false;
                            count = 0;
                            break;
                        }


                    case "*d":
                        {
                            if (dloop == 0)
                            {
                                //  byteData.Add(byte.Parse("----------------------------\n"));
                                graphics.DrawString("----------------------------", font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
                                offSet += 20;
                            }
                            dloop++;
                            for (int z = 0; z < visitedStrings.Count; z++)
                            {
                                if (holdPrint[i].name == visitedStrings[z])
                                {
                                    visited = true;
                                    break;
                                }
                            }
                            if (visited == false)
                            {
                                for (int x = 0; x < holdPrint.Count; x++)
                                {
                                    if (holdPrint[i].name == holdPrint[x].name)
                                    {
                                        count++;
                                    }
                                }
                                // byteData.Add(byte.Parse(count.ToString() + " x " + holdPrint[i].name + "\n"));
                                graphics.DrawString(count.ToString() + " x " + holdPrint[i].name, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
                                offSet += 20;

                                visitedStrings.Add(holdPrint[i].name);

                            }
                            visited = false;
                            count = 0;
                            break;
                        }
                    default:
                        break;


                }




            }
            //byte[] arraybyte = byteData.ToArray();
            //socket.Send(arraybyte);
            //socket.Close();


        }


        #region
        private void _1_Click_1(object sender, RoutedEventArgs e)
        {
            Starters(sender as Button);
        }
        private void _2_Click(object sender, RoutedEventArgs e)
        {
            Starters(sender as Button);
        }

        private void _3_Click(object sender, RoutedEventArgs e)
        {
            Starters(sender as Button);
        }

        private void _4_Click(object sender, RoutedEventArgs e)
        {
            Starters(sender as Button);
        }

        private void _5_Click(object sender, RoutedEventArgs e)
        {
            Starters(sender as Button);
        }

        private void _6_Click(object sender, RoutedEventArgs e)
        {
            Starters(sender as Button);
        }

        private void _7_Click(object sender, RoutedEventArgs e)
        {
            Starters(sender as Button);
        }

        private void _8_Click(object sender, RoutedEventArgs e)
        {
            Starters(sender as Button);
        }

        private void _9_Click(object sender, RoutedEventArgs e)
        {
            Starters(sender as Button);
        }

        private void _10_Click(object sender, RoutedEventArgs e)
        {
            Starters(sender as Button);
        }
        private void _11_Click(object sender, RoutedEventArgs e)
        {
            Maincourse(sender as Button);
        }
        private void _12_Click(object sender, RoutedEventArgs e)
        {
            Maincourse(sender as Button);
        }

        private void _13_Click(object sender, RoutedEventArgs e)
        {
            Maincourse(sender as Button);
        }

        private void _14_Click(object sender, RoutedEventArgs e)
        {
            Maincourse(sender as Button);
        }
        private void _15_Click(object sender, RoutedEventArgs e)
        {
            Maincourse(sender as Button);
        }
        private void _16_Click(object sender, RoutedEventArgs e)
        {
            Maincourse(sender as Button);
        }

        private void _17_Click(object sender, RoutedEventArgs e)
        {
            Maincourse(sender as Button);
        }
        private void _18_Click(object sender, RoutedEventArgs e)
        {
            Maincourse(sender as Button);
        }

        private void _19_Click(object sender, RoutedEventArgs e)
        {
            Maincourse(sender as Button);
        }

        private void _20_Click(object sender, RoutedEventArgs e)
        {
            Maincourse(sender as Button);
        }

        private void _21_Click(object sender, RoutedEventArgs e)
        {
            Maincourse(sender as Button);
        }

        private void _22_Click(object sender, RoutedEventArgs e)
        {
            Sideoders(sender as Button);
        }
        private void _23_Click(object sender, RoutedEventArgs e)
        {
            Sideoders(sender as Button);
        }

        private void _24_Click(object sender, RoutedEventArgs e)
        {
            Sideoders(sender as Button);
        }
        private void _25_Click(object sender, RoutedEventArgs e)
        {
            Sideoders(sender as Button);
        }


        private void _26_Click(object sender, RoutedEventArgs e)
        {
            Sideoders(sender as Button);
        }

        private void _27_Click(object sender, RoutedEventArgs e)
        {
            Sideoders(sender as Button);
        }
        private void _28_Click(object sender, RoutedEventArgs e)
        {
            Sideoders(sender as Button);
        }

        private void _29_Click(object sender, RoutedEventArgs e)
        {
            Sideoders(sender as Button);
        }

        private void _30_Click(object sender, RoutedEventArgs e)
        {
            Sideoders(sender as Button);
        }

        private void _31_Click(object sender, RoutedEventArgs e)
        {
            Sideoders(sender as Button);
        }

        private void _32_Click(object sender, RoutedEventArgs e)
        {
            Carbs(sender as Button);
        }

        private void _33_Click(object sender, RoutedEventArgs e)
        {
            Carbs(sender as Button);
        }
        private void _34_Click(object sender, RoutedEventArgs e)
        {
            Carbs(sender as Button);
        }
        private void _35_Click(object sender, RoutedEventArgs e)
        {
            Carbs(sender as Button);
        }

        private void _36_Click(object sender, RoutedEventArgs e)
        {
            Carbs(sender as Button);
        }

        private void _37_Click(object sender, RoutedEventArgs e)
        {
            Carbs(sender as Button);
        }

        private void _38_Click(object sender, RoutedEventArgs e)
        {
            Carbs(sender as Button);
        }

        private void _39_Click(object sender, RoutedEventArgs e)
        {
            Deserts(sender as Button);
        }

        private void _40_Click(object sender, RoutedEventArgs e)
        {
            Deserts(sender as Button);
        }

        private void _41_Click(object sender, RoutedEventArgs e)
        {
            Deserts(sender as Button);
        }

        private void _42_Click(object sender, RoutedEventArgs e)
        {
            Deserts(sender as Button);
        }

        private void _43_Click(object sender, RoutedEventArgs e)
        {
            Deserts(sender as Button);
        }

        private void _44_Click(object sender, RoutedEventArgs e)
        {
            Maincourse(sender as Button);
        }

        private void _45_Click(object sender, RoutedEventArgs e)
        {
            Maincourse(sender as Button);
        }

        private void _46_Click(object sender, RoutedEventArgs e)
        {
            Maincourse(sender as Button);
        }

        private void _47_Click(object sender, RoutedEventArgs e)
        {
            Maincourse(sender as Button);
        }

        private void _48_Click(object sender, RoutedEventArgs e)
        {
            Maincourse(sender as Button);
        }

        private void Starters(Button bName)
        {
            button = bName as Button;
            ItemName = button.Content.ToString(); ;
            foodPrice("St", button.Content.ToString(), itemsDictionary[ItemName]);
        }
        private void Maincourse(Button bName)
        {
            button = bName as Button;
            ItemName = button.Content.ToString(); ;
            foodPrice("M", button.Content.ToString(), itemsDictionary[ItemName]);
        }
        private void Sideoders(Button bName)
        {
            button = bName as Button;
            ItemName = button.Content.ToString(); ;
            foodPrice("Sd", button.Content.ToString(), itemsDictionary[ItemName]);
        }
        private void Carbs(Button bName)
        {
            button = bName as Button;
            ItemName = button.Content.ToString(); ;
            foodPrice("C", button.Content.ToString(), itemsDictionary[ItemName]);
        }
        private void Deserts(Button bName)
        {
            button = bName as Button;
            ItemName = button.Content.ToString(); ;
            foodPrice("D", button.Content.ToString(), itemsDictionary[ItemName]);
        }


    }
}
#endregion