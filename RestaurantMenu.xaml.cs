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

namespace Yo_Tuk_Tuk_Epos
{
    /// <summary>
    /// Interaction logic for RestaurantMenu.xaml
    /// </summary>
    /// 
    
    public partial class RestaurantMenu : Window
    {

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
        Socket socket=null;
        IPAddress ip = null;
        IPEndPoint ipep = null;


        public RestaurantMenu()
        {
            InitializeComponent();
            TableFile();
        }
        List<orderItemIdentify> holdPrint = new List<orderItemIdentify>();
        List<string> currentList = new List<string>();
        List<ItemList> sortedList = new List<ItemList>();
        ServerClass server = new ServerClass();
        decimal totalValue = 0;

        string folderName = "";
        string txtFileName = "";
        string tableNum = "";

        //private void socketConnection()
        //{
        //    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //    socket.NoDelay = true;
        //    ip = IPAddress.Parse("192.168.1.12");
        //    ipep = new IPEndPoint(ip, 9100);
        //    socket.Connect(ipep);
        //}
        public void FolderFileName(string folder, string txtfile, string pTablenum)
        {
            folderName = folder;
            txtFileName = txtfile;
            tableNum = pTablenum;
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
                discountButtonCheck();
            }

        }
        private void TableFile()
        {


            try
            {
                if (!string.IsNullOrEmpty(txtFileName))
                {
                    string read = server.read(txtFileName);                   
                    string[] splitArray = Regex.Split(read, "\r\n");
                    //StreamReader reader = new StreamReader(txtFileName);
                    //string[] splitArray = Regex.Split(reader.ReadToEnd(), "\r\n");
                    //reader.Close();
                    for (int i = 0; i < splitArray.Length; i++)
                    {
                        currentList.Add(splitArray[i]);

                    }
                    discountButtonCheck();
                }
              

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
                if(output!="")
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
                        if(holdPrint[i].name.Contains(output))
                        {
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
            //if(holdPrint.Count!=0)
            //{
            //    StreamWriter writer = new StreamWriter(txtFileName);
            //    for (int i = 0; i < currentList.Count; i++)
            //    {
            //        writer.WriteLine(currentList[i]);
            //    }
            //    writer.Close();
            //    kitchenRecipt();
            //    this.DialogResult = true;
            //}
            if (holdPrint.Count != 0)
            {
                server.write(txtFileName, currentList);
                kitchenRecipt();
                this.DialogResult = true;
            }
            
            this.Close();
        }


        private void Cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void lowStarters(string Name)
        {
            currentList.Add("St" + "," + Name + ",3.95,");
            holdPrint.Insert(0, new orderItemIdentify("*s", Name));
            startersCount++;
            discountButtonCheck();
        }
        private void MidStarters(String Name)
        {
            currentList.Add("St" + "," + Name + ",4.95,");
            holdPrint.Insert(0, new orderItemIdentify("*s", Name));
            startersCount++;
            discountButtonCheck();
        }
        private void HighStarters(string Name)
        {
            currentList.Add("St" + "," + Name + ",5.95,");
            holdPrint.Insert(0, new orderItemIdentify("*s", Name));
            startersCount++;
            discountButtonCheck();
        }

        private void MainCourse(String Name, double value)
        {
            currentList.Add("M" + "," + Name + "," + value + ",");
            holdPrint.Insert(startersCount, new orderItemIdentify("*m", Name));
            MainCount += 1;
            discountButtonCheck();
        }


        private void sideOrders(string Name, double Value)
        {
            currentList.Add("Sd" + "," + Name + "," + Value + ",");
            holdPrint.Insert(startersCount + MainCount, new orderItemIdentify("*sd", Name));
            sideCount += 1;
            discountButtonCheck();
        }

        private void Carbs(string Name, double Value)
        {
            currentList.Add("C" + "," + Name + "," + Value + ",");
            holdPrint.Insert(startersCount + MainCount + sideCount, new orderItemIdentify("*c", Name));
            desertsCount += 1;
            discountButtonCheck();
        }

        private void Desserts(string Name, Double Value)
        {
            currentList.Add("D" + "," + Name + "," + Value + ",");
            holdPrint.Insert(startersCount + MainCount + sideCount + desertsCount, new orderItemIdentify("*d", Name));
            discountButtonCheck();
        }

        #region

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
            sideOrders("Garlic Dhall M", 9.95);
        }

        private void BengalPotato_btn_Click(object sender, RoutedEventArgs e)
        {
            sideOrders("Bengal Potato S", 3.95);

        }
        private void BengalPotatoMain_btn_Click(object sender, RoutedEventArgs e)
        {
            sideOrders("Bengal Potato M", 9.95);
        }
        private void Spinach_btn_Click(object sender, RoutedEventArgs e)
        {
            sideOrders("Coconut Spinach S", 4.95);
        }

        private void Chick_btn_Click(object sender, RoutedEventArgs e)
        {
            sideOrders("Chick Pea Mush S", 3.95);
        }

        private void Veg_btn_Click(object sender, RoutedEventArgs e)
        {
            sideOrders("Mixed Veg S", 3.95);
        }
        private void SpinachMain_btn_Copy_Click(object sender, RoutedEventArgs e)
        {
            sideOrders("Coconut Spinach M", 10.95);
        }

        private void ChickMain_btn_Click(object sender, RoutedEventArgs e)
        {
            sideOrders("Chick Pea Mush M", 9.95);
        }

        private void VegMain_btn_Click(object sender, RoutedEventArgs e)
        {
            sideOrders("Mixed Veg M", 9.95);
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
            Carbs("Perfumed Rice pint", 4.50);
        }

        private void Half_btn_Click(object sender, RoutedEventArgs e)
        {
            Carbs("halfpt Rice", 2.50);
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
            Desserts("Vanilla Ice Cream", 1.95);
        }

        private void Kulfi_btn_Click(object sender, RoutedEventArgs e)
        {
            Desserts("Kulfi", 1.95);
        }



        private void OfficeWorker_btn_Click(object sender, RoutedEventArgs e)
        {
            MainCourse("Office Workers", 12.95);
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
        #endregion
        private void Print_btn_Click(object sender, RoutedEventArgs e)
        {
            receiptPrint();

        }
        private void receiptPrint()
        {
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
            int offSet = 120;

            int mCount = 0;
            int sCount = 0;
            int cCount = 0;
            int dCount = 0;


            //e.PageSettings.PaperSize.Width = 50;

            Image newImage = Image.FromFile("YoTukTukPrint.jpg");

            graphics.DrawImage(newImage,80,0);
    
            graphics.DrawString("Yo Tuk Tuk \n 53 Lairgate \n Beverley \n HU17 8ET\n01482 881955", new Font("Calibri (Body)", 12), new SolidBrush(System.Drawing.Color.Black), 80, 0 + offSet);
            offSet += 100;
            graphics.DrawString(tableNum.ToString(), new Font("Calibri (Body)", 12), new SolidBrush(System.Drawing.Color.Black), 80, 0 + offSet);
            offSet += 40;
            for (int i = 0; i < sortedList.Count; i++)
            {
                switch (sortedList[i].identifier)
                {
                    case "*m":
                        if (mCount == 0)
                        {
                            graphics.DrawString("--------------------------------------", new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
                            offSet += 20;
                            mCount += 1;
                        }
                        graphics.DrawString(sortedList[i].qty.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
                        graphics.DrawString(sortedList[i].name.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 30, startY + offSet);
                        graphics.DrawString("£ " + sortedList[i].value.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 220, startY + offSet);

                        offSet += 20;
                        break;
                    case "*sd":
                        if (sCount == 0)
                        {
                            graphics.DrawString("--------------------------------------", new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
                            offSet += 20;
                            sCount += 1;
                        }
                        graphics.DrawString(sortedList[i].qty.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
                        graphics.DrawString(sortedList[i].name.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 30, startY + offSet);
                        graphics.DrawString("£ " + sortedList[i].value.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 220, startY + offSet);

                        offSet += 20;
                        break;
                    case "*c":
                        if (cCount == 0)
                        {
                            graphics.DrawString("--------------------------------------", new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
                            offSet += 20;
                            cCount += 1;
                        }
                        graphics.DrawString(sortedList[i].qty.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
                        graphics.DrawString(sortedList[i].name.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 30, startY + offSet);
                        graphics.DrawString("£ " + sortedList[i].value.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 220, startY + offSet);

                        offSet += 20;
                        break;
                    case "*d":
                        if (dCount == 0)
                        {
                            graphics.DrawString("--------------------------------------", new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
                            offSet += 20;
                            dCount += 1;
                        }
                        graphics.DrawString(sortedList[i].qty.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
                        graphics.DrawString(sortedList[i].name.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 30, startY + offSet);
                        graphics.DrawString("£ " + sortedList[i].value.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 220, startY + offSet);

                        offSet += 20;
                        break;
                    default:

                        graphics.DrawString(sortedList[i].qty.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
                        graphics.DrawString(sortedList[i].name.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 30, startY + offSet);
                        graphics.DrawString("£ " + sortedList[i].value.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 220, startY + offSet);

                        offSet += 20;
                        break;
                }



            }
            offSet += 20;
            graphics.DrawString("Members Discount", new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
            graphics.DrawString("£-" + discountedValue.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 220, startY + offSet);

            offSet += 20;
            if(isFinalReceipt == false)
            {
                graphics.DrawString("Grand Total", new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
                graphics.DrawString("£ " + totalValue.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 220, startY + offSet);
            }
            else
            {
                graphics.DrawString("Grand Total", new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
                graphics.DrawString("£ " + unChangedTotal.ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 220, startY + offSet);
                offSet += 20;
                graphics.DrawString("Paid", new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
                string changeValue = totalValue.ToString();
                changeValue = changeValue.Replace('-', ' ');
                changeValue = changeValue.Trim();
                totalValue = decimal.Parse(changeValue);
                graphics.DrawString("£ " + (totalValue+unChangedTotal).ToString(), new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), 220, startY + offSet);
            }
           

            offSet += 40;
            
            graphics.DrawString("Thank You For Dining With Us!\n\n", new Font("Arial", 12), new SolidBrush(System.Drawing.Color.Black), startX, startY + offSet);
           



        }
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

            userInput.Text += value.ToString();
            payment = decimal.Parse(userInput.Text);
           
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
            userInput.Text = userInput.Text.Remove(0);
            btn_dot.IsEnabled = true;
        }

        private void Pay_btn_Click(object sender, RoutedEventArgs e)
        {
            if(currentList.Count==0 && holdPrint.Count==0)
            {
                MessageBox.Show("No items in the order!");

            }
            else
            {
                PaymentGrid.Visibility = Visibility.Visible;
                toPay_btn.Text = totalValue.ToString();
                if(holdPrint.Count!=0)
                {
                    StreamWriter writer = new StreamWriter(txtFileName);
                    for (int i = 0; i < currentList.Count; i++)
                    {
                        writer.WriteLine(currentList[i]);
                    }
                    writer.Close();
                }
               
            }
         
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
            this.DialogResult=true;
            this.Close();
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
        private void payMethod(string methodOfPay)
        {

            payment = decimal.Parse(userInput.Text);
            totalValue -= payment;
            toPay_btn.Text = totalValue.ToString();
            userInput.Text = userInput.Text.Remove(0);
            if (totalValue == 0 || totalValue < 0)
            {
                printbtn.IsEnabled = true;
                try
                {
                    StreamWriter writer = new StreamWriter("X-read.txt",true);
                    writer.Write(methodOfPay + "\n" + unChangedTotal+ "\nTips\n" + totalValue+ "\n");
                    writer.Close();
                }
                catch
                {
                    MessageBox.Show("Couldnt store X-read details");
                }
                this.payStackPanel.IsEnabled = false;
                
                toPay_btn.Text = "0";
                change_btn.Text = totalValue.ToString();                
                DateTime date = DateTime.Now;
                string pDate = date.ToString("HH:mm");
                pDate = pDate.Replace(':', ' ');
                fileName = "paid " + methodOfPay + pDate + " " + txtFileName;
                File.Move(txtFileName, "Bills\\" + folderName + "\\" + fileName);

            }
            btn_dot.IsEnabled = true;
        }
        private void printbtn_Click(object sender, RoutedEventArgs e)
        {
            isFinalReceipt = true;
            receiptPrint();

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


            graphics.DrawString(tableNum, font, new SolidBrush(System.Drawing.Color.Black), 100, 0 + 0);
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

       
    }
}
