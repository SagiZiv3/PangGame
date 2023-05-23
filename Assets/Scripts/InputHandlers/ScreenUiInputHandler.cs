using UnityEngine;
using UnityEngine.UI;

namespace Pang.InputHandlers
{
    internal sealed class ScreenUiInputHandler : InputHandler
    {
        [SerializeField] private Button fireButton;
        [SerializeField] private Joystick joystick;
        private Vector2 movement;

        private void OnEnable()
        {
            fireButton.onClick.AddListener(Fire);
            joystick.OnMove += UpdateMovement;
        }

        private void OnDisable()
        {
            fireButton.onClick.RemoveListener(Fire);
            joystick.OnMove -= UpdateMovement;
        }

        private void UpdateMovement(Vector2 input)
        {
            movement = input;
        }

        protected override float GetMovementImpl()
        {
            return movement.x;
        }
    }
}