using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using toy_robot.InputParser.Interfaces;

namespace toy_robot.InputParser
{
    public class ConsoleInputParser : IInputParser
    {
        public IEnumerable<string> ReadCommand()
        {
            var inputCommand = "";
            do
            {
                inputCommand = Console.ReadLine().ToUpperInvariant();
                if (inputCommand != "EXIT")
                {
                    yield return inputCommand;
                }
            }
            while (inputCommand != "EXIT");
        }
    }
}
