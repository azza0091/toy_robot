using System.Collections.Generic;

namespace toy_robot.Robot.Interfaces
{
    public interface IRobot 
    {
        IEnumerable<object> ProcessCommands();
        Response ParseInputCommand(string Input);

        void Place(int X, int Y, Orientations F);
        void Move();
        void Rotate(int steps);
        string GetCurrentPosition();
    }
}
