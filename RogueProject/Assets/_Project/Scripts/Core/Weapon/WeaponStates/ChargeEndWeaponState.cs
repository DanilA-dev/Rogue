namespace _Project.Scripts.Core.Weapon
{
    public class ChargeEndWeaponState : BaseWeaponState
    {
        #region Properties
        public override float ExitTime { get; }

        #endregion

        #region Constructors
        public ChargeEndWeaponState(WeaponBehaviour weaponBehaviour) : base(weaponBehaviour) {}

        #endregion
    }
}