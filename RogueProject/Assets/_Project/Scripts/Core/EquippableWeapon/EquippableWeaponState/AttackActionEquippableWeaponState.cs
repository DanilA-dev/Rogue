namespace _Project.Scripts.Core.EquippableWeapon
{
    public class AttackActionEquippableWeaponState : BaseEquippableWeaponState
    {
        public override float ExitTime { get; }
        public AttackActionEquippableWeaponState(EquippableWeaponBehaviour weaponBehaviour) : base(weaponBehaviour)
        {
        }
    }
}