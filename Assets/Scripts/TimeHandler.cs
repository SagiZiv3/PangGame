using System.Collections;
using Pang.NumericVariables;
using UnityEngine;

namespace Pang
{
    internal sealed class TimeHandler : MonoBehaviour
    {
        [SerializeField] private IntVariable timerVariable;
        private Coroutine countdownCoroutine;
        private bool isPaused;

        public void Initialize(int duration)
        {
            timerVariable.Initialize(duration);
            countdownCoroutine = StartCoroutine(Countdown());
        }

        public void Stop()
        {
            StopCoroutine(countdownCoroutine);
        }

        public void Pause() => isPaused = true;
        public void Resume() => isPaused = false;

        private IEnumerator Countdown()
        {
            WaitForSeconds waitOneSecond = new WaitForSeconds(1f);
            while (timerVariable.Value > 0)
            {
                while (isPaused) yield return null;
                timerVariable.Reduce(1);
                yield return waitOneSecond;
            }
        }
    }
}