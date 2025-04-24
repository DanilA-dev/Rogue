namespace _Project.Scripts.Core.Weapon
{
    public class AttackActionWeaponState : BaseWeaponState
    {
        public override float ExitTime { get; }
        public AttackActionWeaponState(WeaponBehaviour weaponBehaviour) : base(weaponBehaviour)
        {
        }
       
    }
}