using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using toy_robot;
using toy_robot.Robot;
using toy_robot.Robot.Interfaces;
using toy_robot.Board.Interfaces;
using toy_robot.InputParser.Interfaces;

namespace toy_robot_tests.Tests
{
    /// <summary>
    /// Tests regarding robot, movement and input
    /// </summary>
    [TestClass]
    public class RobotTests : BaseTestClass
    {
        private IRobot robot;
        private IBoard board;
        private static int rows;
        private static int columns;

        [ClassInitialize]
        public static void ClassInit(TestContext Context)
        {
            //get config parameters
            rows = Convert.ToInt32(ConfigurationManager.AppSettings["boardRows"] ?? "0");
            columns = Convert.ToInt32(ConfigurationManager.AppSettings["boardCols"] ?? "0");
        }

        [TestInitialize]
        public void TestInit()
        {
            //resolve command parser
            IInputParser inputParser = Container.Resolve<IInputParser>();
            //resolve board and robot
            board = Container.Resolve<IBoard>(new NamedParameter("Rows", rows),
                                               new NamedParameter("Columns", columns));
            robot = Container.Resolve<IRobot>(new NamedParameter("Board", board),
                                              new NamedParameter("InputParser", inputParser));
        }

        [TestMethod]
        public void ParseInputCommand_InvalidCommand_NotInitialized()
        {
            //initialize test
            var input = "MOVE";

            //execute
            var response = robot.ParseInputCommand(input);

            //assert
            Assert.AreEqual(true, response.HasError);
            Assert.AreEqual(ErrorMessage.NotPlaced.Value, response.returnMessage);
        }

        [TestMethod]
        public void ParseInputCommand_InvalidCommand_UnknownCommand()
        {
            //initialize test
            var input = "NEWCMD";
            robot.Place(0, 0, Orientations.NORTH);

            //execute
            var response = robot.ParseInputCommand(input);

            //assert
            Assert.AreEqual(true, response.HasError);
            Assert.AreEqual(ErrorMessage.InvalidCommand.Value, response.returnMessage);
        }

        [TestMethod]
        public void ParseInputCommand_InvalidCommand_EmptyCommand()
        {
            //initialize test
            var input = "";
            robot.Place(0, 0, Orientations.NORTH);

            //execute
            var response = robot.ParseInputCommand(input);

            //assert
            Assert.AreEqual(true, response.HasError);
            Assert.AreEqual(ErrorMessage.InvalidCommand.Value, response.returnMessage);
        }

        [TestMethod]
        public void ParseInputCommand_InvalidCommand_MissingParameters()
        {
            //initialize test
            var input = "PLACE 1,1";

            //execute
            var response = robot.ParseInputCommand(input);

            //assert
            Assert.AreEqual(true, response.HasError);
            Assert.AreEqual(ErrorMessage.InvalidPlaceParams.Value, response.returnMessage);
        }

        [TestMethod]
        public void ParseInputCommand_InvalidCommand_InvalidOrientation()
        {
            //initialize test
            var input = "PLACE 1,1,5";

            //execute
            var response = robot.ParseInputCommand(input);

            //assert
            Assert.AreEqual(true, response.HasError);
            Assert.AreEqual(ErrorMessage.InvalidOrientationValue.Value, response.returnMessage);
        }

        [TestMethod]
        public void ParseInputCommand_ValidCommand()
        {
            //initialize test
            var input = "PLACE 1,1,NORTH";
            
            //execute
            var response = robot.ParseInputCommand(input);

            //assert
            Assert.AreEqual(false, response.HasError);
        }

        [TestMethod]
        public void Place_InRange()
        {
            //initialize test
            var x = 0;
            var y = 0;
            var f = Orientations.SOUTH;
            
            //execute
            robot.Place(x, y, f);

            //assert
            Assert.AreEqual(String.Format("{0},{1},{2}", x, y, f), robot.GetCurrentPosition());
        }

        [TestMethod]
        public void Place_OutOfRange()
        {
            //initialize test
            var x = Convert.ToInt32(ConfigurationManager.AppSettings["boardCols"]);
            var y = Convert.ToInt32(ConfigurationManager.AppSettings["boardRows"]);
            var f = Orientations.SOUTH;
            
            //execute and assert
            Assert.ThrowsException<System.InvalidOperationException>(() => robot.Place(x, y, f));
        }

        [TestMethod]
        public void Move_InRange_North()
        {
            //initialize test
            var starting_x = 0;
            var starting_y = 0;
            var starting_f = Orientations.NORTH;
            var ending_x = 0;
            var ending_y = 1;
            var ending_f = Orientations.NORTH;
            
            //execute
            robot.Place(starting_x, starting_y, starting_f);
            robot.Move();

            //assert
            Assert.AreEqual(String.Format("{0},{1},{2}", ending_x, ending_y, ending_f), robot.GetCurrentPosition());
        }

