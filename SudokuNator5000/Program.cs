using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuNator5000
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*int[,] sqrs = { { 0, 0, 0, 5, 0, 9, 0, 0, 0 }, 
                            { 0, 0, 0, 0, 0, 7, 0, 0, 0 }, 
                            { 0, 0, 9, 6, 2, 0, 0, 0, 0 }, 
                            { 0, 1, 0, 0, 0, 0, 6, 0, 5 }, 
                            { 9, 0, 6, 0, 0, 0, 0, 0, 0 },
                            { 5, 0, 3, 0, 0, 0, 0, 0, 9 },
                            { 0, 0, 0, 0, 5, 0, 0, 0, 0 },
                            { 0, 0, 0, 4, 0, 0, 0, 0, 0 },
                            { 4, 0, 0, 0, 9, 3, 0, 0, 1 } };
            Board brd = new Board(9);
            brd.LoadBoard(sqrs);

            brd.PrintBoard();

            Console.WriteLine(brd.Solve());

            brd.PrintBoard();*/
            
            Board brd2 = new Board(9);
            int[,] sqrs2 =
            {
                { 4, 0, 0, 0, 0, 3, 0, 0, 2 },
                { 0, 9, 2, 7, 8, 0, 5, 4, 0 },
                { 0, 3, 0, 0, 0, 9, 0, 1, 0 },
                { 6, 0, 9, 0, 0, 0, 0, 2, 0 },
                { 0, 7, 0, 0, 0, 0, 0, 6, 0 },
                { 0, 5, 0, 0, 0, 0, 1, 0, 7 },
                { 0, 6, 0, 8, 0, 0, 0, 5, 0 },
                { 0, 2, 5, 0, 7, 4, 6, 8, 0 },
                { 7, 0, 0, 9, 0, 0, 0, 0, 1 }
            };
            brd2.LoadBoard(sqrs2);
            brd2.PrintBoard();
            Console.WriteLine(brd2.Solve());
            brd2.PrintBoard();
        }
    }
}
