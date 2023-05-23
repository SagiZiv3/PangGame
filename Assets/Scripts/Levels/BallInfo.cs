using UnityEngine;

namespace Pang.Levels
{
    [CreateAssetMenu(menuName = "Levels/Create Ball Info", fileName = "BallInfo")]
    internal sealed class BallInfo : ScriptableObject
    {
        [field: SerializeField] public Ball Prefab { get; private set; }
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public float AppearAfter { get; private set; }
    }
}