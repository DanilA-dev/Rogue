namespace _Project.Scripts.Core.Player.States
{
    public class DeadPlayerState : BasePlayerState
    {
        public override float ExitTime { get; }
        public DeadPlayerState(PlayerControllerBehaviour playerController) : base(playerController)
        {
        }

    }
}