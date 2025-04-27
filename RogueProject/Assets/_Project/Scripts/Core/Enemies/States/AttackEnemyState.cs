using D_Dev.UtilScripts.Extensions;
using UnityEngine;

namespace _Project.Scripts.Core.Enemies.States
{
    public class AttackEnemyState : BaseEnemyState
    {
        private Transform _transform;

        public AttackEnemyState(EnemyBehaviour enemyBehaviour) : base(enemyBehaviour)
        {
            _transform = _enemyBehaviour.transform;
        }

        public override float ExitTime { get; }

        public override void OnEnter()
        {
            _enemyBehaviour.EnemyMover.MoveSpeed = 0;
        }

        public override void OnUpdate()
        {
            if (_enemyBehaviour.Vision.IsTargetFound(out var target))
            {
                // _transform.RotateTowardsDirection(target.transform.position, Vector3.up,
                //     _enemyBehaviour.RotationSpeed * Time.deltaTime);
                
                _enemyBehaviour.WeaponBehaviour.Use();
            }
                
        }
    }
}