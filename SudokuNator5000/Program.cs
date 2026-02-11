using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace SudokuNator5000
{
    public class Program
    {
        static void Main(string[] args)
        {
<<<<<<< HEAD
            /*int[,] mat = StrToMat("500090106103005092060020030001000009050000300006007001000870000002000600900200005");
            Board brd = new Board(9);
            brd.LoadBoard(mat);
            brd.Solve();
            brd.PrintBoard();*/
            SudokuPlay();
        }

        public static void SudokuPlay()
        {
            while (true) {
                try
                {
                    var sw = Stopwatch.StartNew();
                    Board brd;
                    Console.Write("Please Enter a sudoku board:> ");
                    int[,] input = GetUserInput(16); // for 9x9 board. to disable the limit change the 9 to 0;
                    brd = new Board(input.GetLength(0));
                    brd.LoadBoard(input);
                    Console.WriteLine("Board Entered:");
                    brd.PrintBoard();

                    if (brd.Solve())
                    {
                        Console.WriteLine("Solved Board:");
                        brd.PrintBoard();
                    }
                    else
                        Console.WriteLine("board unsolvable");
                    sw.Stop();
                    Console.WriteLine($"Time Elapsed: {sw.Elapsed}");
                }
                catch (Exception e)
                {
                    Console.Write($"{e.Message} \n try again:\n");
                }
            }
        }

        public static int[,] GetUserInput(int size)
        {
            //gets expected input size and input from user and returns a matrix representing the input
            //if no input size is expected pass 0;

            string input;
            input = Console.ReadLine();
            if (size != 0 && input.Length != Math.Pow(size, 2))
                throw new InvalidCharacterException($"Expected {Math.Pow(size, 2)} long input, got {input.Length} length instead.");
            return StrToMat(input);
        }
        public static int[,] StrToMat(string input)
        {
            //gets string representing the input mat
            //returns an int matrix representing the input

            int[,] ret;
            int length = input.Length;
            int size = (int)Math.Sqrt(length);
            if (length % size != 0)
                throw new InvalidBoardSizesException("Board dimentions are not square");
            ret = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    ret[i, j] = CharToInt(input[i * size + j]);
                }
            }
            return ret;
        }

        public static int CharToInt(char c)
        {
            //translates ascii values of the input to numeric values

            if (c >= '0' && c <= '9')
                return (int)(c - '0');
            if (c >= 'A' && c <= 'Z')
                return (int)(c - 'A') + 10;
            if (c >= 'a' && c <= 'z')
                return (int)(c - 'a') + 10;
            else throw new InvalidCharacterException("Value must be a digit or a letter.");
=======
            SudokuPlayer.Play();
>>>>>>> bugfix/backtracking_fail
        }
    }
}
