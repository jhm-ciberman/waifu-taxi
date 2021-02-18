using System.Collections.Generic;
using UnityEngine;

namespace WaifuDriver
{
    public class Intersection
    {
        public enum Type { Enter, Leave }

        public Type type;

        public Vector2Int coord;

        public Vector2Int dir; 

        public Vector2 position;

        private List<Road> _roads = new List<Road>();

        public Intersection(Type type, Vector2Int coord, Vector2Int dir, Vector2 position)
        {
            this.type = type;
            this.coord = coord;
            this.dir = dir;
            this.position = position;
        }

        public Road AddRoadTo(Intersection intersection)
        {
            var road = new Road(this, intersection);
            this._roads.Add(road);
            return road;
        }

        public Road AddRoadFrom(Intersection intersection)
        {
            var road = new Road(intersection, this);
            intersection._roads.Add(road);
            return road;
        }

        public void RemoveRoad(Road road)
        {
            this._roads.Remove(road);
        }

        public IReadOnlyList<Road> roads => this._roads;
    }
}