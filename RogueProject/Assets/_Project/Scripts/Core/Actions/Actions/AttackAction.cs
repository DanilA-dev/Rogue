using D_Dev.UtilScripts.DamagableSystem;
using UnityEngine;

namespace _Project.Scripts.Core.Combat
{
    [System.Serializable]
    public class AttackAction : ICombatAction
    {
        #region Fields

        [SerializeField] private int _actionPoints;
        [SerializeField] private int _cooldown;
        [SerializeField] private DamageInfo _damage;

        #endregion
        
        #region Properties

        public int ActionPoints => _actionPoints;
        public int Cooldown => _cooldown;

        #endregion

        #region Public

        public void DoAction(GameObject target)
        {
            if(target == null)
                return;
            
            if (target.TryGetComponent(out IDamagable damagable) && damagable.CanBeDamaged)
                damagable.Damage(_damage);
        }

        #endregion
        
        

    }
}