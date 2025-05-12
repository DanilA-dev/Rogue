using UnityEngine;

namespace _Project.Scripts.Core.Weapon
{
    public class AttackActionWeaponState : BaseWeaponState
    {
        #region Properties
        public override float ExitTime { get; }

        #endregion
        
        #region Constructors
        
        public AttackActionWeaponState(WeaponBehaviour weaponBehaviour) : base(weaponBehaviour) {}

        #endregion

        #region Overrides

        public override void OnEnter()
        {
            _weaponBehaviour.StaminaVariable.Value -= _weaponBehaviour.LastAttackConfig.StaminaCost;
            var attackConfig = _weaponBehaviour.LastAttackConfig;
            var mainRigidbody = _weaponBehaviour.MainRigidBody;
            
            if(attackConfig == null 
               || !attackConfig.ApplyForceOnAttack 
               || mainRigidbody == null)
                return;
            
            var localForce = attackConfig.AttackForce;
            var dir = mainRigidbody.transform.TransformDirection(localForce);
            
            mainRigidbody.AddForce(dir, attackConfig.ForceMode);
                                  
        }

        public override void OnFixedUpdate()
        {
           
        }

        #endregion
    }
}