using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixTranspose {
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

                return ref Matrix[j + Y, i + Y];
            }
        }

        public MatrixView Sub(int x, int y, int w, int h) {
            Debug.Assert(x + w <= W);
            Debug.Assert(y + h <= H);
            return new MatrixView(Matrix, X + x, Y + y, w, h);
        }

        public void Print() {
            for (int i = 0; i < H; i++) {
                for (int j = 0; j < W; j++) {
                    Console.Write($"{Matrix[i + Y, j + X]:00} ");
                }
                Console.WriteLine();
            }
        }
    }

    class Program {
        private static readonly int RecursionStopSize = 8;

        static void Main(string[] args) {
            int k = 4;
            var mat = new int[k, k];

            int init = 1;
            for (int i = 0; i < k; i++) {
                for (int j = 0; j < k; j++) {
                    mat[i, j] = init++;
                }
            }


            var view = MatrixView.FromMatrix(mat);


            view.Print();
            NaiveTranspose(new MatrixView(mat, 0, 0, 2, 2));

            Console.WriteLine();
            Console.WriteLine("**********");
            Console.WriteLine();
            view.Print();

            //CacheOblivousTranspose(mat);
        }

        private static void PrintMatrix(int[,] mat) {
            for (int i = 0; i < mat.GetLength(0); i++) {
                for (int j = 0; j < mat.GetLength(1); j++) {
                    Console.Write($"{mat[i, j]} ");
                }
                Console.WriteLine();
            }
        }

        private static void CacheOblivousTranspose(MatrixView mat) {
            int leftW = mat.W / 2;
            int rightW = mat.W - leftW;

            int leftH = mat.H / 2;


            var topLeft = new MatrixView(mat.Matrix, 0, 0, leftW, leftH);
            var bottomRight = new MatrixView(mat.Matrix, 0, 0, leftW, leftH);

            CacheOblivousTranspose(topLeft);
            CacheOblivousTranspose(bottomRight);

            var bottomLeft = new MatrixView(mat.Matrix, 0, 0, leftW, leftH);
            var topRight = new MatrixView(mat.Matrix, 0, 0, leftW, leftH);

            SwapCorners(bottomLeft, topRight);
        }

        private static void RecursiveSwap(MatrixView bottomLeft, MatrixView topRight) {
            Debug.Assert(bottomLeft.W == topRight.H);
            Debug.Assert(bottomLeft.H == topRight.W);

            if (bottomLeft.W < RecursionStopSize || bottomLeft.H < RecursionStopSize) {
                SwapCorners(bottomLeft, topRight);
                return;
            }

            int lowW = bottomLeft.W / 2;
            int highW = bottomLeft.W - lowW;

            int lowH = bottomLeft.H / 2;
            int highH = bottomLeft.H - lowH;

            RecursiveSwap(new MatrixView(bottomLeft.Matrix, ), );

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