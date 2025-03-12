namespace _Project.Scripts.Core.EquippableWeapon
{
    public class EquippingEquippableWeaponState : BaseEquippableWeaponState
    {
        public override float ExitTime { get; }
        public EquippingEquippableWeaponState(EquippableWeaponBehaviour weaponBehaviour) : base(weaponBehaviour)
        {
        }

    }
}