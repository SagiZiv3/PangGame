using System.Collections.Generic;
using Pang.NumericVariables;
using Pang.Pools;
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

        public void SpawnBall(BallInfo ballInfo)
        {
            Ball ball = GetOrCreateBall(ballInfo.Prefab);
            var (position, direction) = GetRandomPositionAndDirection();
            ball.transform.position = position;
            ball.Initialize(ballInfo.Level, direction,
                () => OnBallExplode(ball), () => OnBallHitPlayer(ball));
        }

        private Ball GetOrCreateBall(Ball ballPrefab)
        {
            // If the pool for this prefab doesn't exist, create it.
            if (!poolsByPrefab.ContainsKey(ballPrefab.GetHashCode()))
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
            // Find the id of the prefab that this ball was created from.
            int prefabId = prefabIdByInstanceId[ball.GetHashCode()];
            // Get the pool for this prefab (Can't be null).
            var objectsPool = poolsByPrefab[prefabId];
            objectsPool.Return(ball);
            ballsCounterVariable.Reduce(1);
        }

        private (Vector2 position, Vector2 direction) GetRandomPositionAndDirection()
        {
            // Get a random position inside the screen limits (only top-half of the screen, so we don't immediately hit the user)
            Vector2 position = new Vector2(
                Random.Range(screenBoundsHandler.ScreenBounds.xMin + spawnPadding,
                    screenBoundsHandler.ScreenBounds.xMax - spawnPadding),
                Random.Range(screenBoundsHandler.ScreenBounds.center.y,
                    screenBoundsHandler.ScreenBounds.yMax - spawnPadding)
            );
            Vector2 direction = Vector2.down;
            if (Random.Range(0f, 1f) > 0.5f)
                direction += Vector2.left;
            else
                direction += Vector2.right;
            return (position, direction);
        }

        private void OnBallExplode(Ball ball)
        {
            // Because we don't want to trigger the value changed event unnecessarily, we use the `BeginModification` method.
            ballsCounterVariable.BeginModification();
            int prefabId = prefabIdByInstanceId[ball.GetHashCode()];
            DisposeBall(ball);

            // If this ball is at the lowest level, we can't split it and therefore exit early.
            if (ball.BallLevel == 1)
            {
                ballsCounterVariable.EndModification();
                return;
            }

            int ballLevel = ball.BallLevel - 1;
            var position = ball.transform.position;
            // Instantiate the left ball
            CreateBallFromExplosion(prefabId, position, ballLevel, Vector2.left);
            // Instantiate the right ball
            CreateBallFromExplosion(prefabId, position, ballLevel, Vector2.right);
        }

        private void CreateBallFromExplosion(int prefabId, Vector3 position, int ballLevel, Vector2 direction)
        {
            Ball ball = GetOrCreateBall(prefabId);
            ball.transform.position = position;
            ball.Initialize(ballLevel, direction + Vector2.up / 2f,
                () => OnBallExplode(ball), () => OnBallHitPlayer(ball));
            prefabIdByInstanceId[ball.GetHashCode()] = prefabId;
        }

        private void OnBallHitPlayer(Ball ball)
        {
            DisposeBall(ball);
        }
    }
}