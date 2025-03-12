namespace _Project.Scripts.Core.EquippableWeapon
{
    public class CooldownEquippableWeaponState : BaseEquippableWeaponState
    {
        public override float ExitTime { get; }
        public CooldownEquippableWeaponState(EquippableWeaponBehaviour weaponBehaviour) : base(weaponBehaviour)
        {
        }

    }
}