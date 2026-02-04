using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuNator5000
{
    public abstract class Move
    {
        private int row, col;
        public Move (int row, int col)
        {
            this.row = row;
            this.col = col;
        }

        public int Row { get { return row; } }
        public int Col { get { return col; } }

        public abstract int GetValue();

    }

    public class SolvingMove : Move
    {
        private int newVal;
        public SolvingMove(int row, int col, int value) : base(row,col)
        {
            this.newVal = value;
        }
        public override int GetValue() => newVal;
    }

    public class CheckoffMove : Move
    {
        private int removed;
        public CheckoffMove(int row, int col, int value) : base(row, col)
        {
            this.removed = value;
        }
        public override int GetValue() => removed;
    }
}
