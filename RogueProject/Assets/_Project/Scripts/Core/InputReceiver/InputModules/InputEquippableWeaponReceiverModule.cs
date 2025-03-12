using _Project.Scripts.Core.EquippableWeapon;
using UnityEngine;

namespace _Project.Scripts.Core
{
    [System.Serializable]
    public class InputEquippableWeaponReceiverModule : BaseInputReceiverModule
    {
        #region Fields

        [SerializeField] private EquippableWeaponHolder _equippableWeapon;

        #endregion
        
        #region Overrides

        public override void OnInputEnable()
        {
            if(_equippableWeapon == null)
                return;

            _inputRouter.LmbPressed += (isPressed) =>
            {
                if(isPressed)
                    _equippableWeapon.UseEquippableWeapon();
            };
        }

        public override void OnInputDisable()
        {
            if(_equippableWeapon == null)
                return;
            
            _inputRouter.LmbPressed -= (isPressed) =>
            {
                if(isPressed)
                    _equippableWeapon.UseEquippableWeapon();
            };
        }
        
        #endregion
        
        
    }
}