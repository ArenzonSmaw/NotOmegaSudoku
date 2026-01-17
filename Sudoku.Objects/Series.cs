using Sudoku.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal abstract class Series
    {
        protected Square[] squares;
        protected HashSet<int> missing;
        private bool isSolved;
        public static int size = -1;

        public Series()
        {
            if (size == -1)
                throw new ArgumentException("Series size must be set before any series can be constructed");
            this.squares = new Square[size];
            this.missing = new HashSet<int>();
            isSolved = false;
        }

        public bool IsSolved() => isSolved;
        public HashSet<int> GetMissing() => missing;
        public bool IsMissing(int value) => missing.Contains(value);
        public int GetMissingCount() => missing.Count;

        protected void SetSolved(bool value) => isSolved = value;
        public abstract bool Insert(Square square);
    }
}
