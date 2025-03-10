using D_Dev.UtilScripts.Extensions;
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
        [SerializeField] private float _rotationSpeed;
        
        private Rigidbody _rigidbody;

        #endregion

        #region Properties

        [Title("Debug"),ShowInInspector, DisplayAsString]
        public float MoveSpeed { get; set; }
        public float StoppingDistance { get; set; }
        public bool EnableDirectionRotation { get; set; } = true;
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
                if (EnableDirectionRotation)
                    Rotate(movement, Vector3.up);
            }
            
            MoveSpeed = Mathf.MoveTowards(MoveSpeed, movement == Vector3.zero 
                ? 0 
                : _movementMaxForce, _movementAcceleration * Time.deltaTime);
            _rigidbody.velocity = movement * MoveSpeed;
        }


        public void Rotate(Vector3 forwards, Vector3 upwards)
        {
           transform.RotateTowards(forwards, upwards, _rotationSpeed * Time.deltaTime);
        }
        
        #endregion
    }
}