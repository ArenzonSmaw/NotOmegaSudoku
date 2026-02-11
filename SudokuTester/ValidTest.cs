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
    public class ValidTest
    {
        [TestMethod]
        public void TestValid1()
        {
            int[,] input = SudokuPlayer.StrToMat("008052070091040006705800400570083204600000017810764035156400320024030700387095641");
            string solution = "468952173291347586735816492579183264643529817812764935156478329924631758387295641";
            Board puzzle = new Board(9);
            puzzle.LoadBoard(input);
            puzzle.Solve();
            Assert.AreEqual(puzzle.ToString(), solution);
        }

        [TestMethod]
        public void TestValid2()
        {
            int[,] input = SudokuPlayer.StrToMat("000050780000600010090020603100400000054260090007030001582910370000002500003000129");
            string solution = "236159784875643912491728653168497235354261897927835461582914376619372548743586129";
            Board puzzle = new Board(9);
            puzzle.LoadBoard(input);
            puzzle.Solve();
            Assert.AreEqual(puzzle.ToString(), solution);
        }

        [TestMethod]
        public void TestValid3()
        {
            int[,] input = SudokuPlayer.StrToMat("060107004010480650204659300009008000830746020046900081158300079072090006693000140");
            string solution = "365127894917483652284659317729518463831746925546932781158364279472891536693275148";
            Board puzzle = new Board(9);
            puzzle.LoadBoard(input);
            puzzle.Solve();
            Assert.AreEqual(puzzle.ToString(), solution);
        }

        [TestMethod]
        public void TestValid4()
        {
            int[,] input = SudokuPlayer.StrToMat("078143000645970801910000240096000015500019060801405900784501000060830000059700184");
            string solution = "278143596645972831913658247496287315527319468831465972784591623162834759359726184";
            Board puzzle = new Board(9);
            puzzle.LoadBoard(input);
            puzzle.Solve();
            Assert.AreEqual(puzzle.ToString(), solution);
        }

        [TestMethod]
        public void TestValid5()
        {
            int[,] input = SudokuPlayer.StrToMat("451000070000006800020700095210498060045203189089605243068007904030000720072150030");
            string solution = "451982376397546812826731495213498567645273189789615243168327954534869721972154638";
            Board puzzle = new Board(9);
            puzzle.LoadBoard(input);
            puzzle.Solve();
            Assert.AreEqual(puzzle.ToString(), solution);
        }

        [TestMethod]
        public void TestDifficult1()
        {
            int[,] input = SudokuPlayer.StrToMat("130008900008000000790000080000600400003009070050020800001007090000500006000040200");
            string solution = "134768952528914367796352184819675423243189675657423819461237598382591746975846231";
            Board puzzle = new Board(9);
            puzzle.LoadBoard(input);
            puzzle.Solve();
            Assert.AreEqual(puzzle.ToString(), solution);
        }
        [TestMethod]
        public void TestDifficult2()
        {
            int[,] input = SudokuPlayer.StrToMat("050080000004100900100009020008300700070004009000002010090600050003040006600008300");
            string solution = "259483671784126935136579428918365742372814569465792813897631254523947186641258397";
            Board puzzle = new Board(9);
            puzzle.LoadBoard(input);
            puzzle.Solve();
            Assert.AreEqual(puzzle.ToString(), solution);
        }
        [TestMethod]
        public void TestDifficult3()
        {
            int[,] input = SudokuPlayer.StrToMat("010000075000051604600007030007046001000700300540310000050003000800060000900000500");
            string solution = "214639875738251694695487132387946251169725348542318967451893726873562419926174583";
            Board puzzle = new Board(9);
            puzzle.LoadBoard(input);
            puzzle.Solve();
            Assert.AreEqual(puzzle.ToString(), solution);
        }
        [TestMethod]
        public void TestDifficult4()
        {
            int[,] input = SudokuPlayer.StrToMat("500090106103005092060020030001000009050000300006007001000870000002000600900200005");
            string solution = "527398146143765892869124537781532469254619378396487251615873924472951683938246715";
            Board puzzle = new Board(9);
            puzzle.LoadBoard(input);
            puzzle.Solve();
            Assert.AreEqual(puzzle.ToString(), solution);
        }
        [TestMethod]
        public void TestDifficult5()
        {
            int[,] input = SudokuPlayer.StrToMat("800003000500020001409605700000100309000004002050930060000200600000050070760300000");
            string solution = "817493526536728941429615738642187359973564812158932467385271694294856173761349285";
            Board puzzle = new Board(9);
            puzzle.LoadBoard(input);
            puzzle.Solve();
            Assert.AreEqual(puzzle.ToString(), solution);
        }

        [TestMethod]
        public void TestEmpty()
        {
            int[,] input = SudokuPlayer.StrToMat("000000000000000000000000000000000000000000000000000000000000000000000000000000000");
            Board puzzle = new Board(9);
            puzzle.LoadBoard(input);
            puzzle.Solve();
            Assert.IsTrue(puzzle.CheckValid());
        }
    }
}
