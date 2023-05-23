using TMPro;
using UnityEngine;

namespace Pang.Animations
{
    internal sealed class TextFadeAnimation : Animation
    {
        [SerializeField] private TMP_Text text;
        [SerializeField, Range(0f, 1f)] private float targetOpacity;
        private Color startColor, currentColor;

        protected override void OnAnimationStarted()
        {
            startColor = text.color;
            currentColor = text.color;
        }

        protected override void OnAnimationEnded()
        {
            currentColor.a = targetOpacity;
            text.color = currentColor;
        }

        protected override void UpdateState(float progressPercentage)
        {
            currentColor.a = Mathf.Lerp(startColor.a, targetOpacity, progressPercentage);
            text.color = currentColor;
        }
    }
}