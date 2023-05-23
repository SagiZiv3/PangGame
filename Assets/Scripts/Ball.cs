using System;
using UnityEngine;

namespace Pang
{
    internal sealed class Ball : MonoBehaviour, IHittable
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private AnimationCurve sizeByLevel, speedByLevel;
        public int BallLevel { get; private set; }
        private Vector2 movementDirection;
        private float movementSpeed;
        private Action onExplode, onHitPlayer;

        public void Initialize(int level, Vector2 direction, Action onExplodeCallback, Action onHitPlayerCallback)
        {
            if (level > sizeByLevel.length)
            {
                Debug.LogError($"({name}): Undefined size for level {level}", this);
                level = sizeByLevel.length;
            }

            if (level > speedByLevel.length)
            {
                Debug.LogError($"({name}): Undefined speed for level {level}", this);
                level = speedByLevel.length;
            }

            transform.localScale = Vector2.one * sizeByLevel.Evaluate(level);
            movementSpeed = speedByLevel.Evaluate(level);
            movementDirection = direction.normalized;
            onExplode = onExplodeCallback;
            onHitPlayer = onHitPlayerCallback;
            BallLevel = level;
            gameObject.SetActive(true);
        }

        private void FixedUpdate()
        {
            rb.MovePosition(rb.position + movementDirection * (movementSpeed * Time.fixedDeltaTime));
        }

        public void HandleHit()
        {
            gameObject.SetActive(false);
            onExplode();
        }

        public void HandleHitPlayer()
        {
                gameObject.SetActive(false);
                onHitPlayer();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var contact = other.GetContact(0).normal;
            if (Mathf.Approximately(contact.x, 0f))
            {
                movementDirection.y = -movementDirection.y;
            }
            else
            {
                movementDirection.x = -movementDirection.x;
            }
            rb.MovePosition(rb.position + movementDirection * (movementSpeed * Time.fixedDeltaTime));
        }
    }
}