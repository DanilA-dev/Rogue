using UnityEngine;

namespace _Project.Scripts.Core.Enemies.States
{
    public class ChasePlayerEnemyState : BaseEnemyState
    {
        #region Fields

        private IMover _mover;
        private Transform _target;

        #endregion

        #region Properties

        public override float ExitTime { get; }
       

        #endregion

        #region Constructors

        public ChasePlayerEnemyState(EnemyBehaviour enemyBehaviour) : base(enemyBehaviour)
        {
            _mover = _enemyBehaviour.EnemyMover;
        }

        #endregion
       
        #region Overrides

        public override void OnEnter()
        {
            _mover.MoveSpeed = _enemyBehaviour.ChaseMovementSpeed;
            _mover.StoppingDistance = _enemyBehaviour.StoppingDistance;
            if (_enemyBehaviour.Vision.IsTargetFound(out var c))
                _target = c.transform;
        }

        public override void OnUpdate()
        {
            if (_mover != null && _target != null)
            {
                _mover.Target = _target.position;
                _mover.Move();
                _enemyBehaviour.View.PlayChaseAnimation();
            }
        }

        #endregion
    }
}