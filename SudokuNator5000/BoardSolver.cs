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
            bool didChange = true;
            Square sqr;
            while(didChange)
            {
                didChange = false;
                for (int row = 0; row < size; row++)
                {
                    for(int col = 0; col < size; col++)
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
            
            for( int minNotes = 2; minNotes <= size; minNotes++)
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

            if (IsSolved())
                return true;
            else throw new UnsolvableBoardException("Unsolvable Board: The given board could not be solved...");
        }

        public int IsSolvable(Square sqr)
        {
            //gets square obj and coordinates
            //returns a solution if sqr is solvable, else 0

            Square check;
            HashSet<int> notes = sqr.GetNotes();
            int row = sqr.Coordinates.Item1;
            int col = sqr.Coordinates.Item2;
            bool doesAppear;
            if (notes.Count() == 1)
                return notes.First();
            foreach (int note in notes)
            {
                doesAppear = false;
                for (int i = 0; i < size && !doesAppear; i++)
                {
                    check = squares[row, i];
                    if (check.GetValue() == note)
                        doesAppear = true;
                    else if (check.GetValue() ==0)
                        if(check != sqr &&  check.GetNotes().Contains(note)) 
                            //if the square is not solved, different from sqr and has note as a possibility
                            doesAppear = true;
                }
                if (!doesAppear) return note;

                doesAppear = false;
                for (int i = 0; i < size && !doesAppear; i++)
                {
                    check= squares[i, col];
                    if (check.GetValue() == note)
                        doesAppear = true;
                    else if (check.GetValue() == 0 && check != sqr && check.GetNotes().Contains(note))
                           //if the square is not solved, different from sqr and has note as a possibility
                           doesAppear = true;
                }
                if (!doesAppear) return note;

                int sqrt = (int)Math.Sqrt(size);
                doesAppear = false;
                int block_row = row - row%sqrt;
                int block_col = col - col%sqrt;

                for(int i = block_row; i < block_row + sqrt && !doesAppear; i++)
                {
                    for(int j = block_col; j < block_col + sqrt; j++)
                    {
                        check = squares[i, j];
                        if (check.GetValue() == note)
                            doesAppear = true;
                        else if (check.GetValue() == 0 && check != sqr && check.GetNotes().Contains(note))
                                //if the square is not solved, different from sqr and has note as a possibility
                                doesAppear = true;
                    }
                }
                if (!doesAppear) return note;
            }
            return 0;
        }

        public void SolveFor(Square sqr, int solution)
        {
            //Solves square by updating the value and pushing the move into the move stack
            if (!sqr.GetNotes().Contains(solution)) 
                throw new InvalidInputException($"Solution {solution} for square {sqr.Coordinates} is impossible.");
            (int, int) co_ordinatot = sqr.Coordinates;

            Move Move = new Move(sqr);
            int mCount = moveStack.Count();
            sqr.SolveFor(solution);
            moveStack.Push(Move);
            try
            {
                UpdateNotes(co_ordinatot.Item1, co_ordinatot.Item2, solution);
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

            (int, int) co_ordinatot = sqr.Coordinates;
            int moveCount = moveStack.Count();

            try
            {
                SolveFor(sqr, solution);
                Console.WriteLine(board);
                if (Solve())
                {
                    return true;
                }
                else
                {
                    RevertChanges(moveCount);
                }
            }
            catch (UnsolvableBoardException)
            {
                RevertChanges(moveCount);
                return false;
            }
            return false;
        }

        public void RevertChanges(int moveCount)
        {
            //gets a "checkpoint" from the moves stack and returns to that point
            //pops past moves and reverses them

            Square sqr;
            Move mv;
            while(moveStack.Count() > moveCount) 
            {
                mv = moveStack.Pop();
                squares[mv.Row, mv.Col] = mv.GetOldSquare();
            }
        }

        public HashSet<Square> FindWithNotes(int noteNum)
        {
            //finds square with noteNum amount of notes

            HashSet<Square> sqrSet = new HashSet<Square>();
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (squares[i,j].GetValue() == 0 && squares[i,j].GetNotes().Count() == noteNum)
                        sqrSet.Add(squares[i,j]);
            return sqrSet;
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
                    int block_row = i - i%block_size, block_col = j - j%block_size;

                    //Handle block
                    for (int r = block_row; r < block_row + block_size; r++)
                        for (int c = block_col; c < block_col + block_size; c++)
                        {
                            squares[i, j].CheckOff(squares[r, c].GetValue());
                            squares[r,c].GetValue();
                        }
                    if (squares[i, j].GetNotes().Count() == 1)
                        continue;
                    
                    for(int k = 0; k < size; k++)
                    {
                        squares[i, j].CheckOff(squares[i, k].GetValue()); // Row
                        squares[i, j].CheckOff(squares[k, j].GetValue()); // Column
                    }
                }
            }
        }
        public void UpdateNotes(int row, int col, int value) 
        {
            //updaets row column and block of the square so that the new value is checked off

            HashSet<Square> neighbors = new HashSet<Square>();
            Square sqr;
            Move mv;
            for (int i = 0; i < size; i++)
            {
                // Handle Row
                if (i != col)
                {
                    sqr = squares[row, i];

                    if (sqr.GetValue() == 0)
                    {
                        neighbors.Add(sqr);

                    }
                }
                // Handle Column
                if (i != row)
                {
                    sqr = squares[i, col];
                    if (sqr.GetValue() == 0)
                    {
                        neighbors.Add(sqr);

                    }
                }
            }


            //Handle block
            int block_size = (int)Math.Sqrt(size);
            int block_row = row - row % block_size, block_col = col - col % block_size;
            for (int i = block_row; i < block_row + block_size; i++)
            {
                for (int j = block_col; j < block_col + block_size; j++)
                {
                    sqr = squares[i, j];
                    if (sqr.GetValue() == 0 && (i, j) != (row, col))
                    {
                        neighbors.Add(sqr);
                    }
                }
            }

            foreach (Square square in neighbors)
            {
                mv = new Move(square);
                if (square.CheckOff(value))
                    moveStack.Push(mv);
            }
            //foreach (Square square in neighbors)
            //{
            //    if (square.GetNotes().Count() == 1)
            //        SolveFor(square, square.GetNotes().First());
            //}
        }
    }
}
