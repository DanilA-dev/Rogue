using D_Dev.UtilScripts.DamagableSystem;
using UnityEngine;

namespace _Project.Scripts.Core.Combat
{
    [System.Serializable]
    public class AttackAction : BaseAction
    {
        #region Fields

        [SerializeField] private DamageInfo _damage;

        #endregion

        #region Public


        public override void DoAction(GameObject target)
        {
            base.DoAction(target);
            if (target.TryGetComponent(out IDamagable damagable) && damagable.CanBeDamaged)
            {
                damagable.Damage(_damage);
                IsReady.Value = false;
            }
        }

        #endregion
    }
}