using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuNator5000
{
    internal class Move
    {
        private int row, col, old_val, new_value;

        public Move (int row, int col, int old_val, int new_value)
        {
            this.row = row;
            this.col = col;
            this.old_val = old_val;
            this.new_value = new_value;
        }

        public int Row { get { return row; } }
        public int Col { get { return col; } }
        public int OldVal { get { return old_val; } }
        public int NewVal { get { return new_value; } }

        public void Revert(Square[,] squares)
        {
            squares[row, col].SolveFor(old_val);
        }

    }
}
