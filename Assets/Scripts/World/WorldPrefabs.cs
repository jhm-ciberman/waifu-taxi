using UnityEngine;

namespace WaifuTaxi
{
    [CreateAssetMenu(fileName = "WorldPrefabs", menuName = "WaifuTaxi/WorldPrefabs", order = 1)]
    public class WorldPrefabs : ScriptableObject
    {
        public float buildingsDensity = 0.85f;

        public Transform roadLine;
        public Transform roadLineAlternative;
        public Transform roadCurve;
        public Transform roadCross;
        public Transform roadEnding;
        public Transform roadT;
        public Transform[] plazas;
        public Transform[] buildings;
    }
}