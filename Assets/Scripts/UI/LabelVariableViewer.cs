using TMPro;
using UnityEngine;

namespace Pang.UI
{
    internal sealed class LabelVariableViewer : VariableViewer
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private string prefix = "Score:";

        protected override void ShowValue(int currentValue)
        {
            string text = currentValue.ToString();
            if (prefix.Length > 0)
            {
                text = $"{prefix} {text}";
            }

            label.SetText(text);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (label != null)
                ShowValue(0); // Show a preview of the text
        }
#endif
    }
}