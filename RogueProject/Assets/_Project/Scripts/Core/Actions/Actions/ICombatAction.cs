using D_Dev.UtilScripts.ValueSystem;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Core.Combat
{
    public interface ICombatAction
    {
        public int ActionPoints { get; }
        public int CooldownAmount { get; }
        public IntValue CurrentCooldown { get; set; }
        public BoolValue IsReady { get; }
        
        public void DoAction(GameObject target);
    }
}