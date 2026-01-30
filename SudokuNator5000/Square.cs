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

        public Square(int value)
        {
            this.value = value;
            notes = null;
        }
        public Square(HashSet<int> series, int boardSize)
        {
            //gets: set of existing values in the current series (row,col,block)
            //builds the square according to the already existing values
            notes = new HashSet<int>();
            for (int i = 1; i <= boardSize; i++)
            {
                if(!series.Contains(i))
                    notes.Add(i);
            }
            value = 0;
        }
        public bool checkOff(int value)
        {
            if (notes.Contains(value))
            {
                notes.Remove(value);
                return true;
            }
            return false;
        }

        public int GetValue() => value;

        public HashSet<int> GetNotes() => notes;
    }
}
