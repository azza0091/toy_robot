using toy_robot.Board.Interfaces;

namespace toy_robot.Board
{
    public class Board : IBoard
    {
        protected int Rows { get; set; }
        protected int Columns { get; set; }

        public Board(int Rows, int Columns)
        {
            this.Rows = Rows;
            this.Columns = Columns;
        }

        public bool CheckIfValidPosition(int x, int y)
        {
            return (x < Columns && x >= 0) && (y < Rows && y >= 0);
        }
    }
}
