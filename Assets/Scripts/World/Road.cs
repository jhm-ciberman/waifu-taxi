using System.Collections.Generic;
using UnityEngine;

namespace WaifuDriver
{
    public class Road
    {        
        public readonly Intersection start;

        public readonly Intersection end;

        public readonly float length; // cached for speed

        public readonly float direction;

        public Road(Intersection start, Intersection end)
        {
            this.start = start;
            this.end = end;
            Vector2 vector = (start.position - end.position);
            this.direction = Vector2.Angle(Vector2.up, vector);
            this.length = vector.magnitude;
        }
    }
}