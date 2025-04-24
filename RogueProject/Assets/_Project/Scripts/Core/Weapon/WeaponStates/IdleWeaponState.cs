namespace _Project.Scripts.Core.Weapon
{
    public class IdleWeaponState : BaseWeaponState
    {
        public override float ExitTime { get; }
        public IdleWeaponState(WeaponBehaviour weaponBehaviour) : base(weaponBehaviour)
        {
        }
    }
}