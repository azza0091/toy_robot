using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toy_robot.Board.Interfaces;
using System.Configuration;
using Autofac;

namespace toy_robot_tests.Tests
{
    /// <summary>
    /// Tests regarding board and positioning
    /// </summary>
    [TestClass]
    public class BoardTests : BaseTestClass
    {
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
            board = Container.Resolve<IBoard>(new NamedParameter("Rows", rows),
                                              new NamedParameter("Columns", columns));
        }

        [TestMethod]
        public void CheckIfValidPosition_Valid()
        {
            //initialize test
            var x = 0;
            var y = 0;

            //execute
            var result = board.CheckIfValidPosition(x, y);

            //assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void CheckIfValidPosition_Invalid_x_upper()
        {
            //initialize test
            var x = columns;
            var y = 0;

            //execute
            var result = board.CheckIfValidPosition(x, y);

            //assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void CheckIfValidPosition_Invalid_x_lower()
        {
            //initialize test
            var x = -1;
            var y = 0;

            //execute
            var result = board.CheckIfValidPosition(x, y);

            //assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void CheckIfValidPosition_Invalid_y_upper()
        {
            //initialize test
            var x = 0;
            var y = rows;

            //execute
            var result = board.CheckIfValidPosition(x, y);

            //assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void CheckIfValidPosition_Invalid_y_lower()
        {
            //initialize test
            var x = 0;
            var y = -1;

            //execute
            var result = board.CheckIfValidPosition(x, y);

            //assert
            Assert.AreEqual(false, result);
        }
    }
}
