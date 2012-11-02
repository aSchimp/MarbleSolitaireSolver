// Copyright (c) 2012 Alex Schimp
// Licensed under the MIT license (http://opensource.org/licenses/MIT).

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarbleSolitaireSolver
{
    /// <summary>
    /// A class for finding the solution to the traditional marble solitaire game.
    /// </summary>
    public class MarbleSolitaireSolver
    {
        /// <summary>
        /// A hashset of board configurations that have been determined not to have any potential of reaching the winning configuration.
        /// </summary>
        private HashSet<string> testedBoardConfigHashes = new HashSet<string>();

        /// <summary>
        /// The current state of the board, represented by a 2-dimensional array - 7x7.
        /// </summary>
        private State[,] _board;

        /// <summary>
        /// Initializes a new instance of the Solver class.
        /// </summary>
        public MarbleSolitaireSolver()
        {
            this.ResetBoard();
        }

        /// <summary>
        /// Resets the board to its initial state.
        /// </summary>
        public void ResetBoard()
        {
            this._board = new State[7, 7];

            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    // 2 x 2 corners are invalid
                    if (((x < 2 || x > 4) && (y < 2 || y > 4)))
                    {
                        this._board[x, y] = State.Invalid;
                    }
                    else if (x == 3 && y == 3)
                    {
                        // Middle location is open
                        this._board[x, y] = State.Open;
                    }
                    else
                    {
                        // Everything else is full
                        this._board[x, y] = State.Full;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a multi-line string representation of the current state of the board.
        /// </summary>
        /// <returns></returns>
        public string GetBoard()
        {
            StringBuilder builder = new StringBuilder();
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    builder.Append(((int)_board[x, y]).ToString());
                }

                builder.Append("\r\n");
            }

            return builder.ToString();
        }

        /// <summary>
        /// Solves the puzzle, and returns the list of moves performed to do so.
        /// </summary>
        /// <returns></returns>
        public List<Move> Solve()
        {
            return SolveStep(new List<Move>());
        }

        /// <summary>
        /// Solves one step of the puzzle (moving one piece) and calls itself recursively to find the solution.  Returns null if
        /// no solution was found at the end of the current path, or a list of moves if the solution was found.  To begin the solving
        /// process, call this method with an empty list or null for the parameter.
        /// </summary>
        /// <param name="movesSoFar">A list of the moves executed to get the board into its current state.</param>
        /// <returns></returns>
        private List<Move> SolveStep(List<Move> movesSoFar)
        {
            // Check if the current board configuration has already been determined to have no possible solutions
            if (testedBoardConfigHashes.Contains(GetBoardAsString(_board)))
                return null;

            if (movesSoFar == null)
                movesSoFar = new List<Move>();

            List<Move> moves = GetAllCurrentValidMoves();
            if (moves.Count > 0)
            {
                // Save the current state of the board, in case one of the moves doesn't work out
                State[,] currentBoard = new State[7, 7];
                Array.Copy(_board, currentBoard, 49);

                foreach (var move in moves)
                {
                    // Execute the move
                    ExecuteMove(move);

                    // Update the list of moves performed on the board
                    var movesCurrentlyDone = movesSoFar.ToList();
                    movesCurrentlyDone.Add(move);

                    // Move on to the next level/step
                    var result = SolveStep(movesCurrentlyDone);
                    if (result != null)
                    {
                        // If that path yielded a solution, return that list of moves used to get to that solution
                        return result;
                    }
                    else
                    {
                        // Otherwise, reset the board to its earlier state, which we saved above
                        Array.Copy(currentBoard, _board, 49);
                    }
                }

                // The current board configuration has no solutions
                testedBoardConfigHashes.Add(GetBoardAsString(_board));
                return null;
            }
            else
            {
                // There are no moves that can be executed (on the current level), so check if the board satisfies the conditions for winning.
                if (IsBoardInWinningState())
                {
                    // If yes, return the moves needed to get to this state.
                    return movesSoFar;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Executes a move on the board.  This method does NOT check to ensure that the move is valid.
        /// </summary>
        /// <param name="move">The move to perform.</param>
        private void ExecuteMove(Move move)
        {
            _board[move.InitialLocation.X, move.InitialLocation.Y] = State.Open;
            _board[move.JumpedLocation.X, move.JumpedLocation.Y] = State.Open;
            _board[move.FinalLocation.X, move.FinalLocation.Y] = State.Full;
        }

        /// <summary>
        /// Executes a list of moves on the board.  This method does NOT check to ensure that the moves are valid.
        /// </summary>
        /// <param name="moves">The list of moves to perform.</param>
        private void ExecuteMoves(List<Move> moves)
        {
            foreach (var move in moves)
            {
                ExecuteMove(move);
            }
        }

        /// <summary>
        /// Determines whether or not a move is valid given the current state of the board.
        /// </summary>
        /// <param name="move">The move to test.</param>
        /// <returns>Returns true if the move is valid, otherwise false.</returns>
        private bool IsMoveValid(Move move)
        {
            int initialX = move.InitialLocation.X;
            int initialY = move.InitialLocation.Y;
            int jumpedX = move.JumpedLocation.X;
            int jumpedY = move.JumpedLocation.Y;
            int finalX = move.FinalLocation.X;
            int finalY = move.FinalLocation.Y;

            if (initialX < 0 || initialX > 6 || initialY < 0 || initialY > 6)
                return false;
            if (finalX < 0 || finalX > 6 || finalY < 0 || finalY > 6)
                return false;

            if (_board[initialX, initialY] != State.Full)
                return false;
            if (_board[jumpedX, jumpedY] != State.Full)
                return false;
            if (_board[finalX, finalY] != State.Open)
                return false;

            return true;
        }

        /// <summary>
        /// Determines the final landing position of a move, given the move's initial location and direction.
        /// </summary>
        /// <param name="initialLocation">The initial location of the move.</param>
        /// <param name="direction">The direction of the move.</param>
        /// <returns>Returns the final landing position of the move as a Coordinate.</returns>
        private Coordinate GetFinalCoordinate(Coordinate initialLocation, Direction direction)
        {
            int finalX = initialLocation.X;
            int finalY = initialLocation.Y;

            switch (direction)
            {
                case Direction.Up:
                    finalY -= 2;
                    break;
                case Direction.Down:
                    finalY += 2;
                    break;
                case Direction.Left:
                    finalX -= 2;
                    break;
                case Direction.Right:
                    finalX += 2;
                    break;
            }

            return new Coordinate(finalX, finalY);
        }

        /// <summary>
        /// Determines the coordinate that will be jumped by a move, given the move's initial location and direction.
        /// </summary>
        /// <param name="initialLocation">The initial location of the move.</param>
        /// <param name="direction">The direction of the move.</param>
        /// <returns>Returns the coordinate that will be jumped by the move.</returns>
        private Coordinate GetJumpedCoordinate(Coordinate initialLocation, Direction direction)
        {
            int jumpedX = initialLocation.X;
            int jumpedY = initialLocation.Y;

            switch (direction)
            {
                case Direction.Up:
                    jumpedY--;
                    break;
                case Direction.Down:
                    jumpedY++;
                    break;
                case Direction.Left:
                    jumpedX--;
                    break;
                case Direction.Right:
                    jumpedX++;
                    break;
            }

            return new Coordinate(jumpedX, jumpedY);
        }

        /// <summary>
        /// Gets a list of all valid moves that could be performed on the board in its current state.
        /// </summary>
        /// <returns></returns>
        private List<Move> GetAllCurrentValidMoves()
        {
            List<Move> moves = new List<Move>();

            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    // If the initial coordinate is not full, the move isn't valid
                    if (_board[x, y] != State.Full)
                        continue;

                    Coordinate coordinate = new Coordinate(x, y);
                    for (int d = 0; d < 4; d++)
                    {
                        Direction direction = (Direction)d;
                        Move move = new Move(coordinate, GetJumpedCoordinate(coordinate, direction), GetFinalCoordinate(coordinate, direction), direction);
                        if (IsMoveValid(move))
                        {
                            moves.Add(move);
                        }
                    }
                }
            }

            return moves;
        }

        /// <summary>
        /// Determines whether or not the current state of the board satisfies the conditions for winning.
        /// </summary>
        /// <returns></returns>
        private bool IsBoardInWinningState()
        {
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    // 2 x 2 corners are invalid
                    if (((x < 2 || x > 4) && (y < 2 || y > 4)))
                    {
                        if (this._board[x, y] != State.Invalid)
                            return false;
                    }
                    else if (x == 3 && y == 3)
                    {
                        // Middle location should be full
                        if (this._board[x, y] != State.Full)
                            return false;
                    }
                    else
                    {
                        // Everything else should be empty
                        if (this._board[x, y] != State.Open)
                            return false;
                    }
                }
            }

            return true;
        }

        ///<summary>
        /// Gets a single-line string representation of the board specified by 'board'.
        /// </summary>
        /// <param name="board">The board to hash.</param>
        /// <returns></returns>
        private string GetBoardAsString(State[,] board)
        {
            StringBuilder builder = new StringBuilder();
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    builder.Append(((int)board[x, y]).ToString());
                }
            }

            return builder.ToString();
        }
    }

    /// <summary>
    /// An enumeration containing values which represent the state of a location on the board.
    /// </summary>
    public enum State
    {
        Invalid,
        Open,
        Full
    }
}
