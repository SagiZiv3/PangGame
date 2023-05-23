using System;
using UnityEngine;

namespace Pang.InputHandlers
{
    internal abstract class InputHandler : MonoBehaviour
    {
        public event Action OnFire;

        public float GetMovement()
        {
            if (enabled)
                return GetMovementImpl();
            return 0f;
        }

        public void Enable()
        {
            enabled = true;
        }

        public void Disable()
        {
            enabled = false;
        }

        protected void Fire()
        {
            if (enabled)
                OnFire?.Invoke();
        }

        protected abstract float GetMovementImpl();
    }
}