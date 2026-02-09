using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuNator5000
{
    public class Move
    {
        //stores the old square of a move1
        Square oldSqr;
        public Move(Square sqr)
        {
            oldSqr = new Square(sqr);
        }

        public int Row { get { return oldSqr.Coordinates.Item1; } }
        public int Col { get { return oldSqr.Coordinates.Item2; } }

        public Square GetOldSquare() => oldSqr;

    }
}
