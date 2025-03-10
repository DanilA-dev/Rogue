using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshMover : MonoBehaviour, IMover
    {
        #region Fields

        [SerializeField] private float _defaultMoveSpeed;
        [SerializeField] private float _defaultRotationSpeed;
        [SerializeField] private float _defautStoppingDistance;

        #endregion
        
        #region Properties

        public float MoveSpeed { get => _defaultMoveSpeed; set => _defaultMoveSpeed = value; }
        public float RotationSpeed { get => _defaultRotationSpeed; set => _defaultRotationSpeed = value; }
        public float StoppingDistance { get => _defautStoppingDistance; set => _defautStoppingDistance = value; }
        public bool EnableDirectionRotation { get; set; } = true;
        public NavMeshAgent Agent {get; private set;}
        
        #endregion

        #region Monobehaviour

        private void Awake() => Agent = GetComponent<NavMeshAgent>();

        public void Move(Vector3 movement)
        {
            Agent.speed = MoveSpeed;
            Agent.stoppingDistance = StoppingDistance;
            Agent.updateRotation = EnableDirectionRotation;
            Agent.angularSpeed = RotationSpeed;
            Agent.destination = movement;
        }


        #endregion
    }
}