        [TestMethod]
        public void Move_InRange_East()
        {
            //initialize test
            var starting_x = 0;
            var starting_y = 0;
            var starting_f = Orientations.EAST;
            var ending_x = 1;
            var ending_y = 0;
            var ending_f = Orientations.EAST;
            
            //execute
            robot.Place(starting_x, starting_y, starting_f);
            robot.Move();

            //assert
            Assert.AreEqual(String.Format("{0},{1},{2}", ending_x, ending_y, ending_f), robot.GetCurrentPosition());
        }

        [TestMethod]
        public void Move_InRange_South()
        {
            //initialize test
            var starting_x = 1;
            var starting_y = 1;
            var starting_f = Orientations.SOUTH;
            var ending_x = 1;
            var ending_y = 0;
            var ending_f = Orientations.SOUTH;
            
            //execute
            robot.Place(starting_x, starting_y, starting_f);
            robot.Move();

            //assert
            Assert.AreEqual(String.Format("{0},{1},{2}", ending_x, ending_y, ending_f), robot.GetCurrentPosition());
        }

        [TestMethod]
        public void Move_InRange_West()
        {
            //initialize test
            var starting_x = 1;
            var starting_y = 1;
            var starting_f = Orientations.WEST;
            var ending_x = 0;
            var ending_y = 1;
            var ending_f = Orientations.WEST;
            
            //execute
            robot.Place(starting_x, starting_y, starting_f);
            robot.Move();

            //assert
            Assert.AreEqual(String.Format("{0},{1},{2}", ending_x, ending_y, ending_f), robot.GetCurrentPosition());
        }

        [TestMethod]
        public void Move_OutOfRange_North()
        {
            //initialize test
            var x = Convert.ToInt32(ConfigurationManager.AppSettings["boardCols"]) - 1;
            var y = Convert.ToInt32(ConfigurationManager.AppSettings["boardRows"]) - 1;
            var f = Orientations.NORTH;
            
            robot.Place(x, y, f);

            //execute and assert
            Assert.ThrowsException<System.InvalidOperationException>(() => robot.Move());
        }

        [TestMethod]
        public void Move_OutOfRange_East()
        {
            //initialize test
            var x = Convert.ToInt32(ConfigurationManager.AppSettings["boardCols"]) - 1;
            var y = Convert.ToInt32(ConfigurationManager.AppSettings["boardRows"]) - 1;
            var f = Orientations.EAST;
            
            robot.Place(x, y, f);

            //execute and assert
            Assert.ThrowsException<System.InvalidOperationException>(() => robot.Move());
        }

        [TestMethod]
        public void Move_OutOfRange_South()
        {
            //initialize test
            var x = 0;
            var y = 0;
            var f = Orientations.SOUTH;
            
            robot.Place(x, y, f);

            //execute and assert
            Assert.ThrowsException<System.InvalidOperationException>(() => robot.Move());
        }

        [TestMethod]
        public void Move_OutOfRange_West()
        {
            //initialize test
            var x = 0;
            var y = 0;
            var f = Orientations.WEST;
            
            robot.Place(x, y, f);

            //execute and assert
            Assert.ThrowsException<System.InvalidOperationException>(() => robot.Move());
        }

        [TestMethod]
        public void Rotate_Right_450()
        {
            //initialize test
            var starting_x = 0;
            var starting_y = 0;
            var starting_f = Orientations.NORTH;
            var ending_x = 0;
            var ending_y = 0;
            var ending_f = Orientations.EAST;
            //rotation==5 to perform a 450° counterclockwise rotation and ensure that the position is calculated correctly even after a complete rotation
            var rotation = 5;
            
            //execute
            robot.Place(starting_x, starting_y, starting_f);
            robot.Rotate(rotation);

            //assert
            Assert.AreEqual(String.Format("{0},{1},{2}", ending_x, ending_y, ending_f), robot.GetCurrentPosition());
        }

        [TestMethod]
        public void Rotate_Left_450()
        {
            //initialize test
            var starting_x = 0;
            var starting_y = 0;
            var starting_f = Orientations.NORTH;
            var ending_x = 0;
            var ending_y = 0;
            var ending_f = Orientations.WEST;
            //rotation==-5 to perform a 450° clockwise rotation and ensure that the position is calculated correctly even after a complete rotation
            var rotation = -5;
            
            //execute
            robot.Place(starting_x, starting_y, starting_f);
            robot.Rotate(rotation);

            //assert
            Assert.AreEqual(String.Format("{0},{1},{2}", ending_x, ending_y, ending_f), robot.GetCurrentPosition());
        }
    }
}
