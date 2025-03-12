namespace _Project.Scripts.Core.EquippableWeapon
{
    public class ChargeEndEquippableWeaponState : BaseEquippableWeaponState
    {
        public override float ExitTime { get; }
        public ChargeEndEquippableWeaponState(EquippableWeaponBehaviour weaponBehaviour) : base(weaponBehaviour)
        {
        }

    }
}