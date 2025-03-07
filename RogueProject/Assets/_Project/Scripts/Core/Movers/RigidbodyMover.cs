using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyMover : MonoBehaviour, IMover
    {
        #region Fields

        [Title("Movement")]
        [SerializeField] private float _movementMaxForce;
        [SerializeField] private float _movementAcceleration;
        [Space]
        [Title("Rotation")]
        [SerializeField] private bool _rotateTowardsMovementDirection;
        [SerializeField] private float _rotationSpeed;
        
        private Rigidbody _rigidbody;

        #endregion

        #region Properties

        [Title("Debug"),ShowInInspector, DisplayAsString]
        public float Speed { get; set; }

        public float MovementMaxForce
        {
            get => _movementMaxForce;
            set => _movementMaxForce = value;
        }

        public float RotationSpeed
        {
            get => _rotationSpeed;
            set => _rotationSpeed = value;
        }

        #endregion
        
        #region Monobehaviour

        private void Awake() => _rigidbody = GetComponent<Rigidbody>();

        #endregion

        #region Public

        public void Move(Vector3 movement)
        {
            if (movement != Vector3.zero)
            {
                if (_rotateTowardsMovementDirection)
                    Rotate(movement, Vector3.up);
            }
            
            Speed = Mathf.MoveTowards(Speed, movement == Vector3.zero 
                ? 0 
                : _movementMaxForce, _movementAcceleration * Time.deltaTime);
            _rigidbody.velocity = movement * Speed;
        }


        public void Rotate(Vector3 forwards, Vector3 upwards)
        {
            Quaternion targetRotation = Quaternion.LookRotation(forwards, upwards);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
        
        #endregion
    }
}