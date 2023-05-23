using System.Collections.Generic;
using Pang.NumericVariables;
using UnityEngine;

namespace Pang.Levels
{
    internal sealed class BallsSpawner : MonoBehaviour
    {
        [SerializeField] private ScreenBoundsHandler screenBoundsHandler;
        [SerializeField] private IntVariable ballsCounterVariable;
        [SerializeField] private float spawnPadding = 50f;
        private IDictionary<int, ObjectsPool<Ball>> poolsByPrefab;
        private IDictionary<int, int> prefabIdByInstanceId;
        private ISet<Ball> spawnedBalls;

        private void Awake()
        {
            poolsByPrefab = new Dictionary<int, ObjectsPool<Ball>>();
            prefabIdByInstanceId = new Dictionary<int, int>();
            spawnedBalls = new HashSet<Ball>();
            ballsCounterVariable.Initialize(0);
        }

        public void SpawnBall(BallInfo ballInfo)
        {
            Ball ball = GetOrCreateBall(ballInfo.Prefab);
            Vector2 position = new Vector2(
                Random.Range(screenBoundsHandler.ScreenBounds.xMin + spawnPadding,
                    screenBoundsHandler.ScreenBounds.xMax - spawnPadding),
                Random.Range(screenBoundsHandler.ScreenBounds.center.y,
                    screenBoundsHandler.ScreenBounds.yMax - spawnPadding)
            );
            ball.transform.position = position;
            ball.Initialize(ballInfo.Level, Vector2.right + Vector2.down,
                () => OnBallExplode(ball), () => OnBallHitPlayer(ball));
        }

        public void DisposeAllBalls()
        {
            foreach (Ball spawnedBall in spawnedBalls)
            {
                spawnedBall.enabled = false;
                DisposeBall(spawnedBall);
            }
        }
        public void Pause()
        {
            foreach (Ball spawnedBall in spawnedBalls)
            {
                spawnedBall.enabled = false;
            }
        }

        public void Resume()
        {
            foreach (Ball spawnedBall in spawnedBalls)
            {
                spawnedBall.enabled = true;
            }
        }

        private Ball GetOrCreateBall(Ball ballPrefab)
        {
            if (!poolsByPrefab.TryGetValue(ballPrefab.GetHashCode(), out _))
            {
                poolsByPrefab[ballPrefab.GetHashCode()] = new ObjectsPool<Ball>(ballPrefab);
            }

            return GetOrCreateBall(ballPrefab.GetHashCode());
        }

        private Ball GetOrCreateBall(int prefabId)
        {
            Ball ball = poolsByPrefab[prefabId].GetOrCreate();
            prefabIdByInstanceId[ball.GetHashCode()] = prefabId;
            ballsCounterVariable.Add(1);
            spawnedBalls.Add(ball);
            return ball;
        }

        private void DisposeBall(Ball ball)
        {
            int prefabId = prefabIdByInstanceId[ball.GetHashCode()];
            var objectsPool = poolsByPrefab[prefabId];
            objectsPool.Return(ball);
            ballsCounterVariable.Reduce(1);
        }

        private void OnBallExplode(Ball ball)
        {
            ballsCounterVariable.BeginModification();
            int prefabId = prefabIdByInstanceId[ball.GetHashCode()];
            var objectsPool = poolsByPrefab[prefabId];
            DisposeBall(ball);

            if (ball.BallLevel == 1)
            {
                ballsCounterVariable.EndModification();
                return;
            }

            // Show explosion stuff
            int ballLevel = ball.BallLevel - 1;
            var position = ball.transform.position;
            Ball b1 = GetOrCreateBall(prefabId);
            b1.transform.position = position;
            b1.Initialize(ballLevel, Vector2.left + Vector2.up / 2f,
                () => OnBallExplode(b1), () => OnBallHitPlayer(b1));
            prefabIdByInstanceId[b1.GetHashCode()] = prefabId;
            Ball b2 = GetOrCreateBall(prefabId);
            b2.transform.position = position;
            b2.Initialize(ballLevel, Vector2.right + Vector2.up / 2f,
                () => OnBallExplode(b2), () => OnBallHitPlayer(b2));
            prefabIdByInstanceId[b2.GetHashCode()] = prefabId;
            ballsCounterVariable.EndModification();
        }

        private void OnBallHitPlayer(Ball ball)
        {
            DisposeBall(ball);
        }
    }
}