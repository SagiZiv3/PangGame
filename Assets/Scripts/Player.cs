using EditorTools.SerializedReferenceInitializer.Attributes;
using Pang.InputHandlers;
using Pang.NumericVariables;
using Pang.Weapons;
using UnityEngine;

namespace Pang
{
    internal sealed class Player : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private Rigidbody2D rigidbody2D;
        [SerializeField] private Weapon weapon;
        [SerializeField] private IntVariable healthVariable;

        [SerializeReference, ShowInitializationMenu]
        private IInitializable[] initializables;

        private InputHandler inputHandler;
        private float movement;

        public void SetInputHandler(InputHandler inputHandler)
        {
            this.inputHandler = inputHandler;
            inputHandler.OnFire += Fire;
        }

        private void Start()
        {
            foreach (var initializable in initializables)
            {
                initializable.Initialize();
            }
        }

        private void OnDisable()
        {
            inputHandler.OnFire -= Fire;
        }

        private void Update()
        {
            movement = inputHandler.GetMovement();
        }

        private void Fire()
        {
            weapon.Fire(transform.position);
        }

        private void FixedUpdate()
        {
            rigidbody2D.MovePosition(rigidbody2D.position +
                                     new Vector2(movement * movementSpeed * Time.fixedDeltaTime, 0f));
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                healthVariable.Reduce(1);
                other.gameObject.GetComponent<Ball>().HandleHitPlayer();
            }
        }
    }
}