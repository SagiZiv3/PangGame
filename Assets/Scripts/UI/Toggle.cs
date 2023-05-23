using UnityEngine;
using UnityEngine.Events;

namespace Pang.UI
{
    internal sealed class Toggle : MonoBehaviour
    {
        [SerializeField] private UnityEvent onToggledOn, onToggledOff;
        [SerializeField] private bool isOn;

        public void ToggleState()
        {
            if (isOn)
            {
                isOn = false;
                onToggledOff.Invoke();
            }
            else
            {
                isOn = true;
                onToggledOn.Invoke();
            }
        }
    }
}