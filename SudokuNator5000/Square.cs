using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuNator5000
{
    public class Square
    {
        private int value;
        private HashSet<int> notes; // possible solutions for the square
        private (int, int) coords;

        public Square(char value)
        {
            if (value >= '0' && value <= '9')
                this.value = (int)(value - '0');
            else if ((value >= 'A' && value <= 'Z'))
                this.value = (int)(value + 10 - (int)'A');
            else if ((value >= 'a' && value <= 'z'))
                this.value = (int)(value + 10 - (int)'a');
            else
                throw new InvalidInputException("Square value must be a digit or a letter");
        }
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
                if (notes.Contains(value))
                {
                    notes.Remove(value);
                    if (notes.Count() == 0)
                        throw new InvalidInputException($"hold up how does this square have no possibilities? {Coordinates}");
                    return true;
                }
            }
            return false;
        }

        public int GetValue() => value;

        public HashSet<int> GetNotes() => notes;
        public (int, int) Coordinates { get { return coords; } }
        public void SolveFor(int newValue)
        {
            if (!notes.Contains(newValue))
                throw new InvalidInputException($"solution {newValue} for square {Coordinates} is wrong.");
            value = newValue;
        }
        public void RevertValue (int oldVal)
        {
            notes.Add(value);
            value = oldVal;
        }
        public void RevertNotes (int note)
        {
            notes.Add(note);
        }

        public override string ToString()
        {
            string str = "";
            if (value > 9)
                str += (char)(value-10 + 'A');
            else
                str += (char)(value + '0');

            return str;
        }
    }
}
