using UnityEngine;

namespace _Project.Scripts.Core.Combat
{
    public interface ICombatAction
    {
        public int ActionPoints { get; }
        public int Cooldown { get; }
        
        public void DoAction(GameObject target);
    }
}