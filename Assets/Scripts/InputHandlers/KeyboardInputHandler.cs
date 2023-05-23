using System;
using UnityEngine;

namespace Pang.InputHandlers
{
    internal sealed class KeyboardInputHandler : InputHandler
    {
        protected override float GetMovementImpl()
        {
            if (Input.GetKey(KeyCode.A))
                return -1;
            if (Input.GetKey(KeyCode.D))
                return 1;
            return 0;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Fire();
            }
        }
    }
}