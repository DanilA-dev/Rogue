namespace _Project.Scripts.Core.Weapon
{
    public class CooldownWeaponState : BaseWeaponState
    {
        #region Properties
        public override float ExitTime { get; }

        #endregion

        #region Constructors
        public CooldownWeaponState(WeaponBehaviour weaponBehaviour) : base(weaponBehaviour) {}

        #endregion

        #region Overrides

        public override void OnEnter()
        {
            _weaponBehaviour.StopMovementOnAttack = false;
        }

        #endregion
    }
}