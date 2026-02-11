using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuNator5000
{
    public class Move
    {
        //stores the coordinates and new value of a square
        private int row, col, row_mask, col_mask, block_mask;
        public Move(int row, int col, int row_mask, int col_mask, int block_mask)
        {
            this.row = row;
            this.col = col;
            this.row_mask = row_mask;
            this.col_mask = col_mask;
            this.block_mask = block_mask;
        }

        public int Row { get { return row; } }
        public int Col { get { return col; } }
        public int RowMask { get { return row_mask; } }
        public int ColMask { get { return col_mask; } }
        public int BlockMask { get { return block_mask; } }

    }
}
