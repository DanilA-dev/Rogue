using D_Dev.Scripts.Runtime.UtilScripts.SimpleStateMachine;

namespace _Project.Scripts.Core.EquippableWeapon
{
    public abstract class BaseEquippableWeaponState : IState
    {
        protected EquippableWeaponBehaviour _weaponBehaviour;

        public BaseEquippableWeaponState(EquippableWeaponBehaviour weaponBehaviour)
        {
            _weaponBehaviour = weaponBehaviour;
        }

        public abstract float ExitTime { get; }

        public virtual void OnEnter() {}
        public virtual void OnUpdate() {}
        public virtual void OnFixedUpdate() {}
        public virtual void OnExit() {}
        
    }
}