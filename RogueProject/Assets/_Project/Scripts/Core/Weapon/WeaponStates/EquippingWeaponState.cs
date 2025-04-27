namespace _Project.Scripts.Core.Weapon
{
    public class EquippingWeaponState : BaseWeaponState
    {
        #region Properties
        public override float ExitTime { get; }

        #endregion

        #region Constructors

        public EquippingWeaponState(WeaponBehaviour weaponBehaviour) : base(weaponBehaviour) {}

        #endregion
    }
}