using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace SudokuNator5000
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            /*int[,] sqrs = { {1, 2, 3, 4 },
                            {2, 3, 4, 1 },
                            {3, 4, 1, 2 },
                            {4, 1, 2, 3 } };
            Board brd = new Board(4);
            brd.LoadBoard(sqrs);

            brd.PrintBoard();

            Console.WriteLine(brd.Solve());

            brd.PrintBoard();*/

            Board brd2 = new Board(9);
            int[,] sqrs2 = // WRONG!
            {
                { 3, 0, 0, 0, 4, 9, 0, 0, 0 },
    { 0, 0, 0, 6, 0, 0, 5, 0, 1 },
    { 7, 5, 2, 0, 0, 1, 0, 0, 0 },
    { 0, 0, 1, 0, 0, 0, 7, 0, 0 },
    { 5, 0, 0, 3, 9, 6, 0, 0, 0 },
    { 0, 0, 8, 1, 5, 0, 0, 9, 6 },
    { 0, 0, 3, 0, 1, 0, 0, 6, 0 },
    { 0, 0, 4, 0, 0, 0, 1, 0, 0 },
    { 0, 0, 0, 0, 2, 8, 0, 0, 0 }
            };
            brd2.LoadBoard(sqrs2);
            brd2.PrintBoard();
            sw = Stopwatch.StartNew();
            try
            {
                Console.WriteLine(brd2.Solve());
            }
            catch (UnsolvableBoardException e)
            {
                Console.WriteLine(e.Message);
            }
            sw.Stop();
            brd2.PrintBoard();
            Console.WriteLine(sw.Elapsed);
            //SudokuPlay();
        }

        public static void SudokuPlay()
        {
            while (true) {
                var sw = Stopwatch.StartNew();
                try
                {
                    Board brd;
                    Console.Write("Please Enter a sudoku board:> ");
                    int[,] input = GetUserInput();
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

        public static int[,] GetUserInput()
        {
            int[,] ret;
            string input;
            input = Console.ReadLine();
            int length = input.Length;
            int size = (int)Math.Sqrt(length);
            if (length % size != 0)
                throw new InvalidInputException("Board dimentions are not square");
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
            if (c >= '0' && c <= '9')
                return c - '0';
            if (c >= 'A' && c <= 'Z')
                return c - 'A';
            if (c >= 'a' && c <= 'z')
                return c - 'a';
            else throw new InvalidInputException("Square value must be a digit or a letter.");
        }
    }
}
