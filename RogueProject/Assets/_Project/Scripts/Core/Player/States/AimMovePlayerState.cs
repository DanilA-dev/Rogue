using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Core.Player.States
{
    public class AimMovePlayerState : BasePlayerState
    {
        private List<Collider> _targets;
        private Vector3 _targetDir;
        
        public override float ExitTime { get; }
        public AimMovePlayerState(PlayerControllerBehaviour playerController) : base(playerController)
        {
        }

        public override void OnEnter()
        {
            _targets = _playerController.Sensor.Trigger.Colliders;
            _playerController.View.ToggleAimLocomotion(true);
        }

        public override void OnUpdate()
        {
            if(_playerController.IsCurrentWeaponAttackStopMove())
                return;
            
            _playerController.Mover.Move();
            RotateTowardsNearestTarget();
            _playerController.View.EvaluateAimLocomotionSpeed(_playerController.Mover.Target);
        }

        public override void OnExit()
        {
            _playerController.View.ToggleAimLocomotion(false);
            _playerController.TargetVariable.Value.SetActive(false);
        }

        private void RotateTowardsNearestTarget()
        {
            if(_targets.Count <= 0 || _targets == null)
                return ;
            
            var minDistance = _targets.Min(t => Vector3.Distance(t.transform.position, _playerController.transform.position));
            foreach (var target in _targets)
            {
                if (Vector3.Distance(_playerController.transform.position, target.transform.position) <= minDistance)
                {
                    var dir = (target.transform.position - _playerController.transform.position).normalized;
                    var speed = _playerController.RotateAimSpeed;
                    var targetObjectPos = new Vector3(target.transform.position.x + _playerController.TargetObjectPositionOffset.x,
                        target.transform.position.y + _playerController.TargetObjectPositionOffset.y,
                        target.transform.position.z + _playerController.TargetObjectPositionOffset.z);
                    _playerController.TargetVariable.Value.SetActive(true);
                    _playerController.TargetVariable.Value.transform.position = targetObjectPos;
                    _playerController.RotateTowards(dir, Vector3.up, speed, constrainX:true, constrainZ:true);
                }
            }
        }
    }
}