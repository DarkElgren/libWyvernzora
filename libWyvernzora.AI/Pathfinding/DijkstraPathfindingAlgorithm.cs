using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libWyvernzora.Collections;
using libWyvernzora.Core;
using libWyvernzora.Utilities;

namespace libWyvernzora.AI.Pathfinding
{
    /// <summary>
    /// Implementation of Dijkstra Pathfinding Algorithm.
    /// </summary>
    /// <typeparam name="T">Type of graph nodes.</typeparam>
    public class DijkstraPathfindingAlgorithm<T> : IDisposable where T : INode
    {
        protected IGraph<T> graph;

        protected readonly Heap<T> openNodes;
        protected readonly Dictionary<INode, Pair<Double, Double>> scores;
        protected readonly Dictionary<T, T> links;
        protected readonly Dictionary<T, Boolean> closed; 

        protected Int32 searchLimit;


        public DijkstraPathfindingAlgorithm(IGraph<T> graph)
        {
            this.graph = graph;

            openNodes = new Heap<T>(new MappedComparer<T, Double>(node =>
                {
                    var score = scores[node];
                    return score.First + score.Second;
                }));

            scores = new Dictionary<INode, Pair<double, double>>();
            links = new Dictionary<T, T>();
            closed = new Dictionary<T, bool>();

            searchLimit = 0;
        }

        #region Pathfinding

        public T[] FindPath(T origin, T destination)
        {
            // Search Counter
            Counter searchCounter = new Counter();

            // Put origin node into the open list
            scores[origin] = new Pair<double, double>(0.0, GetHeruistics(origin, destination));
            closed[origin] = true;
            //openNodes.Insert(origin);

            // Current Node
            T current = origin;

            // Start Iterating
            while (true)
            {
                // Get all nodes adjacent to the current
                IEnumerable<T> adjacent = graph.GetAdjacentNodes(current);

                // Get G score of current node
                Double gCurrent = scores[current].First;

                foreach (var node in adjacent)
                {
                    // Skip Closed Nodes
                    if (closed.ContainsKey(node))
                        continue;

                    // Increment Counter
                    searchCounter.Count();

                    if (scores.ContainsKey(node))
                    {
                        // Node already open
                        Double g = scores[node].First;

                        // Consider Relink
                        Double weight = graph.GetEdge(current, node).Weight;
                        if (gCurrent + weight < g)
                            // Relinking the node to the current node produces a better solution
                            links[node] = current;
                    }
                    else
                    {
                        // Node not open yet, set it up
                        Double weight = graph.GetEdge(current, node).Weight;
                        scores[node] = new Pair<double, double>(gCurrent + weight, GetHeruistics(node, destination));
                        links[node] = current;

                        // Put the node into the open list
                        openNodes.Insert(node);
                    }
                }

                // Check if open list is empty
                if (openNodes.IsEmpty) return null;

                // Get the best move
                current = openNodes.DeleteRoot();
                closed[current] = true;

                // Check if it is the destination
                if (current.Equals(destination)) break;

                // Search Limit
                if (searchLimit > 0 && searchCounter.Value > searchLimit)
                    return null;
            }

            // Assemble Path
            List<T> path = new List<T>();
            current = destination;
            while (links.ContainsKey(current))
            {
                path.Add(current);
                current = links[current];
            }
            path.Reverse();

            return path.ToArray();
        }

        protected virtual Double GetHeruistics(T current, T destination)
        {
            return 1.0;
        }

        #endregion

        public void Clear()
        {
            openNodes.Clear();
            scores.Clear();
            links.Clear();
            closed.Clear();
        }

        public void Dispose()
        {

        }

    }
}
