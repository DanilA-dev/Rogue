using UnityEngine;

namespace _Project.Scripts.Core.Enemies.States
{
    public class ChasePlayerEnemyState : BaseEnemyState
    {
        private IMover _mover;
        private Transform _target;
        public override float ExitTime { get; }
        
        public ChasePlayerEnemyState(EnemyBehaviour enemyBehaviour) : base(enemyBehaviour)
        {
            _mover = _enemyBehaviour.EnemyMover;
        }

        public override void OnEnter()
        {
            _target = _enemyBehaviour.Vision.Target.transform;
            _mover.Speed = _enemyBehaviour.ChaseMovementSpeed;
        }

        public override void OnUpdate()
        {
            _mover.Move(_target.position);
        }
    }
}