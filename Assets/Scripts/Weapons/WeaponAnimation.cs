using UnityEngine;

namespace Pang.Weapons
{
    internal abstract class WeaponAnimation : MonoBehaviour
    {
        [SerializeField] private Transform headTransform;
        [SerializeField] protected float speed;
        private Vector3 originalPosition;

        [ContextMenu("Start")]
        public void StartAnimation()
        {
            enabled = true;
        }

        [ContextMenu("Stop")]
        public void StopAnimation()
        {
            enabled = false;
            ResetAnimation();
        }

        [ContextMenu("Stop")]
        public void PauseAnimation()
        {
            enabled = false;
        }

        [ContextMenu("Reset")]
        public void ResetAnimation()
        {
            headTransform.localPosition = originalPosition;
            ResetImpl();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            headTransform.Translate(Vector3.up * (speed * deltaTime));

            Animate(deltaTime);
        }

        protected abstract void Animate(float deltaTime);
        protected abstract void ResetImpl();

        protected virtual void Initialize()
        {
            originalPosition = headTransform.localPosition;
        }

        private void Start()
        {
            Initialize();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying)
                enabled = false;
        }
#endif
    }
}