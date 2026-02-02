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
        HashSet<(int, int)> affectedSquares;
        public Move (int row, int col, int old_val, int new_value)
        {
            this.row = row;
            this.col = col;
            this.old_val = old_val;
            this.new_value = new_value;
            affectedSquares = new HashSet<(int, int)> ();
        }

        public int Row { get { return row; } }
        public int Col { get { return col; } }
        public int OldVal { get { return old_val; } }
        public int NewVal { get { return new_value; } }
        public HashSet<(int,int)> AffectedSqrs { get { return affectedSquares; }  }
        public void AddAffected(int row, int col)
        {
            affectedSquares.Add((row, col));
        }
        

        public void Revert(Square[,] squares)
        {
            squares[row, col].SolveFor(old_val);
        }

    }
}
