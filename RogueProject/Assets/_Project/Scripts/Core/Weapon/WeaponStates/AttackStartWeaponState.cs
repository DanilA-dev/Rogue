namespace _Project.Scripts.Core.Weapon
{
    public class AttackStartWeaponState : BaseWeaponState
    {
        public override float ExitTime { get; }
        public AttackStartWeaponState(WeaponBehaviour weaponBehaviour) : base(weaponBehaviour)
        {
        }

        public override void OnEnter()
        {
            _weaponBehaviour.PlayAttackConfigAnimation();
            _weaponBehaviour.StopMovementOnAttack = _weaponBehaviour.LastAttackConfig.StopMovementWhileAttacking;
        }
    }
}