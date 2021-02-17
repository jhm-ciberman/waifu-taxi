namespace WaifuDriver
{
    public class City
    {
        public World.TileType[,] GetCity()
        {
            World.TileType X = World.TileType.Road; // road
            World.TileType o = World.TileType.Building; // building
            World.TileType p = World.TileType.Plaza; // plaza
            World.TileType z = World.TileType.Plaza; // plaza
            return new World.TileType[,] {
                { o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o },
                { o, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, o },
                { o, X, o, o, o, o, o, o, o, o, X, o, o, o, X, o, o, X, o, X, o, o, o, o, o, o, X, o, p, o, p, o, p, o, X, o },
                { o, X, o, o, o, o, o, o, o, o, X, o, o, o, X, o, o, X, o, X, o, o, o, o, o, o, X, o, o, o, p, o, o, o, X, o },
                { o, X, X, z, X, X, p, X, X, o, X, o, o, o, X, o, o, X, o, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, o },
                { o, X, o, o, o, X, o, o, X, o, X, X, o, o, X, o, o, X, o, X, o, o, o, o, p, o, X, o, o, o, o, o, o, o, X, o },
                { o, X, o, o, o, X, o, o, X, o, o, X, o, o, X, X, o, X, o, X, o, o, o, o, p, o, X, X, X, X, X, X, X, X, X, o },
                { o, X, o, o, o, X, o, o, X, o, o, z, o, o, o, o, o, X, X, X, X, X, o, o, p, o, X, o, X, p, p, X, o, o, X, o },
                { o, X, o, o, o, X, X, X, X, o, o, X, o, o, o, o, o, X, o, o, o, X, o, o, p, o, X, o, X, p, p, X, o, o, X, o },
                { o, X, o, o, o, X, o, o, o, o, o, X, X, X, X, X, X, X, o, o, o, X, o, o, p, o, z, o, X, p, p, X, o, o, X, o },
                { o, X, o, o, o, X, o, o, o, o, o, X, p, o, p, X, o, o, o, o, o, X, o, o, p, o, X, X, X, X, X, X, o, o, z, o },
                { o, X, o, o, o, X, o, o, o, o, o, X, o, o, o, z, o, o, o, o, o, z, o, o, p, o, X, o, o, o, o, X, o, o, X, o },
                { o, X, X, X, X, X, X, X, X, X, X, X, o, o, o, X, o, o, o, o, o, X, o, o, p, o, X, o, o, o, o, X, o, o, X, o },
                { o, X, o, o, o, o, o, o, o, o, o, X, o, o, o, X, X, X, X, X, X, X, X, X, X, X, X, o, o, o, o, X, o, o, X, o },
                { o, X, o, o, X, X, z, X, X, o, o, X, o, o, o, X, p, p, p, p, p, X, o, o, o, o, o, o, o, o, o, X, X, X, X, o },
                { o, X, o, o, X, p, p, p, X, o, p, X, o, o, o, X, X, X, z, X, X, X, o, o, o, o, o, o, o, o, o, X, o, o, X, o },
                { o, X, X, X, X, p, o, p, X, X, X, X, X, X, X, X, p, p, p, p, p, X, X, X, X, X, X, X, X, X, X, X, o, o, X, o },
                { o, X, o, o, X, p, p, p, X, o, X, p, o, o, o, X, X, X, X, X, X, X, o, X, o, X, o, o, o, X, p, o, o, o, X, o },
                { o, X, o, o, X, X, z, X, X, o, X, o, o, o, o, o, o, o, o, o, o, o, o, X, o, X, o, X, X, X, o, o, o, o, X, o },
                { o, X, o, o, o, o, o, o, o, o, X, o, o, o, o, o, o, o, o, o, o, o, o, X, o, X, o, X, o, o, o, o, o, o, X, o },
                { o, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, z, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, o },
                { o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o, o },
            };
        }
    }
}