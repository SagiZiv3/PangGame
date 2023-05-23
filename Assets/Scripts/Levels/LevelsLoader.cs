using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Pang.Levels
{
    internal sealed class LevelsLoader : MonoBehaviour
    {
        [SerializeField] private LevelInfo[] levels;
        [SerializeField] private BackgroundHandler backgroundHandler;
        [SerializeField] private BallsSpawner ballsSpawner;
        [SerializeField] private TimeHandler timeHandler;
        [SerializeField] private UnityEvent levelLoaded;
        private Coroutine ballsSpawnCoroutine;
        private bool isPaused;

        public int NumLevels => levels.Length;
        
        public void LoadLevel(int levelIndex)
        {
            if (levelIndex < 0 || levelIndex >= levels.Length)
                throw new ArgumentOutOfRangeException(nameof(levelIndex));

            LevelInfo level = levels[levelIndex];
            backgroundHandler.InitializeBackground(level.Background);
            timeHandler.Initialize(level.Duration);
            ballsSpawnCoroutine = StartCoroutine(SpawnBalls(level.Balls));
            levelLoaded.Invoke();
        }

        public void StopLoading()
        {
            if (ballsSpawnCoroutine != null)
                StopCoroutine(ballsSpawnCoroutine);
        }

        public void Pause() => isPaused = true;
        public void Resume() => isPaused = false;

        private IEnumerator SpawnBalls(BallInfo[] balls)
        {
            float startTime = Time.time;

            foreach (BallInfo ballInfo in balls)
            {
                while (isPaused) yield return null;
                float elapsedTime = Time.time - startTime;

                if (elapsedTime < ballInfo.AppearAfter)
                {
                    float remainingTime = ballInfo.AppearAfter - elapsedTime;
                    yield return new WaitForSeconds(remainingTime);
                }

                while (isPaused) yield return null;
                // Instantiate the ball prefab
                ballsSpawner.SpawnBall(ballInfo);
            }

            ballsSpawnCoroutine = null;
        }
    }
}