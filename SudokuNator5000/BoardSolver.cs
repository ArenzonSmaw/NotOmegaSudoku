using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace SudokuNator5000
{
    internal class BoardSolver
    {
        private Board board;
        private Square[,] squares;
        private int size;
        private Stack<Move> moveStack;

        public BoardSolver(Board board)
        {
            this.board = board;
            this.squares = board.GetBoardMat();
            this.size = squares.GetLength(0);
            moveStack = new Stack<Move>();
        }

        public Board GetBoard() => board;
        public bool IsSolved()
        {
            bool isSolved = true;
            foreach (Square square in squares)
            {
                if (square.GetValue() == 0)
                {
                    isSolved = false;
                    break;
                }
            }
            if (!isSolved) return false;
            try
            {
                board.CheckValid();
            }
            catch (InvalidInputException)
            { 
                return false; 
            }
            return true;
        }

        public HashSet<Square> GetRowOf(Square sqr)
        {
            int row = sqr.Coordinates.Item1;
            int col = sqr.Coordinates.Item2;
            HashSet<Square> sqrSet = new HashSet<Square>();
            for (int i = 0; i < size; i++)
            {
                if (i != col)
                    sqrSet.Add(squares[row, i]);
            }
            return sqrSet;
        }
        public HashSet<Square> GetColOf(Square sqr)
        {
            int row = sqr.Coordinates.Item1;
            int col = sqr.Coordinates.Item2;
            HashSet<Square> sqrSet = new HashSet<Square>();
            for (int i = 0; i < size; i++)
            {
                if (i != row)
                    sqrSet.Add(squares[i, col]);
            }
            return sqrSet;
        }
        public HashSet<Square> GetBlockOf(Square sqr)
        {
            int root = (int)Math.Sqrt(size);
            int block_row = (sqr.Coordinates.Item1 / root) * root;
            int block_col = (sqr.Coordinates.Item2 / root) * root;
            HashSet<Square> sqrSet = new HashSet<Square>();
            for (int offset_row = 0; offset_row < root; offset_row++)
            {
                for (int offset_col = 0; offset_col < root; offset_col++)
                {
                    if ((offset_row, offset_col) != sqr.Coordinates)
                        sqrSet.Add(squares[block_row + offset_row, block_col + offset_col]);
                }
            }
            return sqrSet;
        }

        public bool Solve()
        {
            //solves board first by marking obvious solutions, then guessing a square with a minimal note count

            try
            {
                MakeNotes();
            }
            catch (InvalidInputException)
            {
                throw new UnsolvableBoardException("Unsolvable Board: the board has no solution.");
            }

            SolveByGuessing();

            if (IsSolved())
                return true;
            else throw new UnsolvableBoardException("Unsolvable Board: The given board could not be solved...");
        }

        public bool SolveSingles()
        {
            bool didChange = true;
            Square sqr;
            while (didChange)
            {
                didChange = false;
                for (int row = 0; row < size; row++)
                {
                    for (int col = 0; col < size; col++)
                    {
                        sqr = squares[row, col];
                        if (sqr.GetValue() == 0)
                        {
                            int solution = IsSolvable(sqr);
                            if (solution != 0)
                            {
                                SolveFor(sqr, solution);
                                didChange = true;
                            }
                        }
                    }
                }
            }
            return IsSolved();
        }

        public bool SolveByGuessing()
        {
            Square sqr;

            for (int minNotes = 1; minNotes <= size; minNotes++)
            {
                for (int row = 0; row < size; row++)
                {
                    for (int col = 0; col < size; col++)
                    {
                        sqr = squares[row, col];
                        if (sqr.GetValue() == 0 && sqr.GetNotes().Count() == minNotes)
                        {
                            while (sqr.GetNotes().Count() > 0)
                            {
                                int solution = sqr.GetNotes().First();
                                if (GuessFor(sqr, solution))
                                    return true;
                                moveStack.Push(new Move(sqr));
                                sqr.GetNotes().Remove(solution);
                            }
                            throw new UnsolvableBoardException("Unsolvable Board: The given board could not be solved...");
                        }
                    }
                }
            }
            return IsSolved();
        }

        public int IsSolvable(Square sqr)
        {
            //gets square obj and coordinates
            //returns a solution if sqr is solvable, else 0

            HashSet<int> notes = sqr.GetNotes();
            if (notes.Count() == 1) //if given square has only 1 note
                return notes.First();
            
            HashSet<Square>[] neighbors = {
                            GetRowOf(sqr),      //page = 0
                            GetColOf(sqr),      //page = 1
                            GetBlockOf(sqr)     //page = 2
                            };
            

            for (int page = 0; page < 3; page++)
            {
                HashSet<int> diff = new HashSet<int>(notes);
                foreach(Square square in neighbors[page])
                {
                    if (diff == null) break;
                    if (square.GetValue() == 0)
                        diff.ExceptWith(square.GetNotes());
                }
                if (diff.Count() == 1) return diff.First();
            }

            return 0; //return unsolvable
        }

        public void SolveFor(Square sqr, int solution)
        {
            //Solves square by updating the value and pushing the move into the move stack
            if (!sqr.GetNotes().Contains(solution)) 
                throw new InvalidInputException($"Solution {solution} for square {sqr.Coordinates} is impossible.");
            (int row, int col) = sqr.Coordinates;

            Move Move = new Move(sqr);
            int mCount = moveStack.Count();
            sqr.SolveFor(solution);
            moveStack.Push(Move);
            try
            {
                UpdateNotes(row, col, solution);
            }
            catch (Exception)
            {
                throw new UnsolvableBoardException();
            }
        }

        public bool GuessFor(Square sqr, int solution)
        {
            //gets: square object and possible solution
            //returns: true if board is solvable for this guess and false otherwise

            int moveCount = moveStack.Count();

            try
            {
                SolveFor(sqr, solution);
                Console.WriteLine(board); /**/
                if (Solve())
                {
                    return true;
                }
                else
                {
                    RevertChanges(moveCount);
                    return false;
                }
            }
            catch (UnsolvableBoardException)
            {
                RevertChanges(moveCount);
                return false;
            }
        }

        public void RevertChanges(int moveCount)
        {
            //gets a "checkpoint" from the moves stack and returns to that point
            //pops past moves and reverses them

            Move mv;
            while(moveStack.Count() > moveCount) 
            {
                mv = moveStack.Pop();
                squares[mv.Row, mv.Col] = mv.GetOldSquare();
            }
        }

        public void MakeNotes()
        {
            //goes over the entire board and checks off all impossible possibilities

            int block_size = (int)Math.Sqrt(size);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {   
                    if (squares[i, j].GetValue() != 0) continue;

                    HashSet<Square>[] neighbors = {
                        GetRowOf(squares[i,j]),     //page=0
                        GetColOf(squares[i,j]),     //page=1
                        GetBlockOf(squares[i,j])    //page=2
                    };
                    
                    for (int page = 0; page < 3; page++)
                    {
                        foreach(Square sqr in neighbors[page])
                        {
                            squares[i, j].CheckOff(sqr.GetValue());
                        }
                    }
                }
            }
        }
        public void UpdateNotes(int row, int col, int value) 
        {
            //updaets row column and block of the square so that the new value is checked off

            HashSet<Square>[] neighbors = { 
                GetRowOf(squares[row, col]),        // i = 0
                GetColOf(squares[row, col]),        // i = 1
                GetBlockOf(squares[row, col]) };    // i = 2
            Move mv;

            for (int i = 0; i < 3; i ++)
            {
                foreach (Square sqr in neighbors[i])
                {
                    if (sqr.GetValue() == 0) { 
                        mv = new Move(sqr);
                        if (sqr.CheckOff(value))
                            moveStack.Push(mv);
                    }
                }
            }

            //foreach (Square square in neighbors)
            //{
            //    if (square.GetNotes().Count() == 1)
            //        SolveFor(square, square.GetNotes().First());
            //}
        }
    }
}
