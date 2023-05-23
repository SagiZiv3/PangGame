using UnityEngine;

namespace Pang
{
    [CreateAssetMenu(menuName = "Levels/Create Background Asset", fileName = "Background")]
    internal sealed class Background : ScriptableObject
    {
        [field: SerializeField, Tooltip("The same sprite in different aspect-ratios.")]
        public Sprite[] Versions { get; private set; }
    }
}