using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace ModellingAffine
{
    public class Calculation
    {
        public List<Point> points;
        public double[,] firstPT { get; set; }
        public double[,] secondPT { get; set; }
        public double[,] reflect { get; set; }
        public double[,] firstR { get; set; }
        public double[,] secondR { get; set; }

        public Calculation(List<Point> points)
        {
            this.points = points; // strangly does deep copying of list
        }
        private List<Point> GetLine()
        {
            List<Point> result = new List<Point>
            {
                points[0],
                points[1]
            };
            return result;
        }
        private List<Point> GetSegment()
        {
            List<Point> result = new List<Point>
            {
                points[2],
                points[3]
            };
            return result;
        }
        public List<Point> AffineTransformation()
        {
            List<Point> line = GetLine();
            List<Point> segment = GetSegment();

            double[,] newCordsA = new double[,] { { line[0].X, line[0].Y, 1 } };
            double[,] newCordsB = new double[,] { { line[1].X, line[1].Y, 1 } };
            double[,] newCordsC = new double[,] { { segment[0].X, segment[0].Y, 1 } };
            double[,] newCordsD = new double[,] { { segment[1].X, segment[1].Y, 1 } };

            double[,] affineMatrix = new double[3, 3];

            affineMatrix = ParallelTranslation(-line[0].X, -line[0].Y); // parallel translation
            this.firstPT= affineMatrix;

            double[,] temp = MultiplyMatrix(newCordsB, affineMatrix);
            double angleLine = Math.Atan(temp[0, 1] / temp[0, 0]); // finding angle

            affineMatrix = MultiplyMatrix(affineMatrix, Rotation(-angleLine)); // rotation
            this.firstR = affineMatrix;

            affineMatrix = MultiplyMatrix(affineMatrix, Reflection()); // reflect segment relative to X axis
            this.reflect = affineMatrix;

            // getting everything back

            affineMatrix = MultiplyMatrix(affineMatrix, Rotation(angleLine)); // rotation
            this.secondR = affineMatrix;

            newCordsC = MultiplyMatrix(newCordsC, affineMatrix);
            newCordsD = MultiplyMatrix(newCordsD, affineMatrix);
            affineMatrix = MultiplyMatrix(affineMatrix, ParallelTranslation(line[0].X, line[0].Y)); // parallel translation
            this.secondPT = affineMatrix;

            newCordsA = MultiplyMatrix(newCordsA, affineMatrix);
            newCordsB = MultiplyMatrix(newCordsB, affineMatrix);

            {
                newCordsC[0, 0] = newCordsC[0, 0] + line[0].X;
                newCordsC[0, 1] = newCordsC[0, 1] + line[0].Y;
                newCordsD[0, 0] = newCordsD[0, 0] + line[0].X;
                newCordsD[0, 1] = newCordsD[0, 1] + line[0].Y;
            }

            List<Point> newPoints = new List<Point>() {new Point(newCordsA[0,0], newCordsA[0,1]), new Point(newCordsB[0, 0], newCordsB[0, 1]),
                new Point(newCordsC[0, 0], newCordsC[0, 1]), new Point(newCordsD[0, 0], newCordsD[0, 1]), };

            return newPoints;
        }
        public double[,] ParallelTranslation(double dx, double dy)
        {
            double[,] T = new double[3, 3] { { 1, 0, 0 }, { 0, 1, 0 }, { dx, dy, 1 } };
            return T;
        }
        public double[,] Rotation(double alpha)
        {
            double[,] R = new double[3, 3] { { Math.Cos(alpha), Math.Sin(alpha), 0 }, { -Math.Sin(alpha), Math.Cos(alpha), 0 }, { 0, 0, 1 } };
            return R;
        }
        public double[,] Scaling(double sx, double sy)
        {
            double[,] S = new double[3, 3] { { sx, 0, 0 }, { 0, sy, 0 }, { 0, 0, 1 } };
            return S;
        }
        public double[,] ShearByX(double sh)
        {
            double[,] Shx = new double[3, 3] { { 1, 0, 0 }, { sh, 1, 0 }, { 0, 0, 1 } };
            return Shx;
        }
        public double[,] ShearByY(double sh)
        {
            double[,] Shx = new double[3, 3] { { 1, sh, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
            return Shx;
        }
        public double[,] Reflection()
        {
            double[,] reflection = new double[3, 3] { { 1, 0, 0 }, { 0, -1, 1 }, { 0, 0, 1 } };
            return reflection;
        }
        public double[,] MultiplyMatrix(double[,] A, double[,] B)
        {
            int rA = A.GetLength(0);
            int cA = A.GetLength(1);
            int rB = B.GetLength(0);
            int cB = B.GetLength(1);

            double[,] result = new double[rA, cB];

            for (int i = 0; i < rA; i++)
            {
                for (int j = 0; j < cB; j++)
                {
                    for (int k = 0; k < rB; k++)
                    {
                        result[i, j] += A[i, k] * B[k, j];
                    }
                }
            }
            return result;
        }
    }
}
