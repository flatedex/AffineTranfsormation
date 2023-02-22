using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;

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
            for (int i = 0; i < elements - 8; i += 9)
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

            List<TextBlock> textBlocks = new List<TextBlock>();

            FillingListOfItems(ref textBlocks, first);
            FillingListOfItems(ref textBlocks, second);
            FillingListOfItems(ref textBlocks, third);
            FillingListOfItems(ref textBlocks, fourth);
            FillingListOfItems(ref textBlocks, fifth);

            for (int i = 0; i < elements / 5; i++)
            {
                FirstGrid.Children.Add(textBlocks[i]);
                SecondGrid.Children.Add(textBlocks[i + 9]);
                ThirdGrid.Children.Add(textBlocks[i + 18]);
                FourthGrid.Children.Add(textBlocks[i + 27]);
                FifthGrid.Children.Add(textBlocks[i + 36]);
            }
        }
        public TextBlock CreateTextBlock(double[,] matrix, int i, int j)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = Math.Round(matrix[i, j], 2).ToString();
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;

            Grid.SetRow(textBlock, i);
            Grid.SetColumn(textBlock, j);

            return textBlock;
        }
        public void FillingListOfItems(ref List<TextBlock> list,double[,] matrix)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    list.Add(CreateTextBlock(matrix, i, j));
                }
            }
        }
    }

}
