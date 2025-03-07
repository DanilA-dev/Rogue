using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshMover : MonoBehaviour, IMover
    {
        #region Properties

        public float Speed { get; set; }
        public float StoppingDistance { get; set; }
        public NavMeshAgent Agent {get; private set;}
        
        #endregion

        #region Monobehaviour

        private void Awake() => Agent = GetComponent<NavMeshAgent>();

        public void Move(Vector3 movement)
        {
            Agent.speed = Speed;
            Agent.stoppingDistance = StoppingDistance;
            Agent.destination = movement;
        }


        #endregion
    }
}