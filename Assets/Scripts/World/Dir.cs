using UnityEngine;

namespace WaifuTaxi
{
    public static class Dir
    {
        public static string DirVectorToString(Vector2Int dir)
        {
            string s = "none";
            if (dir == Vector2Int.up) {
                s = "UP";
            } else if (dir == Vector2Int.down) {
                s = "DOWN";
            } else if (dir == Vector2Int.right) {
                s = "RiGHT";
            } else if (dir == Vector2Int.left) {
                s = "LEFT";
            }

            return s;
        }
    }
}