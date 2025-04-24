namespace _Project.Scripts.Core.Weapon
{
    public class ChargeEndWeaponState : BaseWeaponState
    {
        public override float ExitTime { get; }
        public ChargeEndWeaponState(WeaponBehaviour weaponBehaviour) : base(weaponBehaviour)
        {
        }

    }
}