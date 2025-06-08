namespace _Project.Scripts.Core.Player.States
{
    public class CombatPlayerState :BasePlayerState
    {
        public override float ExitTime { get; }
        public CombatPlayerState(PlayerControllerBehaviour playerController) : base(playerController)
        {
        }

    }
}