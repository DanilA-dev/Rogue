using D_Dev.Scripts.Runtime.UtilScripts.SimpleStateMachine;
using UnityEngine;

namespace _Project.Scripts.Core.Enemies.States
{
    public abstract class BaseEnemyState : IState
    {
        protected EnemyBehaviour _enemyBehaviour;

        public abstract float ExitTime { get; }
        protected BaseEnemyState(EnemyBehaviour enemyBehaviour)
        {
            _enemyBehaviour = enemyBehaviour;
        }

        protected bool IsTargetReached(Vector3 targetPosition, float distance)
        {
            return Vector3.Distance(_enemyBehaviour.transform.position, targetPosition) < distance;
        }
        
        public virtual void OnEnter() {}
        public virtual void OnUpdate() {}
        public virtual void OnFixedUpdate() {}
        public virtual void OnExit() {}
        
    }
}