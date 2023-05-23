using Pang.NumericVariables;
using UnityEngine;
using UnityEngine.Events;

namespace Pang
{
    internal sealed class GameManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent onPlayerOutOfHealth, onPlayerOutOfTime, onPlayerWon;
        [SerializeField] private UnityEvent onGameOver;
        [SerializeField] private IntVariable healthVariable, gameTimerVariable;
        [SerializeField] private IntVariable ballsCounterVariable;

        private void Awake()
        {
            onPlayerOutOfHealth.AddListener(onGameOver.Invoke);
            onPlayerOutOfTime.AddListener(onGameOver.Invoke);
            onPlayerWon.AddListener(onGameOver.Invoke);
        }

        public void OnLevelLoaded()
        {
            healthVariable.OnValueChanged += HealthVariableValueChanged;
            gameTimerVariable.OnValueChanged += TimerVariableValueChanged;
            ballsCounterVariable.OnValueChanged += BallsCounterVariableValueChanged;
        }

        private void OnDisable()
        {
            healthVariable.OnValueChanged -= HealthVariableValueChanged;
            gameTimerVariable.OnValueChanged -= TimerVariableValueChanged;
            ballsCounterVariable.OnValueChanged -= BallsCounterVariableValueChanged;
        }

        private void HealthVariableValueChanged(int currentHealth)
        {
            if (currentHealth != 0) return;
            enabled = false;
            onPlayerOutOfHealth.Invoke();
        }

        private void TimerVariableValueChanged(int timeRemaining)
        {
            if (timeRemaining != 0) return;
            enabled = false;
            onPlayerOutOfTime.Invoke();
        }

        private void BallsCounterVariableValueChanged(int remainingBalls)
        {
            if (remainingBalls != 0 || healthVariable.Value == 0 || gameTimerVariable.Value == 0) return;
            enabled = false;
            onPlayerWon.Invoke();
        }
    }
}