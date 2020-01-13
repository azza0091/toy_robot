using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace toy_robot.Robot
{
    public class Position
    {
        public int x { get; set; }
        public int y { get; set; }
        public Orientations f { get; set; }

        public Position(int X, int Y, Orientations F)
        {
            x = X;
            y = Y;
            f = F;
        }
        public Position(Position Position)
        {
            x = Position.x;
            y = Position.y;
            f = Position.f;
        }
    }
}
