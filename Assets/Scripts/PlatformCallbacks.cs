using UnityEngine;
using UnityEngine.Events;

namespace Pang
{
    internal sealed class PlatformCallbacks : MonoBehaviour
    {
        [SerializeField] private UnityEvent onAndroid, onWindows;

        private void Awake()
        {
#if UNITY_STANDALONE_WIN
            onWindows.Invoke();
#else
            onAndroid.Invoke();
#endif
            Destroy(this);
        }
    }
}