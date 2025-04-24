namespace _Project.Scripts.Core.Weapon
{
    public class CooldownWeaponState : BaseWeaponState
    {
        public override float ExitTime { get; }
        public CooldownWeaponState(WeaponBehaviour weaponBehaviour) : base(weaponBehaviour)
        {
        }

        public override void OnExit()
        {
            _weaponBehaviour.StopMovementOnAttack = false;
        }
    }
}