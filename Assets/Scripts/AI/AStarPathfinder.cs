using System;
using System.Collections.Generic;
using UnityEngine;

namespace WaifuDriver
{
    public class AStarPathfinder : IComparer<Vector2Int>
    {
        private Vector2Int _start;

        private Vector2Int _end;

        private HashSet<Vector2Int> _closedSet;

        private PriorityQueue<Vector2Int> _openSet;

        private Dictionary<Vector2Int, Node> _nodes;

        private INavigator _navigator;

        public AStarPathfinder(INavigator navigator)
        {
            this._navigator = navigator;
            this._closedSet = new HashSet<Vector2Int>();
            this._openSet = new PriorityQueue<Vector2Int>(this);
            this._nodes = new Dictionary<Vector2Int, Node>();
        }

        public float _GetNodeGScore(Vector2Int coord)
        {
            if (this._nodes.TryGetValue(coord, out Node node)) return node.gScore;
            return float.PositiveInfinity;
        }

        int IComparer<Vector2Int>.Compare(Vector2Int x, Vector2Int y)
        {
            return Math.Sign(this._nodes[x].fScore - this._nodes[y].fScore);
        }

        public interface INavigator
        {
            float HeuristicDistance(Vector2Int start, Vector2Int end);
            float WeightFunction(Vector2Int fromCoord, Vector2Int toCoord, Vector2Int cameFromCoord);
        }

        protected List<Vector2Int> _ReconstructPath(Vector2Int current)
        {
            var path = new List<Vector2Int>();
            
            path.Add(current);
            do {
                current = this._nodes[current].cameFrom;
                path.Add(current);
            } while (current != this._start);

            path.Reverse();

            return path;
        }

        readonly struct Node : IEquatable<Node>
        {
            public readonly Vector2Int coord;
            public readonly Vector2Int cameFrom;
            public readonly float gScore;
            public readonly float fScore;

            public Node(Vector2Int coord, Vector2Int cameFrom, float gScore, float fScore)
            {
                this.coord = coord;
                this.cameFrom = cameFrom;
                this.gScore = gScore;
                this.fScore = fScore;
            }

            public bool Equals(Node other)
            {
                return this.coord.Equals(other.coord);
            }

            public override int GetHashCode()
            {
                return this.coord.GetHashCode();
            }
        }

        public List<Vector2Int> Pathfind(Vector2Int start, Vector2Int end)
        {
            this._nodes.Clear();
            this._closedSet.Clear();
            this._openSet.Clear();
            
            this._start = start;
            this._end = end;

            float hCost = this._navigator.HeuristicDistance(this._start, this._end);
            this._nodes[this._start] = new Node(this._start, this._start, 0f, hCost);
            this._openSet.Enqueue(this._start);

            while (this._openSet.Count > 0) {
                var coords = this._openSet.Dequeue();
                this._closedSet.Add(coords);

                if (coords == this._end) {
                    return this._ReconstructPath(coords);
                }

                var node = this._nodes[coords];

                // Expand tile
                this._ExpandNode(ref node, new Vector2Int(coords.x - 1, coords.y));
                this._ExpandNode(ref node, new Vector2Int(coords.x + 1, coords.y));
                this._ExpandNode(ref node, new Vector2Int(coords.x, coords.y - 1));
                this._ExpandNode(ref node, new Vector2Int(coords.x, coords.y + 1));
            }

            return null; // Failure! 
        }

        private void _ExpandNode(ref Node currentNode, Vector2Int neighbourCoord)
        {
            if (this._closedSet.Contains(neighbourCoord)) return;

            float dist = this._navigator.WeightFunction(currentNode.coord, neighbourCoord, currentNode.cameFrom);
            float gScoreTentative = currentNode.gScore + dist;

            var neighbourGScore = this._GetNodeGScore(neighbourCoord);

            if (gScoreTentative < neighbourGScore) {
                // This path to neighbor is better than any previous one. Record it!
                float h = this._navigator.HeuristicDistance(neighbourCoord, this._end);
                float f = gScoreTentative + h;
                this._nodes[neighbourCoord] = new Node(neighbourCoord, currentNode.coord, gScoreTentative, f);

                if (! this._openSet.Contains(neighbourCoord)) {
                    this._openSet.Enqueue(neighbourCoord);
                }
            }
        }
    }
}