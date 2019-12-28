using System;
using System.Collections.Generic;
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

namespace Yo_Tuk_Tuk_Epos
{
    /// <summary>
    /// Interaction logic for RestaurantMenu.xaml
    /// </summary>
    public partial class RestaurantMenu : Window
    {

        public RestaurantMenu()
        {
            InitializeComponent();

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {


            menuCanvas.Width = e.NewSize.Width;
            menuCanvas.Height = e.NewSize.Height;

            double xChange = 1, yChange = 1;

            if (e.PreviousSize.Width != 0)
                xChange = (e.NewSize.Width / e.PreviousSize.Width);

            if (e.PreviousSize.Height != 0)
                yChange = (e.NewSize.Height / e.PreviousSize.Height);

            foreach (FrameworkElement fe in menuCanvas.Children)
            {
                /*because I didn't want to resize the grid I'm having inside the canvas in this particular instance. (doing that from xaml) */
                if (fe is Grid == false)
                {
                    fe.Height = fe.ActualHeight * yChange;
                    fe.Width = fe.ActualWidth * xChange;

                    Canvas.SetTop(fe, Canvas.GetTop(fe) * yChange);
                    Canvas.SetLeft(fe, Canvas.GetLeft(fe) * xChange);

                }
            }

        }

        private void Bombay_bnt_Click(object sender, RoutedEventArgs e)
        {
            lowStarters(bombay_bnt.Content.ToString());
        }

        private void Order_Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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

                var lines = File.ReadAllLines("../../Table1.txt");

                StreamWriter writer = new StreamWriter("../../Table1.txt", true);

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains(output))
                    {
                        writer.WriteLine(lines[i]);
                        break;
                    }
                }



