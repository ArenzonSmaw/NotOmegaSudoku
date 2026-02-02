using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuNator5000
{
    internal class Square
    {
        private int value;
        private HashSet<int> notes; // possible solutions for the square
        private (int, int) coords;
        public Square(int value)
        {
            this.value = value;
            notes = null;
        }
        public Square(int boardSize, int row, int col)
        {
            //gets: set of existing values in the current series (row,col,block)
            //builds the square according to the already existing values
            coords = (row, col);
            notes = new HashSet<int>();
            for (int i = 1; i <= boardSize; i++)
            {
                notes.Add(i);
            }
            
            value = 0;
        }
        public bool CheckOff(int value)
        {
            if (this.value == 0)
            {
                notes.Remove(value);
                return true;
            }
            return false;
        }

        public int GetValue() => value;

        public HashSet<int> GetNotes() => notes;
        public (int, int) Coordinates { get { return coords; } }
        public void SolveFor(int newValue)
        {
            value = newValue;
        }
    }
}
