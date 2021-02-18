using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WaifuDriver
{
    public class RoadGraph
    {
        private static Vector2Int[] _dirs = new Vector2Int[] { 
            Vector2Int.left, 
            Vector2Int.right, 
            Vector2Int.up, 
            Vector2Int.down
        };

        public readonly struct CoordDir
        {
            public readonly Vector2Int coord;
            public readonly Vector2Int dir;

            public CoordDir(Vector2Int coord, Vector2Int dir)
            {
                this.coord = coord;
                this.dir = dir;
            }
        }

        private World _world;
        
        private Dictionary<CoordDir, Intersection> _intersectionsLeave = new Dictionary<CoordDir, Intersection>();
        private Dictionary<CoordDir, Intersection> _intersectionsEnter = new Dictionary<CoordDir, Intersection>();
        private List<Intersection> _intersectionsList = new List<Intersection>();

        private List<Road> _temporaryRoads = new List<Road>();

        public RoadGraph(World world)
        {
            this._world = world;

        }

        public World world => this._world;

        public Intersection FindIntersectionEnter(Vector2Int coord, Vector2Int cameDir)
        {
            this._intersectionsEnter.TryGetValue(new CoordDir(coord, cameDir), out Intersection intersection);
            return intersection;
        }

        public Intersection FindIntersectionLeave(Vector2Int coord, Vector2Int exitDir)
        {
            this._intersectionsLeave.TryGetValue(new CoordDir(coord, exitDir), out Intersection intersection);
            return intersection;
        }

        public Intersection CreateStart(Vector2Int start, Vector2Int startingDir)
        {
            var node = new Intersection(Intersection.Type.Enter, start, Vector2Int.zero, start);
            var end = this.FindNextIntersectionEnter(start, startingDir);
            if (end != null) {
                var road = node.AddRoadTo(end);
                this._temporaryRoads.Add(road);
            } else {
                foreach (var dir in RoadGraph._dirs) {
                    var road = this.FindNextIntersectionEnter(start, dir)?.AddRoadFrom(node);
                    if (road != null) {
                        this._temporaryRoads.Add(road);
                    }
                }
            }
            return node;
        }

        public Intersection CreateEnd(Vector2Int end)
        {
            var node = new Intersection(Intersection.Type.Leave, end, Vector2Int.zero, end);
            foreach (var dir in RoadGraph._dirs) {
                var road = this.FindNextIntersectionLeave(end, dir)?.AddRoadTo(node);
                if (road != null) this._temporaryRoads.Add(road);
            }

            return node;
        }

        public void RemoveTemporaryRoads()
        {
            foreach (var road in this._temporaryRoads) {
                road.start.RemoveRoad(road);
            }
            this._temporaryRoads.Clear();
        }

        public Intersection AddIntersectionEnter(Vector2Int coord, Vector2Int enterDir, Vector2 position)
        {
            var intersection = new Intersection(Intersection.Type.Enter, coord, enterDir, position);
            this._intersectionsEnter.Add(new CoordDir(coord, enterDir), intersection);
            this._intersectionsList.Add(intersection);
            return intersection;
        }

        public Intersection AddIntersectionLeave(Vector2Int coord, Vector2Int leaveDir, Vector2 position)
        {
            var intersection = new Intersection(Intersection.Type.Leave, coord, leaveDir, position);
            this._intersectionsLeave.Add(new CoordDir(coord, leaveDir), intersection);
            this._intersectionsList.Add(intersection);
            return intersection;
        }

        public Intersection FindNextIntersectionEnter(Vector2Int startCoord, Vector2Int searchDir)
        {
            Vector2Int coord = startCoord;
            while (this._world.HasRoad(coord)) {
                var intersection = this.FindIntersectionEnter(coord, searchDir);
                if (intersection != null) {
                    return intersection;
                }
                coord += searchDir;
            }
            return null;
        }

        public Intersection FindNextIntersectionLeave(Vector2Int startCoord, Vector2Int searchDir)
        {
            Vector2Int coord = startCoord;
            while (this._world.HasRoad(coord)) {
                var intersection = this.FindIntersectionLeave(coord, -searchDir);
                if (intersection != null) {
                    return intersection;
                }
                coord += searchDir;
            }
            return null;
        }

        public IEnumerable<Intersection> intersections => this._intersectionsList;
        public IEnumerable<Intersection> intersectionsEnter => this._intersectionsEnter.Values;
        public IEnumerable<Intersection> intersectionsLeave => this._intersectionsLeave.Values;

    }
}