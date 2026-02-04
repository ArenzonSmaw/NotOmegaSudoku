using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuNator5000
{
    public class Board 
    {
        private int size, root_size; // size of the board side, square root of that size
        private Square[,] board_mat;

        public Board (int size) 
        {
            this.root_size = (int)Math.Sqrt(size);
            if ((size % root_size) != 0)
                throw new InvalidBoardSizesException($"Invalid Board Sizes: Board size must be squared, got: {size}.");
            this.size = size;
            this.board_mat = new Square[size, size];
        }

        public void LoadBoard(int[,] numMatrix)
        {
            //gets: matrix of ints representing the sudoku board
            //loads the board onto the object and updates notes on each square
            if (numMatrix == null || numMatrix.GetLength(0) < size || numMatrix.GetLength(1) < size)
                throw new InvalidBoardSizesException("Invalid Board Sizes: input size does not fit the board size.");
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (numMatrix[i, j] != 0)
                    {
                        if (numMatrix[i, j] > size)
                            throw new InvalidCharacterException($"Invalid Character: Maximal value for square in {size} sized matrix is: {size}.");
                        else if (numMatrix[i, j] < 0)
                            throw new InvalidCharacterException($"Invalid Character: Value cannot be less than 0.");
                        this.board_mat[i, j] = new Square(numMatrix[i, j]);
                    }
                    else
                    {
                        this.board_mat[i, j] = new Square(size, i, j);
                    }
                }
            }
            CheckValid();
        }

        public Square[,] GetBoardMat() => board_mat;

        public void PrintBoard()
        {
            //prints the board

            int sqrSize = (int) Math.Sqrt(size);
            Console.ResetColor();
            for (int i = 0; i < size; i++)
                Console.Write("----");
            Console.WriteLine("-");
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (j % sqrSize != 0)
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write("| ");
                    Console.ResetColor();
                    if (board_mat[i, j].GetValue() == 0)
                        Console.Write(" ");
                    else
                        Console.Write(board_mat[i, j]);
                    Console.Write(" ");
                }
                if (i < size - 1)
                {
                    Console.Write("|\n|");

                    if (i % sqrSize != sqrSize - 1)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkBlue;

                            Console.Write("---");
                            if (j % sqrSize == sqrSize - 1)
                                Console.ResetColor();
                            Console.Write("|");

                        }
                    }
                    else
                    {
                        Console.ResetColor();
                        for (int j = 0; j < size*4-1; j++)
                        {
                            Console.Write("-");
                        }
                        Console.Write('|');
                    }
                        Console.ResetColor();
                    Console.WriteLine();
                }
                
            }
            Console.WriteLine("|");
            for (int i = 0; i < (size * 4) + 1; i++)
                Console.Write("-");
            Console.WriteLine("\n");
        }
        public bool CheckValid()
        {
            //checks all blocks, rows and columns. throws an invalid input exception if board is invalid.
            HashSet<int> existing;
            int value;
            for (int i = 0; i < size; i++)
            {
                existing = new HashSet<int>();
                for(int row = 0; row < size; row++)
                {
                    value = board_mat[row, i].GetValue();
                    if (value != 0 && existing.Contains(value))
                        throw new InvalidInputException($"Invalid Input: value {value} appears more than once in row {row}.");
                    existing.Add(value);
                }
                existing = new HashSet<int>();
                for (int col = 0; col < size; col++)
                {
                    value = board_mat[i, col].GetValue();
                    if (value != 0 && existing.Contains(value))
                        throw new InvalidInputException($"Invalid Input: value {value} appears more than once in col {col}.");
                    existing.Add(value);
                }
                
                existing = new HashSet<int>();
                int block_r = i - i%root_size, block_c = i%root_size * root_size;
                for (int b_row = 0; b_row < root_size; b_row++)
                {
                    for (int b_col = 0; b_col < root_size; b_col++)
                    {
                        value = board_mat[b_row + block_r, b_col + block_c].GetValue();
                        if (value != 0 && existing.Contains(value))
                            throw new InvalidInputException($"Iinvalid Input: value {value} appears more than once in block {i} (left corner is {block_r},{block_c}).");
                        existing.Add(value);
                    }
                }
            }
            return true;
        }
        public bool Solve()
        {
            //calls solver method
            BoardSolver solver = new BoardSolver(this);
            return solver.Solve();
        }
        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    str += board_mat[i, j].GetValue();
                }
            }
            return str;
        }
    }
}
