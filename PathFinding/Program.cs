using static System.Linq.Enumerable;
using static System.String;
using static System.Console;
using System.Collections.Generic;
using System;
using PathFinding;


namespace PathFinding
{
    class Program
    {
        static void Main(string[] args)
        {
            // Data required
            var edges = new List<Edge> {
                new Edge(0, 1, 10),
                new Edge(0, 2, 10),
                new Edge(0, 4, 5),
                new Edge(1, 1, 10),
                new Edge(1, 2, 5),
                new Edge(2, 1, 10),
                new Edge(2, 1, 5),
                new Edge(2, 3, 10),
                new Edge(3, 2, 10),
                new Edge(3, 4, 5),
                new Edge(4, 1, 5),
                new Edge(4, 3, 5),
                new Edge(4, 5, 20),
                new Edge(5, 4, 20),
                new Edge(2, 5, 2),
                new Edge(5, 2, 2),
                new Edge(6, 2, 1),
                new Edge(2, 6, 1),
                new Edge(6, 4, 7),
                new Edge(4, 6, 7),
            };

            // Initialise class
            var pathFinder = new PathFinder();
            pathFinder.BuildGraph(edges);

            // Usage = start node -> destination node
            var path = pathFinder.FindPath(0, 6);

            foreach (var node in path)
            {
                Console.WriteLine("Node: " + node);
            }
        }
    }

}