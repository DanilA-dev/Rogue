using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Core.Player.States
{
    public class AimMovePlayerState : BasePlayerState
    {
        private List<Collider> _targets;
        
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
            RotateTowardsNearestTarget();
            _playerController.View.EvaluateAimLocomotionSpeed(_playerController.MovementVelocity, _playerController.Mover.MoveSpeed);
        }

        public override void OnExit()
        {
            _playerController.View.ToggleAimLocomotion(false);
        }

        private void RotateTowardsNearestTarget()
        {
            if(_targets == null)
                return;
            
            var minDistance = _targets.Min(t => Vector3.Distance(t.transform.position, _playerController.transform.position));
            foreach (var target in _targets)
            {
                if (Vector3.Distance(_playerController.transform.position, target.transform.position) <= minDistance)
                {
                    var dir = (target.transform.position - _playerController.transform.position).normalized;
                    var currentMoverVelocity = _playerController.Mover.Velocity;
                    var newVelocity = new Vector3(_playerController.transform.right.x + target.transform.right.x,
                        currentMoverVelocity.y, _playerController.transform.forward.z + target.transform.forward.z);
                    _playerController.Mover.Velocity = newVelocity;
                    var speed = _playerController.RotateAimSpeed;
                    _playerController.RotateTowards(dir, Vector3.up, speed, constrainX:true, constrainZ:true);
                }
            }
        }
    }
}