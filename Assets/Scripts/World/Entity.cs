using UnityEngine;

namespace WaifuDriver
{
    public abstract class Entity : MonoBehaviour
    {
        protected float _angle = 0f;

        public float angle => this.transform.eulerAngles.z;

        public Vector2Int currentDirVector
        {
            get
            {
                var angle = this._angle + 90f;
                angle %= 360f;
                int dir = Mathf.CeilToInt((angle - 45f) / 90f);

                switch (dir) {
                    case 0: return new Vector2Int(1, 0);
                    case 1: return new Vector2Int(0, 1);
                    case 2: return new Vector2Int(-1, 0);
                    case 3: return new Vector2Int(0, -1);
                }
                return new Vector2Int(1, 0);
            }
        }

        public Vector2 currentPosition => this.transform.position;

        public Vector2Int currentCoord
        {
            get => new Vector2Int(Mathf.RoundToInt(this.transform.position.x), Mathf.RoundToInt(this.transform.position.y));
        }
    }
}