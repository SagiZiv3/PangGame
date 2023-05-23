using UnityEngine;

namespace Pang.Weapons
{
    internal sealed class Weapon : MonoBehaviour
    {
        [SerializeField] private WeaponAnimation weaponAnimation;
        [SerializeField] private bool canFire = true;

        // [ContextMenu("Fire")]
        // private void Fire() => Fire(transform.position);


        public void Fire(Vector2 position)
        {
            if (!canFire) return;
            canFire = false;
            gameObject.SetActive(true);
            transform.position = new Vector3(position.x, position.y, transform.position.z);
            weaponAnimation.StartAnimation();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                HideWeapon();
            }
            else if (other.gameObject.TryGetComponent(out IHittable hittable))
            {
                hittable.HandleHit();
                HideWeapon();
            }
        }

        private void HideWeapon()
        {
            canFire = true;
            weaponAnimation.StopAnimation();
            gameObject.SetActive(false);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (weaponAnimation == null)
                weaponAnimation = GetComponentInChildren<WeaponAnimation>();
        }
#endif
    }
}