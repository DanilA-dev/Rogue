using _Project.Scripts.Core.Combat.ActionsInfo;
using _Project.Scripts.Core.Weapon;
using D_Dev.UtilScripts.Extensions;
using UnityEngine;

namespace _Project.Scripts.Core.Characters
{
    public class ActionCharacter : MonoBehaviour
    {
        #region Fields
        
        [SerializeField] private ActionsContainer _combatContainer;
        
        private WeaponHolder _possibleWeaponHolder;
        
        #endregion
        
        #region Properties
        
        public ActionsContainer CombatContainer => _combatContainer;
        
        #endregion

        #region Public

        public void UpdateAllActions()
        {
            TryAddActionsFromWeapon();
        }

        #endregion
        
        #region Private

        private bool TryAddActionsFromWeapon()
        {
            if (gameObject.TryGetComponentInAny(out _possibleWeaponHolder))
            {
                var weaponInfo = _possibleWeaponHolder.CurrentWeaponInfo;
                var weaponActions = weaponInfo.WeaponData.WeaponActions;
                if (weaponInfo == null ||weaponActions.Length == 0)
                    return false;
                
                foreach (var weaponAction in weaponActions)
                {
                    if(weaponAction == null)
                        continue;
                    
                    _combatContainer.AddAction(weaponAction);
                }
                
                return true;
            }
            return false;
        }

        #endregion
    }
}