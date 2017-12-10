using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixTranspose {
    [DebuggerDisplay("{DebugString()}")]
    struct MatrixView {
        public int[,] Matrix;
        public int X;
        public int Y;
        public int W;
        public int H;

        public MatrixView(int[,] matrix, int x, int y, int w, int h) {
            Matrix = matrix;
            X = x;
            Y = y;
            W = w;
            H = h;
        }

        public static MatrixView FromMatrix(int[,] mat) {
            return new MatrixView(mat, 0, 0, mat.GetLength(0), mat.GetLength(1));
        }

        public ref int this[int i, int j] {
            get {
                Debug.Assert(j + X < X + W);
                Debug.Assert(i + Y < Y + H);

                return ref Matrix[j + X, i + Y];
            }
        }

        public MatrixView Sub(int x, int y, int w, int h) {
            Debug.Assert(x + w <= W);
            Debug.Assert(y + h <= H);
            return new MatrixView(Matrix, X + x, Y + y, w, h);
        }

        public string DebugString() {
            var builder = new StringBuilder();

            for (int i = 0; i < H; i++) {
                for (int j = 0; j < W; j++) {
                    builder.Append($"{Matrix[i + Y, j + X]:00} ");
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }

        public void Print() {
            Console.Write(DebugString());
        }
    }

    class Program {
        static int[,] GenerateMatrix(int size) {
            var mat = new int[size, size];

            int value = 1;
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    mat[i, j] = value++;
                }
            }

            return mat;
        }

        private static readonly int RecursionStopSize = 1;

        static void Main(string[] args) {
            Tests();

            int k = 9;

            var data = GenerateMatrix(k);
            var mat = MatrixView.FromMatrix(data);

            mat.Print();
            //NaiveTranspose(new MatrixView(mat, 0, 0, 2, 2));
            //CacheOblivousTranspose(new MatrixView(mat, 0, 0, 2, 2));
            CacheOblivousTranspose(mat);
            CacheOblivousTranspose(mat);

            int init = 1;
            for (int i = 0; i < k; i++) {
                for (int j = 0; j < k; j++) {
                    Debug.Assert(data[i, j] == init++);
                }
            }

            Console.WriteLine();
            Console.WriteLine("**********");
            Console.WriteLine();
            mat.Print();

            //CacheOblivousTranspose(mat);
        }

        private static void Tests() {
            for (int k = 54; k < 120; k++) {
                Console.WriteLine($"Testik {k}");
                int size = (int) Math.Ceiling(Math.Pow(2, k / 9f));
                var mat = GenerateMatrix(size);
                var testMat = GenerateMatrix(size);

                AssertEqualMatrix(mat, testMat);

                CacheOblivousTranspose(MatrixView.FromMatrix(mat));
                NaiveTranspose(MatrixView.FromMatrix(testMat));

                AssertEqualMatrix(mat, testMat);
            }
        }

        private static void AssertEqualMatrix(int[,] a, int[,] b) {
            Debug.Assert(a.GetLength(0) == b.GetLength(0));
            Debug.Assert(a.GetLength(1) == b.GetLength(1));

            for (int i = 0; i < a.GetLength(0); i++) {
                for (int j = 0; j < a.GetLength(1); j++) {
                    Debug.Assert(a[i,j] == b[i, j]);
                }
            }
        }

        private static void CacheOblivousTranspose(MatrixView mat) {
            if (EndRecursion(mat)) {
                NaiveTranspose(mat);
                return;
            }

            int smallW = mat.W / 2;
            int bigW = mat.W - smallW;

            int smallH = mat.H / 2;
            int bigH = mat.H - smallH;

            var topLeft = mat.Sub(0, 0, smallW, smallH);
            var bottomRight = mat.Sub(smallW, smallH, bigW, bigH);

            CacheOblivousTranspose(topLeft);
            CacheOblivousTranspose(bottomRight);

            var bottomLeft = mat.Sub(0, smallH, smallW, bigH);
            var topRight = mat.Sub(smallW, 0, bigW, smallH);

            RecursiveSwap(bottomLeft, topRight);
        }

        private static void RecursiveSwap(MatrixView bottomLeft, MatrixView topRight) {
            Debug.Assert(bottomLeft.W == topRight.H);
            Debug.Assert(bottomLeft.H == topRight.W);

            if (EndRecursion(bottomLeft)) {
                SwapCorners(bottomLeft, topRight);
                return;
            }

            int lowW = bottomLeft.W / 2;
            int highW = bottomLeft.W - lowW;

            int lowH = bottomLeft.H / 2;
            int highH = bottomLeft.H - lowH;

            RecursiveSwap(bottomLeft.Sub(0, 0, lowW, lowH), topRight.Sub(0, 0, lowH, lowW));
            RecursiveSwap(bottomLeft.Sub(0, lowH, lowW, highH), topRight.Sub(lowH, 0, highH, lowW));
            RecursiveSwap(bottomLeft.Sub(lowW, 0, highW, lowH), topRight.Sub(0, lowW, lowH, highW));
            RecursiveSwap(bottomLeft.Sub(lowW, lowH, highW, highH), topRight.Sub(lowH, lowW, highH, highW));
        }

        private static bool EndRecursion(MatrixView mat) {
            return mat.W <= RecursionStopSize || mat.H <= RecursionStopSize || (mat.H < 2 || mat.W < 1);
        }

        private static void SwapCorners(MatrixView bottomLeft, MatrixView topRight) {
            for (int i = 0; i < bottomLeft.H; i++) {
                for (int j = 0; j < bottomLeft.W; j++) {
                    Swap(ref bottomLeft[i, j], ref topRight[j, i]);
                }
            }
        }

        private static void NaiveTranspose(MatrixView mat) {
            for (int i = 0; i < mat.H; i++) {
                for (int j = 0; j < i; j++) {
                    Swap(ref mat[i, j], ref mat[j, i]);
                }
            }
        }

        private static void Swap(ref int a, ref int b) {
            int tmp = a;
            a = b;
            b = tmp;
        }        
    }
}