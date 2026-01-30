using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace SudokuNator5000
{
    internal class BoardSolver
    {
        private Board board;
        private Square[,] squares;
        private bool isSolved;
        private int size;

        public BoardSolver(Board board)
        {
            this.board = board;
            this.squares = board.GetBoardMat();
            this.isSolved = true;
            this.size = squares.GetLength(0);
            foreach (Square sqr in squares)
            {
                if (sqr.GetValue() == 0)
                {
                    this.isSolved = false;
                    break;
                }
            }
        }

        public bool Solve()
        {
            Square sqr = this.FindWithNotes(1); //todo: implement method to find a square with X amount of notes
            while (sqr != null && !isSolved)
            {
                this.SolveFor(sqr, sqr.GetNotes()[0]); //todo: implement method to solve a square and update notes on neighbors
                sqr = this.FindWithNotes(1);
            }
            if (isSolved)
                return true;
            int minNotes = 2;
            while (minNotes <= size && !isSolved)
            {
                sqr = this.FindWithNotes(minNotes);
                while (sqr != null && !isSolved)
                {
                    int safenote = this.FindSafeNote(sqr); //todo: find 
                    this.GuessFor(sqr, sqr.GetNotes()[0]); //todo: implement method to guess a possibility for a square with backtracking
                    sqr = this.FindWithNotes(minNotes);
                }
                minNotes++;
            }
            return isSolved;
        }

        public void SolveFor(Square sqr, int solution)
        {

        }

        public void GuessFor(Square sqr, int solution)
        {

        }

        public Square FindWithNotes(int noteNum)
        {
            return null;
        }
    }
}
