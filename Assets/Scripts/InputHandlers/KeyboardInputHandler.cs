using System;
using UnityEngine;

namespace Pang.InputHandlers
{
    internal sealed class KeyboardInputHandler : InputHandler
    {
        [SerializeField] private KeyCode leftKeyCode = KeyCode.A, rightKeyCode = KeyCode.D, fireKeyCode = KeyCode.Space;

        protected override float GetMovementImpl()
        {
            if (Input.GetKey(leftKeyCode))
                return -1;
            if (Input.GetKey(rightKeyCode))
                return 1;
            return 0;
        }

        private void Update()
        {
            if (Input.GetKeyDown(fireKeyCode))
            {
                Fire();
            }
        }
    }
}