using System;
using System.Collections.Generic;
using System.Linq;
using toy_robot.Board.Interfaces;
using toy_robot.InputParser.Interfaces;
using toy_robot.Robot.Interfaces;

namespace toy_robot.Robot
{
    public class Robot : IRobot
    {
        private Position _position { get; set; }
        private IBoard _board { get; set; }
        private IInputParser _inputParser { get; set; }
        private bool _isPlaced { get; set; }
        
        public Robot(IBoard Board, IInputParser Parser)
        {
            _isPlaced = false;
            this._board = Board;
            this._inputParser = Parser;
        }

        public IEnumerable<object> ProcessCommands()
        {
            foreach (var command in _inputParser.ReadCommand())
            {
                Response response;
                try
                {
                    response = ParseInputCommand(command);
                }
                catch (Exception ex)
                {
                    response = new Response
                    {
                        HasError = true,
                        returnMessage = ex.Message
                    };
                }

                yield return response;
            }
        }

        public Response ParseInputCommand(string Input)
        {
            try
            {
                string command = Input.Split(' ').First();

                if (!_isPlaced && command != Actions.PLACE.ToString())
                {
                    return new Response
                    {
                        HasError = true,
                        returnMessage = ErrorMessage.NotPlaced.Value
                    };
                }

                switch (command)
                {
                    case nameof(Actions.PLACE):
                        var parameters = Input.Substring(command.Length);
                        List<string> startingPosition = parameters.Split(',').ToList();
                        if (startingPosition.Count == 3)
                        {
                            int x = Convert.ToInt32(startingPosition[0]);
                            int y = Convert.ToInt32(startingPosition[1]);
                            Orientations f;
                            try
                            {
                                f = (Orientations)Enum.Parse(typeof(Orientations), startingPosition[2]);
                            }
                            catch (Exception ex)
                            {
                                return new Response
                                {
                                    HasError = true,
                                    returnMessage = ErrorMessage.InvalidOrientationValue.Value
                                };
                            }
                            
                            if (!Enum.IsDefined(typeof(Orientations), f))
                            {
                                return new Response
                                {
                                    HasError = true,
                                    returnMessage = ErrorMessage.InvalidOrientationValue.Value
                                };
                            }

                            Place(x, y, f);
                        }
                        else
                        {
                            return new Response
                            {
                                HasError = true,
                                returnMessage = ErrorMessage.InvalidPlaceParams.Value
                            };
                        }
                        
                        break;
                    case nameof(Actions.LEFT):
                        RotateLeft();
                        break;
                    case nameof(Actions.RIGHT):
                        RotateRight();
                        break;
                    case nameof(Actions.MOVE):
                        Move();
                        break;
                    case nameof(Actions.REPORT):
                        return new Response
                        {
                            HasError = false,
                            returnMessage = GetCurrentPosition()
                        };
                    default:
                        return new Response
                        {
                            HasError = true,
                            returnMessage = ErrorMessage.InvalidCommand.Value
                        };
                }

                return new Response
                {
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    HasError = true,
                    returnMessage = ex.Message
                };
            }
        }

        public void Place(int X, int Y, Orientations F)
        {
            if (CheckBoundaries(new Position(X, Y, F)))
            {
                _position = new Position(X, Y, F);
            }
            else
            {
                throw new InvalidOperationException(ErrorMessage.InvalidPlacePosition.Value);
            }
            _isPlaced = true;
        }

        public void Move()
        {
            var nextPosition = CalculateNextPosition();
            if (CheckBoundaries(nextPosition))
                _position = nextPosition;
            else
                throw new InvalidOperationException(ErrorMessage.InvalidMovePosition.Value);
        }

        public void Rotate(int steps)
        {
            int orientationsCount = Enum.GetValues(typeof(Orientations)).Length;

            //ensure that the new orientation is always within the available orientations
            steps = steps % orientationsCount;
            _position.f = (Orientations)((int)(_position.f + steps) % orientationsCount);

            if (_position.f < 0)
                _position.f = _position.f + orientationsCount;
        }

        public string GetCurrentPosition()
        {
            return String.Format("{0},{1},{2}", _position.x, _position.y, _position.f);
        }


        private bool IsPlaced()
        {
            return _isPlaced;
        }

        private void RotateLeft()
        {
            Rotate(-1);
        }

        private void RotateRight()
        {
            Rotate(1);
        }

        private Position CalculateNextPosition()
        {
            Position nextPosition = new Position(_position);

            switch (_position.f)
            {
                case Orientations.NORTH:
                    nextPosition.y++;
                    break;
                case Orientations.EAST:
                    nextPosition.x++;
                    break;
                case Orientations.SOUTH:
                    nextPosition.y--;
                    break;
                case Orientations.WEST:
                    nextPosition.x--;
                    break;
            }

            return nextPosition;
        }

        private bool CheckBoundaries()
        {
            return CheckBoundaries(_position);
        }
        private bool CheckBoundaries(Position Position)
        {
            return _board.CheckIfValidPosition(Position.x, Position.y);
        }
    }
}
