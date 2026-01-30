using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuNator5000
{
    public interface ISolvable
    {
        bool Solve();
        bool SolveOnes();
        bool Guess();
    }
}
