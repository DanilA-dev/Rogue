namespace _Project.Scripts.Core.Weapon
{
    public class ChargeStartWeaponState : BaseWeaponState
    {
        public override float ExitTime { get; }
        public ChargeStartWeaponState(WeaponBehaviour weaponBehaviour) : base(weaponBehaviour)
        {
        }

    }
}