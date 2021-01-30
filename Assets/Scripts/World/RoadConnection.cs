namespace WaifuTaxi.World
{
    [System.Flags]
    public enum RoadConnection 
    {
        None = 0,
        Left = 1,
        Right = 2,
        Top = 4,
        Bottom = 8,

        Vertical  = Top | Bottom,
        Horizontal = Left | Right,

        TopLeft     = Top | Left,
        TopRight    = Top | Right,
        BottomLeft  = Bottom | Left,
        BottomRight = Bottom | Right,

        TLeft   = Top | Bottom | Left, 
        TRight  = Top | Bottom | Right, 
        TTop    = Left | Right | Top, 
        TBottom = Left | Right | Bottom, 

        Cross       = Top | Bottom | Left | Right,
    }
}