using UnityEngine;

namespace _Project.Scripts.Core.Enemies.States
{
    public class PatrolEnemyState : BaseEnemyState
    {
        private IMover _mover;
        private Transform[] _path;
        private Vector3 _targetPosition;
        public override float ExitTime { get; }
        public PatrolEnemyState(EnemyBehaviour enemyBehaviour) : base(enemyBehaviour)
        {
            _path = _enemyBehaviour.PatrolPoints;
            _mover = _enemyBehaviour.EnemyMover;
        }

        public override void OnEnter()
        {
            _targetPosition = _path[_enemyBehaviour.PatrolPointIndex].position;
            _mover.Speed = _enemyBehaviour.PatrolMovementSpeed;
        }

        public override void OnUpdate()
        {
            if (IsTargetReached(_targetPosition, _enemyBehaviour.StoppingDistance))
                ChangePathIndex();
            
            _mover.Move(_targetPosition);
        }

        private void ChangePathIndex()
        {
            _enemyBehaviour.PatrolPointIndex++;
            if(_enemyBehaviour.PatrolPointIndex >= _path.Length)
                _enemyBehaviour.PatrolPointIndex = 0;
        }
    }
}