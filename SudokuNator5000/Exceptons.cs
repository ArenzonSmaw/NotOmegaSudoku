using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuNator5000
{
    public class InvalidBoardSizesException : Exception
    {
        public InvalidBoardSizesException(string message) : base(message) { }
    }

    public class InvalidInputException : Exception
    {
        public InvalidInputException(string message) : base(message) { }
    }

    public class InvalidCharacterException : InvalidInputException
    {
        public InvalidCharacterException(string message) : base(message) { }
    }

    public class WrongSolutionException : InvalidInputException
    {
        public WrongSolutionException(string message) : base(message) { }
    }

    public class UnsolvableBoardException : InvalidInputException
    {
        public UnsolvableBoardException(string message) : base(message) { }
    }
}
