using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace toy_robot
{
    public class Response
    {
        public bool HasError { get; set; }
        public string returnMessage { get; set; }
    }

    public class ErrorMessage
    {
        private ErrorMessage(string value) { Value = value; }

        public string Value { get; set; }

        public static ErrorMessage NotPlaced { get { return new ErrorMessage("Invalid command: robot must be initialized with PLACE command first!"); } }
        public static ErrorMessage InvalidCommand { get { return new ErrorMessage("Invalid command: the specified command does not exist"); } }
        public static ErrorMessage InvalidPlaceParams { get { return new ErrorMessage("Invalid syntax: the PLACE command requires parameters X,Y,F"); } }
        public static ErrorMessage InvalidPlacePosition { get { return new ErrorMessage("Invalid command: the robot would be placed off the board"); } }
        public static ErrorMessage InvalidOrientationValue { get { return new ErrorMessage("Invalid syntax: parameter F contains an unsupported value"); } }
        public static ErrorMessage InvalidMovePosition { get { return new ErrorMessage("Invalid command: the robot would fall off the board"); } }
    }
}
