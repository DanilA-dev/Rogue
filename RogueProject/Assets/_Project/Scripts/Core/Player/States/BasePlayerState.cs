using D_Dev.Scripts.Runtime.UtilScripts.SimpleStateMachine;

namespace _Project.Scripts.Core.Player.States
{
    public abstract class BasePlayerState : IState
    {
        protected PlayerControllerBehaviour _playerController;
        protected BasePlayerState(PlayerControllerBehaviour playerController)
        {
            _playerController = playerController;
        }

        public abstract float ExitTime { get; }
        public virtual void OnEnter() {}
        public virtual void OnUpdate() {}
        public virtual void OnFixedUpdate() {}
        public virtual void OnExit() {}
    }
}