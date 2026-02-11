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
        //private HashSet<int> notes; // possible solutions for the square
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
        public Square(int row, int col, int value)
        {
            //gets: coordinates and value of a square
            //builds the square

            this.value = value;
            coords = (row, col);
            value = 0;
        }
        public Square(Square other)
        {
            value = other.GetValue();
            coords = other.coords;
        }

        public int GetValue() => value;

        public (int, int) Coordinates { get { return coords; } }

        public void SolveFor(int newValue)
        {
            value = newValue;
        }
        public void Unsolve()
        {
            value = 0;
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
