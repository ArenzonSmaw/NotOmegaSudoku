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
    public class InvalidInputTest
    {
        [TestMethod]
        public void TestInvalidChar1()
        {
            Assert.ThrowsException<InvalidCharacterException>(() => {
                int[,] input = SudokuPlayer.StrToMat("00805207V09104000670580040057008F20460000001781076B03515640032002403070038A095641");

                Board puzzle = new Board(9);
                puzzle.LoadBoard(input); 
            });
        }

        [TestMethod]
        public void TestInvalidChar2()
        {
            Assert.ThrowsException<InvalidCharacterException>(() => {
                int[,] input = SudokuPlayer.StrToMat("0080520700910400067058004005.0083204600000017810764035156400-20024030700387095641");

                Board puzzle = new Board(9);
                puzzle.LoadBoard(input); 
            });
        }

        [TestMethod]
        public void TestInvalidChar3()
        {
            Assert.ThrowsException<InvalidCharacterException>(() => {
                int[,] input = SudokuPlayer.StrToMat("0781430006459708019100002400960000155000190608014059007845010000608300000 9700184");

                Board puzzle = new Board(9); 
                puzzle.LoadBoard(input); 
            });
        }

        [TestMethod]
        public void TestInvalidInputSize1()
        {
            Assert.ThrowsException<InvalidBoardSizesException>(() => {
                int[,] input = SudokuPlayer.StrToMat("00184");

                Board puzzle = new Board(9); 
                puzzle.LoadBoard(input); 
            });
        }

        [TestMethod]
        public void TestInvalidInputSize2()
        {
            Assert.ThrowsException<InvalidBoardSizesException>(() => {
                int[,] input = SudokuPlayer.StrToMat("0012152152150152014514852125414852145202585485258548542589548652358784528614743684");

                Board puzzle = new Board(9);
                puzzle.LoadBoard(input); 
            });
        }

        [TestMethod]
        public void TestInvalidInputSize3()
        {
            Assert.ThrowsException<InvalidBoardSizesException>(() => { Board puzzle = new Board(10); });
        }
    }
}
