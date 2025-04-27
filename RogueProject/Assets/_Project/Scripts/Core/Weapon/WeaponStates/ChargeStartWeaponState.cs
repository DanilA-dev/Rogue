namespace _Project.Scripts.Core.Weapon
{
    public class ChargeStartWeaponState : BaseWeaponState
    {
        #region Properties
        public override float ExitTime { get; }

        #endregion

        #region Constructors
        public ChargeStartWeaponState(WeaponBehaviour weaponBehaviour) : base(weaponBehaviour) {}

        #endregion
    }
}