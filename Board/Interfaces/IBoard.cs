using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace toy_robot.Board.Interfaces
{
    public interface IBoard
    {
        bool CheckIfValidPosition(int x, int y);
    }
}
