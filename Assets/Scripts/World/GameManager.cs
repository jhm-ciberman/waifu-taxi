using UnityEngine;

namespace WaifuTaxi
{
    public class GameManager : MonoBehaviour
    {
        public WorldComponent worldComponent;

        private RoutePlanner _planner;

        public void Start()
        {
            var world = new World(new Vector2Int(20, 20));

            var player = this.worldComponent.GenerateWorld(world);

            this._planner = new RoutePlanner(world, player);
            this._planner.onIndication += (e) => {
                Debug.Log("Indication: " + e.indication);
                Debug.Log("Restarted: " + e.pathWasRestarted);
            };
            this._planner.UpdatePath();
        }

        void Update()
        {
            this._planner.UpdatePath();
        }
    }
}