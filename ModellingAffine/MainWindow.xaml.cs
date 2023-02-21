using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ModellingAffine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Point> points = new List<Point>();
        Point first = new Point(0, 0);
        Point second = new Point(0, 0);
        Point third = new Point(0, 0);
        Point fourth = new Point(0, 0);

        public MainWindow()
        {
            InitializeComponent();
            points.Add(first);
            points.Add(second);
            points.Add(third);
            points.Add(fourth);
        }

        private void ParseBtn_Click(object sender, RoutedEventArgs e)
        {
            Canvas.Children.Clear();

            points[0] = new Point(Convert.ToDouble(x1.Text.ToString()), Convert.ToDouble(y1.Text.ToString()));
            points[1] = new Point(Convert.ToDouble(x2.Text.ToString()), Convert.ToDouble(y2.Text.ToString()));
            points[2] = new Point(Convert.ToDouble(x3.Text.ToString()), Convert.ToDouble(y3.Text.ToString()));
            points[3] = new Point(Convert.ToDouble(x4.Text.ToString()), Convert.ToDouble(y4.Text.ToString()));

            DrawAxis();
            DrawLine(points[0], points[1], Color.FromRgb(255, 0, 0));
            DrawLine(points[2], points[3], Color.FromRgb(128, 0, 128));
        }

        private void CalculateBtn_Click(object sender, RoutedEventArgs e)
        {
            Canvas.Children.Clear();

            Calculation calculation = new Calculation(points);

            points = calculation.AffineTransformation();

            FirstGrid.Children.Clear();
            SecondGrid.Children.Clear();
            ThirdGrid.Children.Clear();
            FourthGrid.Children.Clear();
            FifthGrid.Children.Clear();

            FillMatrix(calculation.firstPT, calculation.firstR, calculation.reflect, calculation.secondR, calculation.secondPT);

            DrawAxis();
            DrawLine(points[0], points[1], Color.FromRgb(255, 0, 0));
            DrawLine(points[2], points[3], Color.FromRgb(128, 0, 128));
        }
        public void DrawAxis()
        {
            Line line = new Line();
            line.X1 = 400;
            line.Y1 = 0;
            line.X2 = 400;
            line.Y2 = 600;

            SolidColorBrush blackBrush = new SolidColorBrush();
            blackBrush.Color = Colors.Black;

            line.StrokeThickness = 2;
            line.Stroke = blackBrush;

            Canvas.Children.Add(line);

            Line line1 = new Line();

            line1.X1 = 0;
            line1.Y1 = 300;
            line1.X2 = 800;
            line1.Y2 = 300;

            line1.StrokeThickness = 2;
            line1.Stroke = blackBrush;

            Canvas.Children.Add(line1);
        }
        public void DrawLine(Point first, Point second, Color color)
        {
            Line line = new Line();
            line.X1 = first.X + 400;
            line.Y1 = -first.Y - 300 + 600;
            line.X2 = second.X + 400;
            line.Y2 = -second.Y - 300 + 600;

            SolidColorBrush redBrush = new SolidColorBrush();
            redBrush.Color = color;

            line.StrokeThickness = 4;
            line.Stroke = redBrush;

            Canvas.Children.Add(line);
        }
        public Border CreateBorder()
        {
            var thickness = new Thickness();
            thickness.Right = 1;
            thickness.Left = 1;
            thickness.Top = 1;
            thickness.Bottom = 1;
            Border border = new Border();
            border.BorderThickness = thickness;
            border.BorderBrush = Brushes.Black;

            return border;
        }
        public void FillMatrix(double[,] first, double[,] second, double[,] third, double[,] fourth, double[,] fifth)
        {
            const int elements = 45;
            List<Border> borders = new List<Border>();
            for (int i = 0; i < elements; i++)
            {
                borders.Add(CreateBorder());
            }
            for (int i = 0; i < elements - 8; i+=9)
            {
                Grid.SetRow(borders[i], 0);
                Grid.SetColumn(borders[i], 0);

                Grid.SetRow(borders[i + 1], 1);
                Grid.SetColumn(borders[i + 1], 0);

                Grid.SetRow(borders[i + 2], 2);
                Grid.SetColumn(borders[i + 2], 0);

                Grid.SetRow(borders[i + 3], 0);
                Grid.SetColumn(borders[i + 3], 1);

                Grid.SetRow(borders[i + 4], 0);
                Grid.SetColumn(borders[i + 4], 2);

                Grid.SetRow(borders[i + 5], 1);
                Grid.SetColumn(borders[i + 5], 1);

                Grid.SetRow(borders[i + 6], 1);
                Grid.SetColumn(borders[i + 6], 2);

                Grid.SetRow(borders[i + 7], 2);
                Grid.SetColumn(borders[i + 7], 2);

                Grid.SetRow(borders[i + 8], 2);
                Grid.SetColumn(borders[i + 8], 1);
            }

            for (int i = 0; i < elements / 5; i++)
            {
                FirstGrid.Children.Add(borders[i]);
                SecondGrid.Children.Add(borders[i + 9]);
                ThirdGrid.Children.Add(borders[i + 18]);
                FourthGrid.Children.Add(borders[i + 27]);
                FifthGrid.Children.Add(borders[i + 36]);
            }

            TextBlock label1 = new TextBlock();
            label1.Text = Math.Round(first[0, 0], 2).ToString();
            label1.HorizontalAlignment = HorizontalAlignment.Center;
            label1.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label2 = new TextBlock();
            label2.Text = Math.Round(first[0, 1], 2).ToString();
            label2.HorizontalAlignment = HorizontalAlignment.Center;
            label2.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label3 = new TextBlock();
            label3.Text = Math.Round(first[0, 2], 2).ToString();
            label3.HorizontalAlignment = HorizontalAlignment.Center;
            label3.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label4 = new TextBlock();
            label4.Text = Math.Round(first[1, 0], 2).ToString();
            label4.HorizontalAlignment = HorizontalAlignment.Center;
            label4.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label5 = new TextBlock();
            label5.Text = Math.Round(first[1, 1], 2).ToString();
            label5.HorizontalAlignment = HorizontalAlignment.Center;
            label5.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label6 = new TextBlock();
            label6.Text = Math.Round(first[1, 2], 2).ToString();
            label6.HorizontalAlignment = HorizontalAlignment.Center;
            label6.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label7 = new TextBlock();
            label7.Text = Math.Round(first[2, 0], 2).ToString();
            label7.HorizontalAlignment = HorizontalAlignment.Center;
            label7.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label8 = new TextBlock();
            label8.Text = Math.Round(first[2, 1], 2).ToString();
            label8.HorizontalAlignment = HorizontalAlignment.Center;
            label8.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label9 = new TextBlock();
            label9.Text = Math.Round(first[2, 2], 2).ToString();
            label9.HorizontalAlignment = HorizontalAlignment.Center;
            label9.VerticalAlignment = VerticalAlignment.Center; // end of first matrix
            TextBlock label10 = new TextBlock();
            label10.Text = Math.Round(second[0, 0], 2).ToString();
            label10.HorizontalAlignment = HorizontalAlignment.Center;
            label10.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label11 = new TextBlock();
            label11.Text = Math.Round(second[0, 1], 2).ToString();
            label11.HorizontalAlignment = HorizontalAlignment.Center;
            label11.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label12 = new TextBlock();
            label12.Text = Math.Round(second[0, 2], 2).ToString();
            label12.HorizontalAlignment = HorizontalAlignment.Center;
            label12.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label13 = new TextBlock();
            label13.Text = Math.Round(second[1, 0], 2).ToString();
            label13.HorizontalAlignment = HorizontalAlignment.Center;
            label13.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label14 = new TextBlock();
            label14.Text = Math.Round(second[1, 1], 2).ToString();
            label14.HorizontalAlignment = HorizontalAlignment.Center;
            label14.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label15 = new TextBlock();
            label15.Text = Math.Round(second[1, 2], 2).ToString();
            label15.HorizontalAlignment = HorizontalAlignment.Center;
            label15.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label16 = new TextBlock();
            label16.Text = Math.Round(second[2, 0], 2).ToString();
            label16.HorizontalAlignment = HorizontalAlignment.Center;
            label16.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label17 = new TextBlock();
            label17.Text = Math.Round(second[2, 1], 2).ToString();
            label17.HorizontalAlignment = HorizontalAlignment.Center;
            label17.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label18 = new TextBlock();
            label18.Text = Math.Round(second[2, 2], 2).ToString();
            label18.HorizontalAlignment = HorizontalAlignment.Center;
            label18.VerticalAlignment = VerticalAlignment.Center;// end of second matrix
            TextBlock label19 = new TextBlock();
            label19.Text = Math.Round(third[0, 0], 2).ToString();
            label19.HorizontalAlignment = HorizontalAlignment.Center;
            label19.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label20 = new TextBlock();
            label20.Text = Math.Round(third[0, 1], 2).ToString();
            label20.HorizontalAlignment = HorizontalAlignment.Center;
            label20.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label21 = new TextBlock();
            label21.Text = Math.Round(third[0, 2], 2).ToString();
            label21.HorizontalAlignment = HorizontalAlignment.Center;
            label21.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label22 = new TextBlock();
            label22.Text = Math.Round(third[1, 0], 2).ToString();
            label22.HorizontalAlignment = HorizontalAlignment.Center;
            label22.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label23 = new TextBlock();
            label23.Text = Math.Round(third[1, 1], 2).ToString();
            label23.HorizontalAlignment = HorizontalAlignment.Center;
            label23.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label24 = new TextBlock();
            label24.Text = Math.Round(third[1, 2], 2).ToString();
            label24.HorizontalAlignment = HorizontalAlignment.Center;
            label24.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label25 = new TextBlock();
            label25.Text = Math.Round(third[2, 0], 2).ToString();
            label25.HorizontalAlignment = HorizontalAlignment.Center;
            label25.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label26 = new TextBlock();
            label26.Text = Math.Round(third[2, 1], 2).ToString();
            label26.HorizontalAlignment = HorizontalAlignment.Center;
            label26.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label27 = new TextBlock();
            label27.Text = Math.Round(third[2, 2], 2).ToString();
            label27.HorizontalAlignment = HorizontalAlignment.Center;
            label27.VerticalAlignment = VerticalAlignment.Center;// end of third matrix
            TextBlock label28 = new TextBlock();
            label28.Text = Math.Round(fourth[0, 0], 2).ToString();
            label28.HorizontalAlignment = HorizontalAlignment.Center;
            label28.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label29 = new TextBlock();
            label29.Text = Math.Round(fourth[0, 1], 2).ToString();
            label29.HorizontalAlignment = HorizontalAlignment.Center;
            label29.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label30 = new TextBlock();
            label30.Text = Math.Round(fourth[0, 2], 2).ToString();
            label30.HorizontalAlignment = HorizontalAlignment.Center;
            label30.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label31 = new TextBlock();
            label31.Text = Math.Round(fourth[1, 0], 2).ToString();
            label31.HorizontalAlignment = HorizontalAlignment.Center;
            label31.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label32 = new TextBlock();
            label32.Text = Math.Round(fourth[1, 1], 2).ToString();
            label32.HorizontalAlignment = HorizontalAlignment.Center;
            label32.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label33 = new TextBlock();
            label33.Text = Math.Round(fourth[1, 2], 2).ToString();
            label33.HorizontalAlignment = HorizontalAlignment.Center;
            label33.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label34 = new TextBlock();
            label34.Text = Math.Round(fourth[2, 0], 2).ToString();
            label34.HorizontalAlignment = HorizontalAlignment.Center;
            label34.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label35 = new TextBlock();
            label35.Text = Math.Round(fourth[2, 1], 2).ToString();
            label35.HorizontalAlignment = HorizontalAlignment.Center;
            label35.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label36 = new TextBlock();
            label36.Text = Math.Round(fourth[2, 2], 2).ToString();
            label36.HorizontalAlignment = HorizontalAlignment.Center;
            label36.VerticalAlignment = VerticalAlignment.Center;// end of fourth matrix
            TextBlock label37 = new TextBlock();
            label37.Text = Math.Round(fifth[0, 0], 2).ToString();
            label37.HorizontalAlignment = HorizontalAlignment.Center;
            label37.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label38 = new TextBlock();
            label38.Text = Math.Round(fifth[0, 1], 2).ToString();
            label38.HorizontalAlignment = HorizontalAlignment.Center;
            label38.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label39 = new TextBlock();
            label39.Text = Math.Round(fifth[0, 2], 2).ToString();
            label39.HorizontalAlignment = HorizontalAlignment.Center;
            label39.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label40 = new TextBlock();
            label40.Text = Math.Round(fifth[1, 0], 2).ToString();
            label40.HorizontalAlignment = HorizontalAlignment.Center;
            label40.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label41 = new TextBlock();
            label41.Text = Math.Round(fifth[1, 1], 2).ToString();
            label41.HorizontalAlignment = HorizontalAlignment.Center;
            label41.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label42 = new TextBlock();
            label42.Text = Math.Round(fifth[1, 2], 2).ToString();
            label42.HorizontalAlignment = HorizontalAlignment.Center;
            label42.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label43 = new TextBlock();
            label43.Text = Math.Round(fifth[2, 0], 2).ToString();
            label43.HorizontalAlignment = HorizontalAlignment.Center;
            label43.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label44 = new TextBlock();
            label44.Text = Math.Round(fifth[2, 1], 2).ToString();
            label44.HorizontalAlignment = HorizontalAlignment.Center;
            label44.VerticalAlignment = VerticalAlignment.Center;
            TextBlock label45 = new TextBlock();
            label45.Text = Math.Round(fifth[2, 2], 2).ToString();
            label45.HorizontalAlignment = HorizontalAlignment.Center;
            label45.VerticalAlignment = VerticalAlignment.Center; ;// end of fifth matrix

            Grid.SetRow(label1, 0);
            Grid.SetColumn(label1, 0);
            Grid.SetRow(label2, 0);
            Grid.SetColumn(label2, 1);
            Grid.SetRow(label3, 0);
            Grid.SetColumn(label3, 2);
            Grid.SetRow(label4, 1);
            Grid.SetColumn(label4, 0);
            Grid.SetRow(label5, 1);
            Grid.SetColumn(label5, 1);
            Grid.SetRow(label6, 1);
            Grid.SetColumn(label6, 2);
            Grid.SetRow(label7, 2);
            Grid.SetColumn(label7, 0);
            Grid.SetRow(label8, 2);
            Grid.SetColumn(label8, 1);
            Grid.SetRow(label9, 2);
            Grid.SetColumn(label9, 2);

            Grid.SetRow(label10, 0);
            Grid.SetColumn(label10, 0);
            Grid.SetRow(label11, 0);
            Grid.SetColumn(label11, 1);
            Grid.SetRow(label12, 0);
            Grid.SetColumn(label12, 2);
            Grid.SetRow(label13, 1);
            Grid.SetColumn(label13, 0);
            Grid.SetRow(label14, 1);
            Grid.SetColumn(label14, 1);
            Grid.SetRow(label15, 1);
            Grid.SetColumn(label15, 2);
            Grid.SetRow(label16, 2);
            Grid.SetColumn(label16, 0);
            Grid.SetRow(label17, 2);
            Grid.SetColumn(label17, 1);
            Grid.SetRow(label18, 2);
            Grid.SetColumn(label18, 2);

            Grid.SetRow(label19, 0);
            Grid.SetColumn(label19, 0);
            Grid.SetRow(label20, 0);
            Grid.SetColumn(label20, 1);
            Grid.SetRow(label21, 0);
            Grid.SetColumn(label21, 2);
            Grid.SetRow(label22, 1);
            Grid.SetColumn(label22, 0);
            Grid.SetRow(label23, 1);
            Grid.SetColumn(label23, 1);
            Grid.SetRow(label24, 1);
            Grid.SetColumn(label24, 2);
            Grid.SetRow(label25, 2);
            Grid.SetColumn(label25, 0);
            Grid.SetRow(label26, 2);
            Grid.SetColumn(label26, 1);
            Grid.SetRow(label27, 2);
            Grid.SetColumn(label27, 2);

            Grid.SetRow(label28, 0);
            Grid.SetColumn(label28, 0);
            Grid.SetRow(label29, 0);
            Grid.SetColumn(label29, 1);
            Grid.SetRow(label30, 0);
            Grid.SetColumn(label30, 2);
            Grid.SetRow(label31, 1);
            Grid.SetColumn(label31, 0);
            Grid.SetRow(label32, 1);
            Grid.SetColumn(label32, 1);
            Grid.SetRow(label33, 1);
            Grid.SetColumn(label33, 2);
            Grid.SetRow(label34, 2);
            Grid.SetColumn(label34, 0);
            Grid.SetRow(label35, 2);
            Grid.SetColumn(label35, 1);
            Grid.SetRow(label36, 2);
            Grid.SetColumn(label36, 2);

            Grid.SetRow(label37, 0);
            Grid.SetColumn(label37, 0);
            Grid.SetRow(label38, 0);
            Grid.SetColumn(label38, 1);
            Grid.SetRow(label39, 0);
            Grid.SetColumn(label39, 2);
            Grid.SetRow(label40, 1);
            Grid.SetColumn(label40, 0);
            Grid.SetRow(label41, 1);
            Grid.SetColumn(label41, 1);
            Grid.SetRow(label42, 1);
            Grid.SetColumn(label42, 2);
            Grid.SetRow(label43, 2);
            Grid.SetColumn(label43, 0);
            Grid.SetRow(label44, 2);
            Grid.SetColumn(label44, 1);
            Grid.SetRow(label45, 2);
            Grid.SetColumn(label45, 2);

            FirstGrid.Children.Add(label1);
            FirstGrid.Children.Add(label2);
            FirstGrid.Children.Add(label3);
            FirstGrid.Children.Add(label4);
            FirstGrid.Children.Add(label5);
            FirstGrid.Children.Add(label6);
            FirstGrid.Children.Add(label7);
            FirstGrid.Children.Add(label8);
            FirstGrid.Children.Add(label9);

            SecondGrid.Children.Add(label10);
            SecondGrid.Children.Add(label11);
            SecondGrid.Children.Add(label12);
            SecondGrid.Children.Add(label13);
            SecondGrid.Children.Add(label14);
            SecondGrid.Children.Add(label15);
            SecondGrid.Children.Add(label16);
            SecondGrid.Children.Add(label17);
            SecondGrid.Children.Add(label18);

            ThirdGrid.Children.Add(label19);
            ThirdGrid.Children.Add(label20);
            ThirdGrid.Children.Add(label21);
            ThirdGrid.Children.Add(label22);
            ThirdGrid.Children.Add(label23);
            ThirdGrid.Children.Add(label24);
            ThirdGrid.Children.Add(label25);
            ThirdGrid.Children.Add(label26);
            ThirdGrid.Children.Add(label27);

            FourthGrid.Children.Add(label28);
            FourthGrid.Children.Add(label29);
            FourthGrid.Children.Add(label30);
            FourthGrid.Children.Add(label31);
            FourthGrid.Children.Add(label32);
            FourthGrid.Children.Add(label33);
            FourthGrid.Children.Add(label34);
            FourthGrid.Children.Add(label35);
            FourthGrid.Children.Add(label36);

            FifthGrid.Children.Add(label37);
            FifthGrid.Children.Add(label38);
            FifthGrid.Children.Add(label39);
            FifthGrid.Children.Add(label40);
            FifthGrid.Children.Add(label41);
            FifthGrid.Children.Add(label42);
            FifthGrid.Children.Add(label43);
            FifthGrid.Children.Add(label44);
            FifthGrid.Children.Add(label45);
        }
    }
}
