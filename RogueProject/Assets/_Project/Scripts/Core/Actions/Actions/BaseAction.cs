using D_Dev.UtilScripts.ValueSystem;
using UnityEngine;

namespace _Project.Scripts.Core.Combat
{
    [System.Serializable]
    public abstract class BaseAction : ICombatAction
    {
        #region Fields

        [SerializeField] protected int _actionPoints;
        [SerializeField] protected int _cooldown;

        #endregion

        #region Properties

        [field: SerializeField] public BoolValue IsReady { get; set; }
        [field: SerializeField] public IntValue CurrentCooldown { get; set; }
        public int ActionPoints => _actionPoints;
        public int CooldownAmount => _cooldown;

        #endregion

        #region Abstarct

        public virtual void DoAction(GameObject target)
        {
            if(!IsReady.Value)
                return;
            
            if(target == null)
                return;
        }


        #endregion
    }
}