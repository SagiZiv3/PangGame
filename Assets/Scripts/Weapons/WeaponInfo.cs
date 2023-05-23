using UnityEngine;

namespace Pang.Weapons
{
    [CreateAssetMenu(menuName = "Weapons/Create Weapon Info", fileName = "WeaponInfo")]
    internal sealed class WeaponInfo : ScriptableObject
    {
        [SerializeField] private WeaponAnimation weaponAnimationPrefab;
        [SerializeField] private int maxShots = 1;
    }
}