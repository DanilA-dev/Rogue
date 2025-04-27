namespace _Project.Scripts.Core.Weapon
{
    public class IdleWeaponState : BaseWeaponState
    {
        #region Properties
        public override float ExitTime { get; }

        #endregion

        #region Constructors

        public IdleWeaponState(WeaponBehaviour weaponBehaviour) : base(weaponBehaviour) {}


        #endregion
    }
}