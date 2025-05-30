using _Project.Scripts.Core.Weapon;
using UnityEngine;

namespace _Project.Scripts.Core
{
    [System.Serializable]
    public class InputWeaponHolderReceiverModule : BaseInputReceiverModule
    {
        #region Fields

        [SerializeField] private WeaponHolder _equippableWeapon;

        #endregion
        
        #region Overrides

        public override void OnInputEnable()
        {
            if(_equippableWeapon == null)
                return;

            _inputRouter.LmbPressed += OnLMBPressed;
        }

        public override void OnInputDisable()
        {
            if(_equippableWeapon == null)
                return;
            
            _inputRouter.LmbPressed -= OnLMBPressed;
        }
        
        #endregion

        #region Listeners

        private void OnLMBPressed(bool isPressed)
        {
            if(isPressed)
                _equippableWeapon.UseWeapon();
        }

        #endregion
        
        
    }
}