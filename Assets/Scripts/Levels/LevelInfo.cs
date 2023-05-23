using System;
using UnityEngine;

namespace Pang.Levels
{
    [CreateAssetMenu(menuName = "Levels/Create Level Info", fileName = "LevelInfo")]
    internal sealed class LevelInfo : ScriptableObject
    {
        [field: SerializeField] public int Duration { get; private set; }
        [field: SerializeField] public BallInfo[] Balls { get; private set; }
        [field: SerializeField] public Background Background { get; private set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // Sort the balls based on their appearance order.
            Array.Sort(Balls, (ball1, ball2) => ball1.AppearAfter.CompareTo(ball2.AppearAfter));
        }
#endif
    }
}