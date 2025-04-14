using D_Dev.UtilScripts.Extensions;
using UnityEngine;

namespace _Project.Scripts.Core.Player.States
{
    public class MovePlayerState : BasePlayerState
    {
        public override float ExitTime { get; }
        public MovePlayerState(PlayerControllerBehaviour playerController) : base(playerController)
        {
        }
        public override void OnUpdate()
        {
            _playerController.Mover.Move();
            var dir = _playerController.Mover.Target;
            var speed = _playerController.RotateMoveSpeed;
           _playerController.RotateTowards(dir, Vector3.up, speed);
        }
    }
}