using UnityEngine;

namespace Pang
{
    [CreateAssetMenu(menuName = "Levels/Create Background Asset", fileName = "Background")]
    internal sealed class Background : ScriptableObject
    {
        [field: SerializeField] public Sprite[] Versions { get; private set; }
    }
}