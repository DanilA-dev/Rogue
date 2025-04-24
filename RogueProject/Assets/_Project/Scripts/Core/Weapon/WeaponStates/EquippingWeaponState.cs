namespace _Project.Scripts.Core.Weapon
{
    public class EquippingWeaponState : BaseWeaponState
    {
        public override float ExitTime { get; }
        public EquippingWeaponState(WeaponBehaviour weaponBehaviour) : base(weaponBehaviour)
        {
        }

    }
}