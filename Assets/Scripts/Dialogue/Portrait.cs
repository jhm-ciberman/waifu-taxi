using UnityEngine;

namespace WaifuTaxi
{
    [CreateAssetMenu(fileName = "Portrait", menuName = "WaifuTaxi/Portrait", order = 1)]
    public class Portrait : ScriptableObject
    {
        public Sprite angry;
        public Sprite asking;
        public Sprite blush;
        public Sprite normal;
    }
}