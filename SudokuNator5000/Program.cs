using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace SudokuNator5000
{
    public class Program
    {
        static void Main(string[] args)
        {
            //SudokuPlayer.Play();
            int[,] input = SudokuPlayer.StrToMat("008052070091040006705800400570083204600000017810764035156400320024030700387095641");
            string solution = "468952173291347586735816492579183264643529817812764935156478329924631758387295641";
            Board puzzle = new Board(9);
            puzzle.LoadBoard(input);
            puzzle.Solve();
            Console.WriteLine("solution: "+ solution);
        }
    }
}
