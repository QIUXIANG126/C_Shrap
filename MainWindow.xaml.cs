
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        List<triangle> Triangle = new List<triangle>();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            double x, y, z;
            bool s = (double.TryParse(textbox1.Text, out x) || double.TryParse(textbox2.Text, out y) || double.TryParse(textbox3.Text, out z));
            if (s)
            {
                 x = double.Parse(textbox1.Text);
                 y = double.Parse(textbox2.Text);
                 z = double.Parse(textbox3.Text);
                if(x > y && x > z)
                {
                    if (y + z > x)
                    {
                        label1.Background=new SolidColorBrush(Colors.Green);
                        label1.Content = $"邊長{x},{y},{z} 可構成三角形";
                        Triangle.Add(new triangle(x, y, z, true));
                    }
                    else
                    {
                        label1.Background = new SolidColorBrush(Colors.Red);
                        label1.Content = $"邊長{x},{y},{z} 不可構成三角形";
                        Triangle.Add(new triangle(x, y, z, false));
                    }
                }else if (y > x && y > z)
                {
                    if (x + z > y)
                    {
                        label1.Background = new SolidColorBrush(Colors.Green);
                        label1.Content = $"邊長{x},{y},{z} 可構成三角形";
                        Triangle.Add(new triangle(x, y, z, true));
                    }
                    else
                    {
                        label1.Background = new SolidColorBrush(Colors.Red);
                        label1.Content = $"邊長{x},{y},{z} 不可構成三角形";
                        Triangle.Add(new triangle(x, y, z, false));
                    }
                }else if(z >x && z > y)
                {
                    if (y + x > z)
                    {
                        label1.Background = new SolidColorBrush(Colors.Green);
                        label1.Content = $"邊長{x},{y},{z} 可構成三角形";
                        Triangle.Add(new triangle(x, y, z, true));
                    }
                    else
                    {
                        label1.Background = new SolidColorBrush(Colors.Red);
                        label1.Content = $"邊長{x},{y},{z} 不可構成三角形";
                        Triangle.Add(new triangle(x, y, z, false));
                    }
                }
            }
            else
            {
                label1.Content = null;
                MessageBox.Show("輸入錯誤，請使用者重新輸入");
            }

            textblock_out(Triangle);
            textbox1.Text = null;
            textbox2.Text = null;
            textbox3.Text = null;
        }
        
        public class triangle
        {
            private
                double x, y, z;
                bool s;
            public
                triangle(double X, double Y, double Z, bool S)
                {
                    x = X;
                    y = Y;
                    z = Z;
                    s = S;

                }
            public string OUT(){ return $"{x},{y},{z} 能構成三角形: {s}";}
        }
        private void textblock_out(List<triangle> Triangles)
        {
            textblock1.Text = "";
            foreach(triangle triangle in Triangles)
            {
                textblock1.Text += $"{triangle.OUT()}\n";
            }
        }
    }
}
