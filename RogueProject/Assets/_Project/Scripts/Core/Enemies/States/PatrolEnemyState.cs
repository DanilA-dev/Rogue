using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Core.Enemies.States
{
    public class PatrolEnemyState : BaseEnemyState
    {
        #region Fields

        private IMover _mover;
        private Vector3[] _path;
        private Vector3 _targetPosition;

        #endregion

        #region Overrides

        public override float ExitTime => 0.5f;
        public PatrolEnemyState(EnemyBehaviour enemyBehaviour) : base(enemyBehaviour)
        {
            _path = _enemyBehaviour.PatrolPoints.Select(p => p.position).ToArray();
            _mover = _enemyBehaviour.EnemyMover;
        }

        public override void OnEnter()
        {
            _targetPosition = _path[_enemyBehaviour.PatrolPointIndex];
            _mover.Speed = _enemyBehaviour.PatrolMovementSpeed;
            _mover.StoppingDistance = _enemyBehaviour.StoppingDistance;
        }

        public override void OnUpdate()
        {
            if (_enemyBehaviour.IsTargetReached(_targetPosition, _enemyBehaviour.StoppingDistance))
                ChangePathIndex();
            
            _mover.Move(_targetPosition);
        }

        #endregion

        #region Private

        private void ChangePathIndex()
        {
            _enemyBehaviour.PatrolPointIndex++;
            if(_enemyBehaviour.PatrolPointIndex >= _path.Length)
                _enemyBehaviour.PatrolPointIndex = 0;
        }

        #endregion
    }
}