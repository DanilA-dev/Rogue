namespace _Project.Scripts.Core.Player.States
{
    public class DeadPlayerState : BasePlayerState
    {
        public override float ExitTime { get; }
        public DeadPlayerState(PlayerControllerBehaviour playerController) : base(playerController)
        {
        }

        public override void OnEnter()
        {
            _playerController.View.PlayDeathAnimation();
        }
    }
}