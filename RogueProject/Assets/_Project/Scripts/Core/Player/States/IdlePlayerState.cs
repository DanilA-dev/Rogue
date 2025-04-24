namespace _Project.Scripts.Core.Player.States
{
    public class IdlePlayerState : BasePlayerState
    {
        #region Properties

        public override float ExitTime { get; }


        #endregion

        #region Constructors
        public IdlePlayerState(PlayerControllerBehaviour playerController) : base(playerController) {}

        #endregion

        #region Overrides

        public override void OnEnter()
        {
            _playerController.View.PlayIdle();
        }

        #endregion
    }
}