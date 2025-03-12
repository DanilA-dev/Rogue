namespace _Project.Scripts.Core.EquippableWeapon
{
    public class ChargeStartEquippableWeaponState : BaseEquippableWeaponState
    {
        public override float ExitTime { get; }
        public ChargeStartEquippableWeaponState(EquippableWeaponBehaviour weaponBehaviour) : base(weaponBehaviour)
        {
        }

    }
}