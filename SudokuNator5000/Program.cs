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
            int[,] input = Program.StrToMat("130008900008000000790000080000600400003009070050020800001007090000500006000040200");
            string solution = "134768952528914367796352184819675423243189675657423819461237598382591746975846231";
            Board puzzle = new Board(9);
            puzzle.LoadBoard(input);
            puzzle.Solve();
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
            //gets input from user and returns a matrix representing the input

            string input;
            input = Console.ReadLine();

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
