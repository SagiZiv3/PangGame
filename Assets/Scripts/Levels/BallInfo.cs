using UnityEngine;

namespace Pang.Levels
{
    [CreateAssetMenu(menuName = "Levels/Create Ball Info", fileName = "BallInfo")]
    internal sealed class BallInfo : ScriptableObject
    {
        [field: SerializeField] public Ball Prefab { get; private set; }
        [field: SerializeField] public int Level { get; private set; }

        [field: SerializeField, Tooltip("How much time after the level was loaded (in seconds) will the ball appear")]
        public float AppearAfter { get; private set; }
    }
}