namespace _Project.Scripts.Core.EquippableWeapon
{
    public class IdleEquippableWeaponState : BaseEquippableWeaponState
    {
        public override float ExitTime { get; }
        public IdleEquippableWeaponState(EquippableWeaponBehaviour weaponBehaviour) : base(weaponBehaviour)
        {
        }
    }
}