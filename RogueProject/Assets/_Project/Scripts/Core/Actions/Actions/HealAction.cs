using D_Dev.UtilScripts.ScriptableVaiables;
using D_Dev.UtilScripts.StatsSystem;
using D_Dev.UtilScripts.ValueSystem;
using UnityEngine;

namespace _Project.Scripts.Core.Combat
{
    [System.Serializable]
    public class HealAction : BaseAction
    {
        #region Fields

        [SerializeField] private IntValue _healAmount;
        [SerializeField] private StringScriptableVariable _healthStatVariable;

        #endregion

        #region Public


        public override void DoAction(GameObject target)
        {
            base.DoAction(target);
            if(target.TryGetComponent(out StatsContainer statsContainer))
            {
                var healthStat = statsContainer.GetStat(_healthStatVariable);
                if(healthStat != null)
                    healthStat.StatValue.Value += _healAmount.Value;
            }
        }

        #endregion
    }
}