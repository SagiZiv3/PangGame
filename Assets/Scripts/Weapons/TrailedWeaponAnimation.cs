using UnityEngine;

namespace Pang.Weapons
{
    internal sealed class TrailedWeaponAnimation : WeaponAnimation
    {
        [SerializeField] private Transform bodyTransform;
        private float startScale;

        protected override void Animate(float deltaTime)
        {
            bodyTransform.localScale += Vector3.up * (speed * deltaTime);
        }

        protected override void ResetImpl()
        {
            Vector3 scale = bodyTransform.localScale;
            scale.y = startScale;
            bodyTransform.localScale = scale;
        }

        protected override void Initialize()
        {
            base.Initialize();
            startScale = bodyTransform.localScale.y;
        }
    }
}