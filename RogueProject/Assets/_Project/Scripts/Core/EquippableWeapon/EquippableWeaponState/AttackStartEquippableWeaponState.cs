namespace _Project.Scripts.Core.EquippableWeapon
{
    public class AttackStartEquippableWeaponState : BaseEquippableWeaponState
    {
        public override float ExitTime { get; }
        public AttackStartEquippableWeaponState(EquippableWeaponBehaviour weaponBehaviour) : base(weaponBehaviour)
        {
        }

        public override void OnEnter()
        {
            _weaponBehaviour.PlayEquippableWeaponAnimation(EquippableWeaponState.AttackStart);
        }
    }
}