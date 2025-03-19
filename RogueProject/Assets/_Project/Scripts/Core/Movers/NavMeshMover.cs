using System;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshMover : MonoBehaviour, IMover
    {
        #region Fields

        [SerializeField] private float _defaultMoveSpeed;
        [SerializeField] private float _defautStoppingDistance;

        public Vector3 Velocity { get; set; }
        public event Action<Vector3> OnMove;

        
        #endregion
        
        #region Properties

        public float MoveSpeed { get => _defaultMoveSpeed; set => _defaultMoveSpeed = value; }
        public float StoppingDistance { get => _defautStoppingDistance; set => _defautStoppingDistance = value; }
        public NavMeshAgent Agent {get; private set;}
        
        #endregion

        #region Monobehaviour

        private void Awake() => Agent = GetComponent<NavMeshAgent>();

        #endregion

        #region Public

        public void Move()
        {
            Agent.speed = MoveSpeed;
            Agent.stoppingDistance = StoppingDistance;
            Agent.destination = Velocity;
            OnMove?.Invoke(Velocity);
        }

        #endregion
    }
}