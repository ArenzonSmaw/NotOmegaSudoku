using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuNator5000;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuTester
{
    [TestClass]
    public class InvalidValueTest
    {
        [TestMethod]
        public void TestInvalidVal1()
        {
            int[,] input = SudokuPlayer.StrToMat("008052078091040006705800400570083204600000017810764035156400320024030700387095641");

            Board puzzle = new Board(9);
            Assert.ThrowsException<InvalidInputException>(() => { puzzle.LoadBoard(input); });
        }

        [TestMethod]
        public void TestInvalidVal2()
        {
            int[,] input = SudokuPlayer.StrToMat("060107004016480650204659300009008000830746020046900081158300079072090006693000140");

            Board puzzle = new Board(9);
            Assert.ThrowsException<InvalidInputException>(() => { puzzle.LoadBoard(input); });
        }

        [TestMethod]
        public void TestInvalidVal3()
        {
            int[,] input = SudokuPlayer.StrToMat("078143000645970801910001240096000015500019060801405900784500000060830000059700184");

            Board puzzle = new Board(9);
            Assert.ThrowsException<InvalidInputException>(() => { puzzle.LoadBoard(input); });
        }


        [TestMethod]
        public void TestUnsolvable1()
        {
            int[,] input = SudokuPlayer.StrToMat("164809732000050000000000000000000000000000000000000000000000000000000000000000000");
            Board puzzle = new Board(9);
            puzzle.LoadBoard(input);
            Assert.ThrowsException<UnsolvableBoardException>(() => { puzzle.Solve(); });
        }

        [TestMethod]
        public void TestUnsolvable2()
        {
            int[,] input = SudokuPlayer.StrToMat("200900000000000060000001000502600407000004100000098023000003080005010000007000000");
            Board puzzle = new Board(9);
            puzzle.LoadBoard(input);
            Assert.ThrowsException<UnsolvableBoardException>(() => { puzzle.Solve(); });
        }

        [TestMethod]
        public void TestUnsolvable3()
        {
            int[,] input = SudokuPlayer.StrToMat("100000000000100000000000005000000100000000000000000000000000000000000010000000000");
            Board puzzle = new Board(9);
            puzzle.LoadBoard(input);
            Assert.ThrowsException<UnsolvableBoardException>(() => { puzzle.Solve(); });
        }
    }
}
