using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        Dictionary<string, int> drinks = new Dictionary<string, int>();
        Dictionary<string, int> orders = new Dictionary<string, int>();
        string takeout = "";
        public MainWindow()
        {
            InitializeComponent();

            //新增所有飲料品項
            AddNewDrink(drinks);

            //顯示飲料品項菜單
            DisplayDrinkMenu(drinks);
        }

        private void DisplayDrinkMenu(Dictionary<string, int> myDrinks)
        {
            foreach (var drink in myDrinks)
            {
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;

                CheckBox cb = new CheckBox();
                cb.Content = $"{drink.Key} : {drink.Value}元";
                cb.Width = 200;
                cb.FontFamily = new FontFamily("Consolas");
                cb.FontSize = 18;
                cb.Foreground = Brushes.Blue;
                cb.Margin = new Thickness(5);

                Slider sl = new Slider();
                sl.Width = 100;
                sl.Value = 0;
                sl.Minimum = 0;
                sl.Maximum = 10;
                sl.VerticalAlignment = VerticalAlignment.Center;
                sl.IsSnapToTickEnabled = true;

                Label lb = new Label();
                lb.Width = 50;
                lb.Content = "0";
                lb.FontFamily = new FontFamily("Consolas");
                lb.FontSize = 18;
                lb.Foreground = Brushes.Red;

                sp.Children.Add(cb);
                sp.Children.Add(sl);
                sp.Children.Add(lb);

                Binding myBinding = new Binding("Value");
                myBinding.Source = sl;
                lb.SetBinding(ContentProperty, myBinding);

                stackpanel_DrinkMenu.Children.Add(sp);
            }
        }

        private void AddNewDrink(Dictionary<string, int> myDrinks)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "CSV檔案|*.csv|文字檔案|*.txt|所有檔案|*.*";

            if (fileDialog.ShowDialog() == true)
            {
                string[] lines = File.ReadAllLines(fileDialog.FileName);
                foreach (string line in lines)
                {
                    string[] tokens = line.Split(',');
                    string drinkName = tokens[0];
                    int price = Convert.ToInt32(tokens[1]);
                    myDrinks.Add(drinkName, price);
                }
            }
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            //將訂購的飲料加入訂單
            PlaceOrder(orders);

            //顯示訂單明細
            DisplayOrderDetail(orders);
        }

        private void DisplayOrderDetail(Dictionary<string, int> myOrders)
        {
            displayTextBlock.Inlines.Clear();
            Run titleString = new Run();
            titleString.Text = $"您所訂購的飲品：";
            titleString.Foreground = Brushes.Blue;

            Run takeoutString = new Run();
            takeoutString.Text = $"{takeout}";
            takeoutString.FontWeight = FontWeights.Bold;

            displayTextBlock.Inlines.Add(titleString);
            displayTextBlock.Inlines.Add(takeoutString);
            displayTextBlock.Inlines.Add(new Run("，訂購明細如下：\n"));
            string discoutnString = "";

            double total = 0.0;
            double sellPrice = 0.0;
            int i = 1;
            foreach (KeyValuePair<string, int> item in myOrders)
            {
                string drinkName = item.Key;
                int quantity = myOrders[drinkName];
                int price = drinks[drinkName];
                total += price * quantity;
                Run runDetail = new Run($"訂購品項{i}：{drinkName} X {quantity}杯，每杯{price}元，小計{price * quantity}元。\n");
                displayTextBlock.Inlines.Add(runDetail);
                i++;
            }


            if (total >= 500)
            {
                discoutnString = "訂購滿500元以上者打8折";
                sellPrice = total * 0.8;
            }
            else if (total >= 300)
            {
                discoutnString = "訂購滿300元以上者打85折";
                sellPrice = total * 0.85;
            }
            else if (total >= 200)
            {
                discoutnString = "訂購滿200元以上者打9折";
                sellPrice = total * 0.9;
            }
            else
            {
                discoutnString = "訂購未滿200元以上者不打折";
                sellPrice = total;
            }

            Run summaryString = new Run($"本次訂購總共{myOrders.Count}項，{discoutnString}，售價{sellPrice}元");
            if(total >= 500)
            {
                summaryString.Foreground = Brushes.Red;
            }else if (total >= 300)
            {
                summaryString.Foreground= Brushes.Green;
            }
            else
            {
                summaryString.Foreground = Brushes.Black;
            }
            summaryString.FontWeight = FontWeights.Bold;
            displayTextBlock.Inlines.Add(summaryString);
            StreamWriter writer = new StreamWriter("C:\\Users\\user\\Desktop\\4b1g0161\\WpfApp1\\WpfApp1\\order.txt");
            writer.WriteLine(displayTextBlock.Text);
            writer.Close();
        }

        private void PlaceOrder(Dictionary<string, int> myOrders)
        {
            myOrders.Clear();
            for (int i = 0; i < stackpanel_DrinkMenu.Children.Count; i++)
            {
                var sp = stackpanel_DrinkMenu.Children[i] as StackPanel;
                var cb = sp.Children[0] as CheckBox;
                var sl = sp.Children[1] as Slider;
                String drinkName = cb.Content.ToString().Substring(0, 4);
                int quantity = Convert.ToInt32(sl.Value);

                if (cb.IsChecked == true && quantity != 0)
                {
                    myOrders.Add(drinkName, quantity);
                }
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            if (rb.IsChecked == true) takeout = rb.Content.ToString();
        }
    }
}
