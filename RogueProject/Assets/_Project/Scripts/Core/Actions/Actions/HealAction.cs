using D_Dev.UtilScripts.ScriptableVaiables;
using D_Dev.UtilScripts.StatsSystem;
using D_Dev.UtilScripts.ValueSystem;
using UnityEngine;

namespace _Project.Scripts.Core.Combat
{
    [System.Serializable]
    public class HealAction : ICombatAction
    {
        #region Fields

        [SerializeField] private int _actionPoints;
        [SerializeField] private int _cooldown;
        [SerializeField] private IntValue _healAmount;
        [SerializeField] private StringScriptableVariable _healthStatVariable;

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