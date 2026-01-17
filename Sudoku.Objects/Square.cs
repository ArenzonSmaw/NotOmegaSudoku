using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Objects
{
    internal class Square
    {
        private int solution;
        private HashSet<int> notes;
        private Column col;
        private Row row;
        private Block block;
        private bool isSolved;

        public Square(Row _row, Column _col, Block _block)
        {
            this.row = _row;
            this.col = _col;
            this.block = _block;
            this.notes = new HashSet<int>();
            this.solution = -1;
            this.isSolved = false;
        }
        public Square(Row _row, Column _col, Block _block, int value)
        {
            this.row = _row;
            this.col = _col;
            this.block = _block;
            this.notes = null;
            this.solution = value;
            this.isSolved = true;
        }

        public int GetSolution() => solution;
        public bool IsSolved() => isSolved;
        public Column GetCol() => col;
        public Row GetRow() => row;
        public Block GetBlock() => block;

    }
}
