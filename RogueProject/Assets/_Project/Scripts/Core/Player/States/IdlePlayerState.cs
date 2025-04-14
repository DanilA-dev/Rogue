using UnityEngine;

namespace _Project.Scripts.Core.Player.States
{
    public class IdlePlayerState : BasePlayerState
    {
        public override float ExitTime { get; }
        
        public IdlePlayerState(PlayerControllerBehaviour playerController) : base(playerController)
        {
        }
    }
}