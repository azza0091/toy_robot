using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toy_robot.Board;
using toy_robot.Board.Interfaces;
using toy_robot.InputParser;
using toy_robot.InputParser.Interfaces;
using toy_robot.Robot;
using toy_robot.Robot.Interfaces;

namespace toy_robot_tests
{
    [TestClass]
    public class BaseTestClass
    {
        protected static IContainer Container;

        static BaseTestClass()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Board>().As<IBoard>();
            builder.RegisterType<Robot>().As<IRobot>();
            builder.RegisterType<ConsoleInputParser>().As<IInputParser>();
            Container = builder.Build();
        }
    }
}
