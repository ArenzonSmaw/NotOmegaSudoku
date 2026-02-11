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

        private int[] rows_masks;
        private int[] cols_masks;
        private int[] blocks_masks;
        private int clipper;

        public BoardSolver(Board board)
        {
            this.board = board;
            this.squares = board.GetBoardMat();
            this.size = squares.GetLength(0);
            moveStack = new Stack<Move>();

            rows_masks = new int[size];
            cols_masks = new int[size];
            blocks_masks = new int[size];
            clipper = (1 << size) - 1;
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

            MakeMasks();

            try
            {
                SolveSingles();
                SolveByGuessing();
            }
            catch (Exception)
            { 
                throw new UnsolvableBoardException(); 
            }
            if (IsSolved())
                return true;
            else throw new UnsolvableBoardException();
        }

        public bool SolveSingles()
        {
            //solves all certain solutions - naked singles and hidden singles.
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
            
            int bestRow = -1;
            int bestCol = -1;
            int minCandidates = size + 1;

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    if (squares[r, c].GetValue() == 0)
                    {
                        int mask = ~(rows_masks[r] | cols_masks[c] | blocks_masks[CoordsToBlockNum(r, c)]) & clipper;
                        int count = CountSetBits(mask); 
                        if (count < minCandidates)
                        {
                            minCandidates = count;
                            bestRow = r; bestCol = c;
                        }
                    }
                }
            }

            
            if (bestRow == -1) return true;

            
            int possibleMask = ~(rows_masks[bestRow] | cols_masks[bestCol] | blocks_masks[CoordsToBlockNum(bestRow, bestCol)]) & clipper;

            for (int v = 1; v <= size; v++)
            {
                if ((possibleMask & (1 << (v - 1))) != 0)
                {
                    Move m = new Move(bestRow, bestCol, rows_masks[bestRow], cols_masks[bestCol], blocks_masks[CoordsToBlockNum(bestRow, bestCol)]);

                    if (CommitMove(m, v))
                    {
                        if (SolveByGuessing()) return true; // Recurse!
                        UndoMove(m); // Backtrack
                    }
                }
            }

            return false; // No number worked here, go back up a level
        }

        public int CountSetBits(int mask)
        {
            int count = 0;
            while (mask > 0)
            {
                count++;
                mask >>= 1;
            }
            return count;
        }

        public bool IsGuessLegal(Square sqr, int guess)
        {
            (int row, int col) = sqr.Coordinates;
            int block = CoordsToBlockNum(row, col);
            int mask = ~(rows_masks[row] | cols_masks[col] | blocks_masks[block]) & clipper;

            return ((mask & guess) != 0);
        }

        public int IsSolvable(Square sqr)
        {
            //gets square obj and coordinates
            //returns a solution if sqr is solvable, else 0

            if (sqr.GetValue() != 0)
                return 0;
            int solution;
            int row, col, block;
            (row, col) = sqr.Coordinates;
            block = CoordsToBlockNum(row, col);

            solution = IsNakedSingle(sqr, row, col, block);
            if (solution == 0)
            {
                solution = IsHiddenSingle(sqr, row, col, block);
            }

            return solution; 
        }
        public int IsNakedSingle(Square sqr, int row, int col, int block)
        {
            int notes = ~(rows_masks[row] | cols_masks[col] | blocks_masks[block]) & ((1 << size)-1);

            if (notes == 0)
                throw new InvalidInputException($"Square {sqr.Coordinates} has no possible solutions.");

            if ((notes & (notes - 1)) == 0) 
                return (int)(Math.Log(notes) / Math.Log(2)) + 1;

            return 0;
        }
        public int IsHiddenSingle(Square sqr, int row, int col, int block)
        {
            int notes = ~(rows_masks[row] | cols_masks[col] | blocks_masks[block]) & clipper;
            int solution = 1;

            while(notes > 0)
            {
                if ((notes & 1) == 0)
                    continue;
                if (IsUnique(row, col, solution))
                    return solution;
                notes >>= 1;
                solution++;
            }
            return 0;
        }
        public bool IsUnique(int row, int col, int value)
        {
            int count = 0;
            int sqrMask = 0;
            for(int icol = 0; icol < size; icol++)
            {
                if (squares[row, icol].GetValue() == 0)
                {
                    sqrMask = ~(rows_masks[row] | cols_masks[icol] | blocks_masks[CoordsToBlockNum(row, icol)]) & clipper;
                    if ((sqrMask & (1 << (value - 1))) != 0)
                        count++;
                }
            }
            if (count == 1) return true;
            
            count = 0;
            for (int irow = 0; irow < size; irow++)
            {
                if (squares[irow, col].GetValue() == 0)
                {
                    sqrMask = ~(rows_masks[irow] | cols_masks[col] | blocks_masks[CoordsToBlockNum(irow, col)]) & clipper;
                    if ((sqrMask & (1 << (value - 1))) != 0)
                        count++;
                }
            }
            if (count == 1) return true;

            count = 0;
            int root = (int)Math.Sqrt(size);
            int bRow = row - (row % root), bCol = col - (col % root);
            int block = CoordsToBlockNum(bRow, bCol);
            for (int i = 0; i < root; i++)
            {
                for (int j = 0; j < root; j++)
                {
                    if (squares[bRow + i, bCol + j].GetValue() == 0)
                    {
                        sqrMask = ~(rows_masks[bRow + i] | cols_masks[bCol + j] | blocks_masks[block]) & clipper;
                        if ((sqrMask & (1 << (value - 1))) != 0)
                            count++;
                    }
                }
            }
            if (count == 1) return true;
            return false;
        }
        public void SolveFor(Square sqr, int solution)
        {
            //Solves square by updating the value and pushing the move into the move stack
            (int row, int col) = sqr.Coordinates;

            Move move = new Move(row, col, rows_masks[row], cols_masks[col], blocks_masks[CoordsToBlockNum(row,col)]);
            //int mCount = moveStack.Count();
            if (!CommitMove(move, solution))
                throw new InvalidInputException($"Solution {solution} for square {sqr.Coordinates} is impossible.");
        }

        public bool GuessFor(Move move, int solution)
        {
            //gets: square object and possible solution
            //returns: true if board is solvable for this guess and false otherwise

            Move lastMove = moveStack.Count > 0 ? moveStack.Peek() : null;

            
            if (!CommitMove(move, solution))
                return false;
            Console.WriteLine(board);
            try
            {   
                if (Solve())
                {
                    return true;
                }
                
            }
            catch (UnsolvableBoardException)
            {
                RevertChanges(lastMove);
                return false;
            }
            RevertChanges(lastMove);
            return false;
        }

        public void RevertChanges(Move lastMove)
        {
            //gets a "checkpoint" from the moves stack and returns to that point
            //pops past moves and reverses them

            Move mv;
            while(moveStack.Count > 0 && moveStack.Peek() != lastMove) 
            {
                mv = moveStack.Pop();
                UndoMove(mv);
            }
            MakeMasks();
        }

        public void MakeMasks()
        {
            for (int i = 0; i < size; i++)
            {
                rows_masks[i] = 0;
                cols_masks[i] = 0;
                blocks_masks[i] = 0;

                UpdateRowMask(i);
                UpdateColMask(i);
                UpdateBlockMask(i);
            }
        }
        public int CoordsToBlockNum(int row, int col)
        {
            int root = (int)Math.Sqrt(size);
            return (row / root) * root + (col / root);
        }
        public (int, int) BlockNumToCoords(int block)
        {
            int root = (int)Math.Sqrt(size);
            return (block - (block % root), (block % root) * root);
        }
        public void UpdateRowMask(int row)
        {
            for (int col = 0; col < size; col++)
            {
                if (squares[row,col].GetValue() != 0) 
                    rows_masks[row] |= 1 << (squares[row, col].GetValue() - 1);
            }
        }
        public void UpdateColMask(int col)
        {
            for (int row = 0; row < size; row++)
            {
                if (squares[row, col].GetValue() != 0)
                    cols_masks[col] |= 1 << (squares[row, col].GetValue() - 1);
            }
        }
        public void UpdateBlockMask(int block)
        {
            int root = (int)Math.Sqrt(size);
            (int bRow, int bCol) = BlockNumToCoords(block);
            for (int i = 0; i < root; i++)
            {
                for (int j = 0; j < root; j++)
                {
                    if (squares[bRow + i, bCol + j].GetValue() != 0)
                        blocks_masks[block] |= 1 << (squares[bRow + i, bCol + j].GetValue() - 1);
                }
            }
        }
        public void UpdateMasks(int row, int column)
        {
            UpdateRowMask(row);
            UpdateColMask(column);
            UpdateBlockMask(CoordsToBlockNum(row,column));  
        }

        public bool CommitMove(Move move, int value)
        {
            int block = CoordsToBlockNum(move.Row, move.Col);
            int bit = 1 << (value - 1);
            if (((rows_masks[move.Row] | cols_masks[move.Col] | blocks_masks[block]) & bit) != 0)
                return false;

            rows_masks[move.Row]    |= bit;
            cols_masks[move.Col]    |= bit;
            blocks_masks[block]     |= bit;

            squares[move.Row, move.Col].SolveFor(value);
            moveStack.Push(move);

            return true;
        }
        public void UndoMove(Move move)
        {
            int block = CoordsToBlockNum(move.Row, move.Col);

            rows_masks[move.Row] = move.RowMask;
            cols_masks[move.Col] = move.ColMask;
            blocks_masks[block] = move.BlockMask;

            squares[move.Row, move.Col].Unsolve();
        }
    }
}
