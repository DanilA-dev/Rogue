using System;
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
        
        private Rigidbody _rigidbody;
        public event Action<Vector3> OnMove;

        #endregion

        #region Properties

        [Title("Debug"),ShowInInspector, DisplayAsString]
        public float MoveSpeed { get; set; }
        public float StoppingDistance { get; set; }
        public float MovementMaxForce
        {
            get => _movementMaxForce;
            set => _movementMaxForce = value;
        }

        #endregion
        
        #region Monobehaviour

        private void Awake() => _rigidbody = GetComponent<Rigidbody>();

        #endregion

        #region Public

        public Vector3 Target { get; set; }

        public void Move()
        {
            MoveSpeed = Mathf.MoveTowards(MoveSpeed, Target == Vector3.zero 
                ? 0 
                : _movementMaxForce, _movementAcceleration * Time.deltaTime);
            _rigidbody.velocity = Target * MoveSpeed;
            OnMove?.Invoke(Target * MoveSpeed);
        }

        #endregion
    }
}