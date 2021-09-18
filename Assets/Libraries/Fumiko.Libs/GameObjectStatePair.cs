using UnityEngine;

namespace Fumiko.Common
{
    [System.Serializable]
    public class GameObjectStatePair
    {
        public GameObject gameObject;
        public EnabledState state;
    }
}