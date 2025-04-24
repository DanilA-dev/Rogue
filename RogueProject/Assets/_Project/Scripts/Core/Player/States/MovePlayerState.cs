using UnityEngine;

namespace _Project.Scripts.Core.Player.States
{
    public class MovePlayerState : BasePlayerState
    {
        #region Properties
        public override float ExitTime { get; }

        #endregion

        #region Constructors

        public MovePlayerState(PlayerControllerBehaviour playerController) : base(playerController)
        {
        }

        #endregion

        #region Overrides

        public override void OnEnter()
        {
            _playerController.View.PlayRun();
        }

        public override void OnUpdate()
        {
            if(_playerController.IsCurrentWeaponAttackStopMove())
                return;
            
            _playerController.Mover.Move();
            _playerController.View.PlayRun();
            var dir = _playerController.Mover.Target;
            var speed = _playerController.RotateMoveSpeed;
            _playerController.RotateTowards(dir, Vector3.up, speed);
        }

        #endregion
    }
}