                writer.Close();
                TotalUp();
            }

        }
        private void TotalUp()
        {
            order_Box.Items.Clear();
            StreamReader reader = new StreamReader("../../Table1.txt");
            string readAll = reader.ReadToEnd();
            reader.Close();
            readAll = readAll.Replace("\r\n", "");

            string dish = "";
            decimal price = 0;
            int count = 0;

            int start = 0;
            int stIndex = 0;
            int mIndex = 0;
            int sdIndex = 0;
            int cIndex = 0;            

            decimal totalValue = 0;
            
            bool visitedDish = false;

            bool isNumber = false;
            string[] splitlines = readAll.Split(',');
            splitlines = splitlines.Take(splitlines.Count() - 1).ToArray();

            List<string> visitedStrings = new List<string>();
            List<string> sortedList = new List<string>();

            for (int i = 0; i < splitlines.Length; i++)
            {
               switch(splitlines[i])
                {
                    case "St":
                        sortedList.Insert(start,splitlines[i + 1]);
                        sortedList.Insert(start+1, splitlines[i + 2]);
                        stIndex = stIndex + 2;
                        break;
                    case "M":
                        sortedList.Insert(stIndex, splitlines[i + 1]);
                        sortedList.Insert(stIndex, splitlines[i + 2]);
                        mIndex = mIndex + 2+stIndex;                       
                        break;
                    case "Sd":
                        sortedList.Insert(sdIndex, splitlines[i + 1]);
                        sortedList.Insert(sdIndex, splitlines[i + 2]);
                        cIndex += 2;

                       


                }
            }

            price_box.Items.Clear();
            order_Box.Items.Clear();
            for (int i = 0; i < splitlines.Length; i++)
            {


                if (count != 0)
                {
                    
                    order_Box.Items.Add(count + " " + dish);
                    price_box.Items.Add("£"+count * price);
                    totalValue = totalValue + count * price;
                    total_box.Text="£"+totalValue.ToString();

                }
                count = 0;

                for (int z = 0; z < visitedStrings.Count; z++)
                {
                    if (visitedStrings[z] == splitlines[i])
                    {
                        visitedDish = true;
                    }
                }
                isNumber = splitlines[i].Any(char.IsNumber);

                if (isNumber == false && visitedDish == false)
                {
                    for (int x = i; x < splitlines.Length; x++)
                    {
                        if (splitlines[i] == splitlines[x])
                        {
                            dish = splitlines[i];
                            price = decimal.Parse(splitlines[i + 1]);
                            count++;

                        }
                    }
                }
                if (isNumber == false && visitedDish == false)
                {
                    visitedStrings.Add(splitlines[i]);
                }
                isNumber = false;
                visitedDish = false;

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

                List<string> lines = File.ReadAllLines("../../Table1.txt").ToList();

                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Contains(output))
                    {
                        lines.RemoveAt(i);
                        break;
                    }
                }
                File.WriteAllLines("../../Table1.txt", lines.ToArray());
                TotalUp();
            }

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
            Lamp(Railway_btn.Content.ToString());
        }
        private void Goan_btn_Click_1(object sender, RoutedEventArgs e)
        {
            SeaFood(Goan_btn.Content.ToString());
        }

        private void Butter_btn_Click(object sender, RoutedEventArgs e)
        {
            Chicken(Butter_btn.Content.ToString());
        }

        private void Staff_btn_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter writer = new StreamWriter("../../Table1.txt", true);
            writer.WriteLine("Staff Curry,10.95,");

            writer.Close();
            TotalUp();
        }


        private void Coconut_btn_Click(object sender, RoutedEventArgs e)
        {
            Chicken(Coconut_btn.Content.ToString());
        }

        private void Mango_btn_Click(object sender, RoutedEventArgs e)
        {
            SeaFood(Mango_btn.Content.ToString());
        }

        private void Lemon_btn_Click(object sender, RoutedEventArgs e)
        {
            SeaFood(Lemon_btn.Content.ToString());
        }

        private void Modu_btn_Click(object sender, RoutedEventArgs e)
        {
            Chicken(Modu_btn.Content.ToString());
        }

        private void Bengal_btn_Click(object sender, RoutedEventArgs e)
        {
            Chicken(Bengal_btn.Content.ToString());
        }

        private void Mixed_btn_Click(object sender, RoutedEventArgs e)
        {
            SeaFood(Mixed_btn.Content.ToString());
        }

        private void Korai_btn_Click(object sender, RoutedEventArgs e)
        {
            SeaFood(Korai_btn.Content.ToString());
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

        private void Cancel_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Pay_btn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void lowStarters(string Name)
        {
            StreamWriter writer = new StreamWriter("../../Table1.txt", true);
            writer.WriteLine("St" + Name + ",3.95,");
            writer.Close();
            TotalUp();
        }
        private void MidStarters(String Name)
        {
            StreamWriter writer = new StreamWriter("../../Table1.txt", true);
            writer.WriteLine("St" + Name + ",4.95,");
            writer.Close();
            TotalUp();
        }
        private void HighStarters(string Name)
        {
            StreamWriter writer = new StreamWriter("../../Table1.txt", true);
            writer.WriteLine("St"+Name + ",5.95,");
            writer.Close();
            TotalUp();
        }
        private void Chicken(string Name)
        {
            StreamWriter writer = new StreamWriter("../../Table1.txt", true);
            writer.WriteLine("M"+Name + ",9.95,");
            writer.Close();
            TotalUp();
        }
        private void SeaFood(String Name)
        {
            StreamWriter writer = new StreamWriter("../../Table1.txt", true);
            writer.WriteLine("M"+Name + ",12.95,");
            writer.Close();
            TotalUp();
        }
        private void Lamp(string Name)
        {
            StreamWriter writer = new StreamWriter("../../Table1.txt", true);
            writer.WriteLine("M"+Name + ",11.95,");
            writer.Close();
            TotalUp();
        }

        private void sideOrders(string Name, double Value)
        {
            StreamWriter writer = new StreamWriter("../../Table1.txt", true);
            writer.WriteLine("Sd"+Name + "," + Value + ",");
            writer.Close();
            TotalUp();
        }

        private void Carbs(string Name, double Value)
        {
            StreamWriter writer = new StreamWriter("../../Table1.txt", true);
            writer.WriteLine("C"+Name + "," + Value + ",");
            writer.Close();
            TotalUp();
        }

        private void Desserts(string Name, Double Value)
        {
            StreamWriter writer = new StreamWriter("../../Table1.txt", true);
            writer.WriteLine("D"+Name + "," + Value + ",");
            writer.Close();
            TotalUp();
        }

        private void OfficeWorker_btn_Click(object sender, RoutedEventArgs e)
        {
            Carbs("Office Workers Tiffin", 12.95);
        }

        private void SchoolBoy_btn_Click(object sender, RoutedEventArgs e)
        {
            Carbs("School Boy Tiffin", 11.95);
        }

        private void VegThali_btn_Click(object sender, RoutedEventArgs e)
        {
            Carbs("Veg Thali", 13.95);
        }

        private void NonVeg_btn_Click(object sender, RoutedEventArgs e)
        {
            Carbs("Non-Veg Thali",13.95);
        }

        private void Taster_Button_Click(object sender, RoutedEventArgs e)
        {
            Carbs("Taster Menu",16.95);
        }
    }
}
