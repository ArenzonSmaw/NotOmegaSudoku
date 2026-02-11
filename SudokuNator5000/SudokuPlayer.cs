using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuNator5000
{
    public class SudokuPlayer
    {
        static bool quit;
        public static void Play()
        {
            Play(9);
        }
        public static void Play(int n)
        {
            Console.WriteLine("Welcome to The Sudokunator 5000!\n");
            quit = false;
            bool dynamic = n == 0;
            do
            {
                var sw = Stopwatch.StartNew();
                try
                {
                    Board brd;
                    Console.WriteLine("Please Enter a sudoku board ('quit' to exit):> ");
                    int[,] input = GetUserInput();
                    if (!quit)
                    {
                        brd = new Board(dynamic ? input.GetLength(0) : n);
                        brd.LoadBoard(input);
                        Console.WriteLine("Board Entered:");
                        brd.PrintBoard();

                        if (brd.Solve())
                        {
                            Console.WriteLine("Solved Board:");
                            brd.PrintBoard();
                            Console.WriteLine(brd); //for debugging purposes
                        }
                        else
                            Console.WriteLine("board unsolvable");
                        sw.Stop();
                        Console.WriteLine($"Time Elapsed: {(double)sw.ElapsedMilliseconds/1000.0} seconds.");
                    }
                }
                catch (Exception e)
                {
                    Console.Write($"{e.Message} \n try again:\n");
                }
            } while (!quit);
        }
        public static int[,] GetUserInput()
        {
            //gets input from user and returns a matrix representing the input

            string input;
            input = Console.ReadLine();

            if (input.Equals("quit"))
            {
                quit = true;
                return null;
            }

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
                return c - '0';
            if (c >= 'A' && c <= 'Z')
                return c - 'A';
            if (c >= 'a' && c <= 'z')
                return c - 'a';
            else throw new InvalidCharacterException("Square value must be a digit or a letter.");
        }
    }
}
