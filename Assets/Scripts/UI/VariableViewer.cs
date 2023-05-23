using Pang.NumericVariables;
using TMPro;
using UnityEngine;

namespace Pang.UI
{
    internal abstract class VariableViewer : MonoBehaviour
    {
        [SerializeField] private IntVariable healthVariable;

        private void OnEnable()
        {
            healthVariable.OnValueChanged += ShowValue;
        }

        private void OnDisable()
        {
            healthVariable.OnValueChanged -= ShowValue;
        }

        protected abstract void ShowValue(int currentValue);
    }
}