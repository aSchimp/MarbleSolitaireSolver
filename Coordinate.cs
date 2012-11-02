// Copyright (c) 2012 Alex Schimp
// Licensed under the MIT license (http://opensource.org/licenses/MIT).

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarbleSolitaireSolver
{
    /// <summary>
    /// Represents a location on the board.
    /// </summary>
    public struct Coordinate
    {
        private int _x;

        /// <summary>
        /// The x value of the coordinate.
        /// </summary>
        public int X
        {
            get { return _x; }
        }

        private int _y;

        /// <summary>
        /// The y value of the coordinate.
        /// </summary>
        public int Y
        {
            get { return _y; }
        }

        /// <summary>
        /// Initializes a new instance of the Coordinate structure with the specified x and y values.
        /// </summary>
        /// <param name="x">The x value of the new Coordinate.</param>
        /// <param name="y">The y value of the new Coordinate.</param>
        public Coordinate(int x, int y)
        {
            this._x = x;
            this._y = y;
        }
    }
}
