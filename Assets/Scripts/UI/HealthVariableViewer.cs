using UnityEngine;
using UnityEngine.UI;

namespace Pang.UI
{
    internal sealed class HealthVariableViewer : VariableViewer
    {
        [SerializeField] private Image[] hearts;

        protected override void ShowValue(int currentHealth)
        {
            if (currentHealth < hearts.Length)
                hearts[currentHealth].enabled = false;
        }
    }
}