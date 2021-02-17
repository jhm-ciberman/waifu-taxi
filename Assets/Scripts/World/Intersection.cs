using System.Collections.Generic;
using UnityEngine;

namespace WaifuDriver
{
    public class Intersection
    {
        public Vector2Int coord;

        private List<Road> _roads = new List<Road>();

        public Intersection(Vector2Int coord)
        {
            this.coord = coord;
        }

        public void AddRoadTo(Intersection intersection)
        {
            this._roads.Add(new Road(this, intersection));
        }

        public IReadOnlyCollection<Road> roads => this._roads;
    }
}