using UnityEngine;

namespace WaifuDriver
{
    public class WorldGenerator : MonoBehaviour
    {
        public Player player;

        public WorldPrefabs prefabs;
        
        private System.Random _random = new System.Random();

        public Player GenerateWorld(World world, Pathfinder pathfinder)
        {
            for (int x = 0; x < world.size.x; x++) {
                for (int y = 0; y < world.size.y; y++) {
                    var pos = new Vector2Int(x, y);
                    var connection = world.GetRoadConnectionAt(pos);
                    bool forceBuilding = (x == 0 || x == world.size.x - 1 || y == 0 || y == world.size.y - 1);
                    var type = world.GetTileType(pos);
                    if (forceBuilding) type = World.TileType.Building;
                    this.MakeRoad(pos, connection, type);
                }
            }

            var numberOfCars = 1; //(int) (world.roadCount * 0.5f);
            for (int i = 0; i < numberOfCars; i++) {
                var pos = world.RandomRoad();
                this.SpawnCar(pos, pathfinder);
            }

            var playerPos = world.RandomRoad();
            return this.SpawnPlayer(playerPos);
        }

        public Player SpawnPlayer(Vector2Int coords)
        {
            var playerPos = new Vector3(coords.x, coords.y, 0f);
            var player = Object.Instantiate(this.player, playerPos, Quaternion.identity);
            return player;
        }

        public void SpawnCar(Vector2Int coords, Pathfinder pathfinder)
        {
            var playerPos = new Vector3(coords.x, coords.y, 0f);
            var prefab = this.RandomCarPrefab();
            var car = Object.Instantiate(prefab, playerPos, Quaternion.identity);
            car.SetDeltaSpeed(((float) this._random.NextDouble()) * 0.2f - 0.1f);
            car.SetPathfinder(pathfinder);
        }

        public void MakeRoad(Vector2Int pos, RoadConnection connection, World.TileType tileType)
        {
            Transform prefab = null;
            float angle = 0;
            switch (connection)
            {
                case RoadConnection.TopLeft: 
                    angle = 270; prefab = this.prefabs.roadCurve; break;
                case RoadConnection.TopRight: 
                    angle = 0;   prefab = this.prefabs.roadCurve; break;
                case RoadConnection.BottomLeft: 
                    angle = 180; prefab = this.prefabs.roadCurve; break;
                case RoadConnection.BottomRight: 
                    angle = 90;  prefab = this.prefabs.roadCurve; break;

                case RoadConnection.Vertical:
                    angle = 0; prefab = this.prefabs.roadLine; break;
                case RoadConnection.Horizontal:
                    angle = 90; prefab = this.prefabs.roadLine; break;

                case RoadConnection.Cross:
                    angle = 0; prefab = this.prefabs.roadCross; break;

                case RoadConnection.TRight: 
                    angle = 0; prefab = this.prefabs.roadT; break;
                case RoadConnection.TLeft: 
                    angle = 180; prefab = this.prefabs.roadT; break;
                case RoadConnection.TBottom: 
                    angle = 90; prefab = this.prefabs.roadT; break;
                case RoadConnection.TTop: 
                    angle = 270; prefab = this.prefabs.roadT; break;

                case RoadConnection.Top: 
                    angle = 0; prefab = this.prefabs.roadEnding; break;
                case RoadConnection.Bottom: 
                    angle = 180; prefab = this.prefabs.roadEnding; break;
                case RoadConnection.Right: 
                    angle = 90; prefab = this.prefabs.roadEnding; break;
                case RoadConnection.Left: 
                    angle = 270; prefab = this.prefabs.roadEnding; break;

                default:
                    angle = this._random.Next(0, 4) * 90f;
                    if (tileType == World.TileType.Building) {
                        prefab = this.RandomBuildingPrefab();
                    } else {
                        prefab = this.RandomPlazaPrefab();
                    }
                    break;
            }

            this._InstantiateTile(prefab, pos, angle);
        }

        private void _InstantiateTile(Transform prefab, Vector2Int pos, float angle)
        {
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            var obj = Object.Instantiate(prefab, new Vector3(pos.x, pos.y, 0f), rotation);
            obj.name = "Tile " + pos;
        }

        private Transform RandomPlazaPrefab()    => this.prefabs.plazas[this._random.Next(0, this.prefabs.plazas.Length - 1)];
        private Transform RandomBuildingPrefab() => this.prefabs.buildings[this._random.Next(0, this.prefabs.buildings.Length - 1)];
        private Car RandomCarPrefab()            => this.prefabs.cars[this._random.Next(0, this.prefabs.cars.Length - 1)];
    }
}