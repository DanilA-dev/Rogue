namespace _Project.Scripts.Core.Weapon
{
    public class AttackStartWeaponState : BaseWeaponState
    {
        #region Properties
        public override float ExitTime { get; }

        #endregion

        #region Constructors

        public AttackStartWeaponState(WeaponBehaviour weaponBehaviour) : base(weaponBehaviour) {}

        #endregion

        #region Overrides

        public override void OnEnter()
        {
            _weaponBehaviour.PlayAttackConfigAnimation();
            _weaponBehaviour.StopMovementOnAttack = _weaponBehaviour.LastAttackConfig.StopMovementWhileAttacking;
        }

        #endregion
    }
}