// Copyright (c) 2012 Alex Schimp
// Licensed under the MIT license (http://opensource.org/licenses/MIT).

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Diagnostics;

namespace MarbleSolitaireSolver
{
    public static class Program
    {
        [STAThread]
        public static void Main(params string[] args)
        {
            Console.WriteLine("MarbleSolitiareSolver - a very simple program that finds a solution to the \r\ntraditional marble solitaire game.");
            Console.WriteLine();
            Console.WriteLine("Copyright (c) 2012 Alex Schimp.  Licensed under the MIT license.");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press any key to begin...");
            Console.ReadKey();

            Stopwatch timer = new Stopwatch();
            MarbleSolitaireSolver solver = new MarbleSolitaireSolver();
            timer.Start();
            List<Move> moves = solver.Solve();
            timer.Stop();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Time to find a solution: {0} ms", timer.ElapsedMilliseconds);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Each move consists of a coordinate, which specifies the location \r\nof the marble to move, and a direction, which, of course, specifies \r\nthe direction to move the marble.");
            Console.WriteLine();
            Console.WriteLine("Coordinates are in the form (x, y), where x is the \r\nhorizontal axis, and y is the vertical axis.");
            Console.WriteLine();
            Console.WriteLine("A value of 0 for x indicates the far-left side of the board, \r\nand a value of 0 for y indicates the top of the board.  \r\nLikewise, a value of 6 for x is the far-right, and a value of \r\n6 for y is the bottom.");
            Console.WriteLine();
            Console.WriteLine("Moves ({0}):", moves.Count);
            foreach (var move in moves)
            {
                Console.WriteLine("({0}, {1}) direction: {2}", move.InitialLocation.X, move.InitialLocation.Y, Enum.GetName(typeof(Direction), move.Direction));
            }

            Console.ReadLine();
        }
    }
}
