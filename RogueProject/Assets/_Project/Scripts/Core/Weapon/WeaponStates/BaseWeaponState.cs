using D_Dev.Scripts.Runtime.UtilScripts.SimpleStateMachine;

namespace _Project.Scripts.Core.Weapon
{
    public abstract class BaseWeaponState : IState
    {
        protected WeaponBehaviour _weaponBehaviour;

        public BaseWeaponState(WeaponBehaviour weaponBehaviour)
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