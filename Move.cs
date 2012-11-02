// Copyright (c) 2012 Alex Schimp
// Licensed under the MIT license (http://opensource.org/licenses/MIT).

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarbleSolitaireSolver
{
    /// <summary>
    /// Represents a move made on the board.
    /// </summary>
    public struct Move
    {
        private Coordinate initialLocation;
        private Coordinate jumpedLocation;
        private Coordinate finalLocation;
        private Direction direction;

        /// <summary>
        /// The initial location of the piece.
        /// </summary>
        public Coordinate InitialLocation { get { return initialLocation; } }

        /// <summary>
        /// The location of the piece that is jumped.
        /// </summary>
        public Coordinate JumpedLocation { get { return jumpedLocation; } }

        /// <summary>
        /// The final location of the piece after it is moved.
        /// </summary>
        public Coordinate FinalLocation { get { return finalLocation; } }

        /// <summary>
        /// The direction of the move.
        /// </summary>
        public Direction Direction { get { return direction; } }

        /// <summary>
        /// Initializes a new instance of the Move structure.
        /// </summary>
        /// <param name="initialLocation">The initial location of the piece moved.</param>
        /// <param name="jumpedLocation">The location of the piece that is jumped.</param>
        /// <param name="finalLocation">The final location of the piece after it is moved.</param>
        /// <param name="direction">The direction of the move.</param>
        public Move(Coordinate initialLocation, Coordinate jumpedLocation, Coordinate finalLocation, Direction direction)
        {
            this.initialLocation = initialLocation;
            this.jumpedLocation = jumpedLocation;
            this.finalLocation = finalLocation;
            this.direction = direction;
        }
    }

    /// <summary>
    /// An enumeration containing values which represent the direction of a move.
    /// </summary>
    public enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }
}
