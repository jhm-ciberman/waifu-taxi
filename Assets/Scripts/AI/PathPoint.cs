using UnityEngine;

namespace WaifuDriver
{
    public readonly struct PathPoint
    {
        public readonly Vector2 position;

        public readonly float length;

        public PathPoint(Vector2 position, float length)
        {
            this.position = position;
            this.length = length;
        }
    }
}