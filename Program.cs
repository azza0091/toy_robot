using Autofac;
using System;
using System.Configuration;
using toy_robot.Board.Interfaces;
using toy_robot.Robot.Interfaces;
using toy_robot.InputParser.Interfaces;

namespace toy_robot
{
    class Program
    {
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            const string instructions =
@"
1: Place the robot on the grid
    using the following command:

    PLACE X,Y,F where:
        - X is an integer from 0 to {0}
        - Y is an integer from 0 to {1} 
        - F can be NORTH, SOUTH, EAST or WEST

2: When the robot is placed, you can use
    the following commands:

    LEFT   – turns the robot 90 degrees left
    RIGHT  – turns the robot 90 degrees right
    MOVE   – Moves the robot 1 unit in the facing direction
    REPORT – Shows the current status of the robot
    EXIT   – Closes the application

";

            //initialize IoC container
            InitializeContainer();
            
            
            //get config parameters
            int rows = Convert.ToInt32(ConfigurationManager.AppSettings["boardRows"] ?? "0");
            int columns = Convert.ToInt32(ConfigurationManager.AppSettings["boardCols"] ?? "0");
            
            //print instructions
            Console.WriteLine("Welcome to  Toy Robot\n");
            Console.Write(String.Format(instructions, columns, rows));

            //resolve command parser
            IInputParser inputParser = Container.Resolve<IInputParser>();
            //resolve board and robot
            IBoard board = Container.Resolve<IBoard>(new NamedParameter("Rows", rows),
                                                     new NamedParameter("Columns", columns));
            IRobot robot = Container.Resolve<IRobot>(new NamedParameter("Board", board),
                                                     new NamedParameter("InputParser", inputParser));


            foreach (Response response in robot.ProcessCommands())
            {
                if (response.HasError)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(response.returnMessage);
                    Console.ResetColor();
                }
                else
                {
                    if (!String.IsNullOrEmpty(response.returnMessage))
                        Console.WriteLine(response.returnMessage);
                }
            }
        }

        private static void InitializeContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Board.Board>().As<IBoard>();
            builder.RegisterType<Robot.Robot>().As<IRobot>();
            builder.RegisterType<InputParser.ConsoleInputParser>().As<IInputParser>();
            Container = builder.Build();
        }
    }
}
