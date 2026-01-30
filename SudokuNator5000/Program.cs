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
            int[,] sqrs = { { 0, 0, 0, 1 }, { 0, 2, 3, 0 }, { 3, 4, 1, 2 }, { 2, 0, 0, 3 } };
            Board brd = new Board(4);
            brd.LoadBoard(sqrs);

            brd.printBoard();

            Console.WriteLine(brd.Solve());

            brd.printBoard();
        }
    }
}
