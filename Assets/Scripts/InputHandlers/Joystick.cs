using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pang.InputHandlers
{
    /// <summary>
    /// Source: https://youtu.be/UCJXgwSnXuU
    /// </summary>
    internal sealed class Joystick : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
    {
        public event Action<Vector2> OnMove;

        [SerializeField] private float dragThreshold = 0.6f;
        [SerializeField] private int dragMovementDistance = 30;
        [SerializeField] private int dragOffsetDistance = 100;
        private RectTransform joystickTransform;

        private void Awake()
        {
            joystickTransform = (RectTransform)transform;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // Move joystick back to the center.
            joystickTransform.anchoredPosition = Vector2.zero;
            OnMove?.Invoke(Vector2.zero);
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                joystickTransform,
                eventData.position,
                null,
                out Vector2 offset
            );
            offset = Vector2.ClampMagnitude(offset, dragOffsetDistance) / dragOffsetDistance;
            joystickTransform.anchoredPosition = offset * dragMovementDistance;
            OnMove?.Invoke(CalculateMovementInput(offset));
        }

        private Vector2 CalculateMovementInput(Vector2 offset)
        {
            float x = Mathf.Abs(offset.x) > dragThreshold ? offset.x : 0;
            float y = Mathf.Abs(offset.y) > dragThreshold ? offset.y : 0;
            return new Vector2(x, y);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }
    }
}