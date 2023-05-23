using UnityEngine;

namespace Pang.Animations
{
    internal sealed class UiResizingAnimation : Animation
    {
        [SerializeField] private RectTransform uiWindow;
        [SerializeField] private Vector2 targetSize;

        private Vector2 startSize;

#if UNITY_EDITOR
        [ContextMenu("Set target to current size")]
        private void TargetToCurrentSize() => targetSize = uiWindow.sizeDelta;
#endif
        protected override void OnAnimationStarted()
        {
            startSize = uiWindow.sizeDelta;
        }

        protected override void OnAnimationEnded()
        {
            uiWindow.sizeDelta = targetSize;
        }

        protected override void UpdateState(float progressPercentage)
        {
            uiWindow.sizeDelta = Vector2.Lerp(startSize, targetSize, progressPercentage);
        }
    }
}