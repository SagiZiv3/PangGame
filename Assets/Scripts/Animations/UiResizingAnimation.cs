using UnityEngine;

namespace Pang.Animations
{
    internal sealed class UiResizingAnimation : Animation
    {
        [SerializeField] private RectTransform uiWindow;

        // [SerializeField] private float animationDuration;
        // [SerializeField] private AnimationCurve animationCurve;
        // [SerializeField] private bool runOnStart = false;
        // [SerializeField] private UnityEvent postAnimationCallback;
        [SerializeField] private Vector2 targetSize;

        // private Coroutine animationCoroutine;
        private Vector2 startSize;

        // private void Start()
        // {
        //     if (runOnStart)
        //         StartAnimation();
        // }
        //
        // [ContextMenu("Start animation")]
        // public void StartAnimation()
        // {
        //     if (animationCoroutine != null)
        //     {
        //         Debug.LogWarning("Animation running, but StartAnimation was called");
        //         return;
        //     }
        //
        //     animationCoroutine = StartCoroutine(ResizeWindow());
        //     animationCoroutine = null;
        // }
        //
        // [ContextMenu("Stop animation")]
        // public void StopAnimation()
        // {
        //     if (animationCoroutine == null)
        //     {
        //         return;
        //     }
        //
        //     StopCoroutine(animationCoroutine);
        //     animationCoroutine = null;
        // }

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