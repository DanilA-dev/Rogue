namespace _Project.Scripts.Core.EquippableWeapon
{
    public class AttackEquippableWeaponState : BaseEquippableWeaponState
    {
        public override float ExitTime { get; }
        public AttackEquippableWeaponState(EquippableWeaponBehaviour weaponBehaviour) : base(weaponBehaviour)
        {
        }

        public override void OnEnter()
        {
            _weaponBehaviour.PlayAnimation(EquippableWeaponState.Attack);
        }
    }
}