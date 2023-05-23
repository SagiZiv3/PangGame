using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Pang.Animations
{
    internal abstract class Animation : MonoBehaviour
    {
        [SerializeField] private float animationDuration;
        [SerializeField] private AnimationCurve animationCurve;
        [SerializeField] private bool runOnStart = false;
        [SerializeField] private UnityEvent postAnimationCallback;
        private Coroutine animationCoroutine;

        private void Start()
        {
            if (runOnStart)
                StartAnimation();
        }

        [ContextMenu("Start animation")]
        public void StartAnimation()
        {
            if (animationCoroutine != null)
            {
                Debug.LogWarning("Animation running, but StartAnimation was called");
                return;
            }

            animationCoroutine = StartCoroutine(Animate());
            animationCoroutine = null;
        }

        [ContextMenu("Stop animation")]
        public void StopAnimation()
        {
            if (animationCoroutine == null)
            {
                return;
            }

            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }

        private IEnumerator Animate()
        {
            float timePassed = 0;
            OnAnimationStarted();
            while (timePassed < animationDuration)
            {
                float percentage = animationCurve.Evaluate(timePassed / animationDuration);
                UpdateState(percentage);
                timePassed += Time.deltaTime;
                yield return null;
            }

            OnAnimationEnded();
            postAnimationCallback.Invoke();
            animationCoroutine = null;
        }

        protected abstract void OnAnimationStarted();
        protected abstract void OnAnimationEnded();
        protected abstract void UpdateState(float progressPercentage);
    }
}