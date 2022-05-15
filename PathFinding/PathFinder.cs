﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Linq.Enumerable;
using EdgeList = System.Collections.Generic.List<(int node, double weight)>;

namespace PathFinding
{
    public class PathFinder
    {
        private Graph _graph;

        public void BuildGraph(List<Edge> edges)
        {
            _graph = new Graph(edges.Count);

            foreach (var edge in edges)
            {
                _graph.AddEdge(edge.StartNode, edge.DestinationNode, edge.Cost);
            }
        }

        public List<int> FindPath(int start, int destination)
        {
            return _graph?.FindPath(start, destination) ?? new List<int>();
        }

        private class Graph
        {
            private readonly List<EdgeList> _adjacency;

            public Graph(int vertexCount) => _adjacency = Range(0, vertexCount).Select(v => new EdgeList()).ToList();

            public int Count => _adjacency.Count;
            public bool HasEdge(int s, int e) => _adjacency[s].Any(p => p.node == e);
            public bool RemoveEdge(int s, int e) => _adjacency[s].RemoveAll(p => p.node == e) > 0;

            public bool AddEdge(int s, int e, double weight)
            {
                if (HasEdge(s, e)) return false;
                _adjacency[s].Add((e, weight));
                return true;
            }

            private (double distance, int prev)[] FindPath(int start)
            {
                var info = Range(0, _adjacency.Count).Select(i => (distance: double.PositiveInfinity, prev: i)).ToArray();
                info[start].distance = 0;
                var visited = new System.Collections.BitArray(_adjacency.Count);

                var heap = new Heap<(int node, double distance)>((a, b) => a.distance.CompareTo(b.distance));
                heap.Push((start, 0));
                while (heap.Count > 0)
                {
                    var current = heap.Pop();
                    if (visited[current.node]) continue;
                    var edges = _adjacency[current.node];
                    for (int n = 0; n < edges.Count; n++)
                    {
                        int v = edges[n].node;
                        if (visited[v]) continue;
                        double alt = info[current.node].distance + edges[n].weight;
                        if (alt < info[v].distance)
                        {
                            info[v] = (alt, current.node);
                            heap.Push((v, alt));
                        }
                    }
                    visited[current.node] = true;
                }
                return info;
            }

            public List<int> FindPath(int start, int destination)
            {
                var pathCalculation = FindPath(start);
                var pathList = new List<int>();

                pathList.Add(destination);
                for (int i = destination; i != start; i = pathCalculation[i].prev)
                {
                    //yield return (pathCalculation[pathCalculation[i].prev].distance, pathCalculation[i].prev);
                    pathList.Add(pathCalculation[i].prev);
                }
                pathList.Reverse();
                return pathList;
            }
        }

        private class Heap<T>
        {
            private readonly IComparer<T> comparer;
            private readonly List<T> list = new List<T> { default };

            public Heap() : this(default(IComparer<T>)) { }

            public Heap(IComparer<T> comparer)
            {
                this.comparer = comparer ?? Comparer<T>.Default;
            }

            public Heap(Comparison<T> comparison) : this(Comparer<T>.Create(comparison)) { }

            public int Count => list.Count - 1;

            public void Push(T element)
            {
                list.Add(element);
                SiftUp(list.Count - 1);
            }

            public T Pop()
            {
                T result = list[1];
                list[1] = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
                SiftDown(1);
                return result;
            }

            private static int Parent(int i) => i / 2;
            private static int Left(int i) => i * 2;
            private static int Right(int i) => i * 2 + 1;

            private void SiftUp(int i)
            {
                while (i > 1)
                {
                    int parent = Parent(i);
                    if (comparer.Compare(list[i], list[parent]) > 0) return;
                    (list[parent], list[i]) = (list[i], list[parent]);
                    i = parent;
                }
            }

            private void SiftDown(int i)
            {
                for (int left = Left(i); left < list.Count; left = Left(i))
                {
                    int smallest = comparer.Compare(list[left], list[i]) <= 0 ? left : i;
                    int right = Right(i);
                    if (right < list.Count && comparer.Compare(list[right], list[smallest]) <= 0) smallest = right;
                    if (smallest == i) return;
                    (list[i], list[smallest]) = (list[smallest], list[i]);
                    i = smallest;
                }
            }

        }
    }

    public class Edge
    {
        public int StartNode { get; set; }
        public int DestinationNode { get; set; }
        public int Cost { get; set; }

        public Edge(int startNode, int destinationNode, int distance)
        {
            StartNode = startNode;
            DestinationNode = destinationNode;
            Cost = distance;
        }
    }

}
