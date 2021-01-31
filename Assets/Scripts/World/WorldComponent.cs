using UnityEngine;

namespace WaifuTaxi
{
    public class WorldComponent : MonoBehaviour
    {
        public Player player;
        public Car car;

        public Transform roadLine;
        public Transform roadCurve;
        public Transform roadCross;
        public Transform roadEnding;
        public Transform roadT;
        public Transform plaza;
        
        public Player GenerateWorld(World world)
        {
            for (int x = 0; x < world.size.x; x++) {
                for (int y = 0; y < world.size.y; y++) {
                    var pos = new Vector2Int(x, y);
                    var connection = world.GetRoadConnectionAt(pos);
                    this.MakeRoad(pos, connection);
                }
            }

            var numberOfCars = (int) (world.roadCount * 0.5f);
            for (int i = 0; i < numberOfCars; i++) {
                var pos = world.RandomRoad();
                this.SpawnCar(world, pos);
            }

            var playerPos = world.RandomRoad();
            return this.SpawnPlayer(world, playerPos);
        }

        public Player SpawnPlayer(World world, Vector2Int coords)
        {
            var playerPos = new Vector3(coords.x, coords.y, 0f);
            var player = Object.Instantiate(this.player, playerPos, Quaternion.identity);
            return player;
        }

        public void SpawnCar(World world, Vector2Int coords)
        {
            var playerPos = new Vector3(coords.x, coords.y, 0f);
            var car = Object.Instantiate(this.car, playerPos, Quaternion.identity);
            car.SetWorld(world);
        }

        public void MakeRoad(Vector2Int pos, RoadConnection connection)
        {
            Transform prefab = this.plaza;
            float angle = 0;
            switch (connection)
            {
                case RoadConnection.TopLeft: 
                    angle = 270; prefab = this.roadCurve; break;
                case RoadConnection.TopRight: 
                    angle = 0; prefab = this.roadCurve; break;
                case RoadConnection.BottomLeft: 
                    angle = 180; prefab = this.roadCurve; break;
                case RoadConnection.BottomRight: 
                    angle = 90; prefab = this.roadCurve; break;

                case RoadConnection.Vertical:
                    angle = 0; prefab = this.roadLine; break;
                case RoadConnection.Horizontal:
                    angle = 90; prefab = this.roadLine; break;

                case RoadConnection.Cross:
                    angle = 0; prefab = this.roadCross; break;

                case RoadConnection.TRight: 
                    angle = 0; prefab = this.roadT; break;
                case RoadConnection.TLeft: 
                    angle = 180; prefab = this.roadT; break;
                case RoadConnection.TBottom: 
                    angle = 90; prefab = this.roadT; break;
                case RoadConnection.TTop: 
                    angle = 270; prefab = this.roadT; break;

                case RoadConnection.Top: 
                    angle = 0; prefab = this.roadEnding; break;
                case RoadConnection.Bottom: 
                    angle = 180; prefab = this.roadEnding; break;
                case RoadConnection.Right: 
                    angle = 90; prefab = this.roadEnding; break;
                case RoadConnection.Left: 
                    angle = 270; prefab = this.roadEnding; break;
            }

            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            var obj = Object.Instantiate(prefab, new Vector3(pos.x, pos.y, 0f), rotation);
            obj.name = "Tile " + pos;
        }
    }
}