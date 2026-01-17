using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1;

namespace Sudoku.Objects 
{ 
    internal class Row : Series
    {
        private int rowNum;

        public Row(int _rowNum) : base()
        {
            this.rowNum = _rowNum;
            for (int i = 0; i < Series.size; i++)
            {
                this.squares[i] = null;
            }
        }

        public override bool Insert(Square square)
        {

            return false;
        }
    }
}
