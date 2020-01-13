using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace toy_robot.InputParser.Interfaces
{
    public interface IInputParser
    {
        IEnumerable<string> ReadCommand();
    }
}
