using D_Dev.Scripts.Runtime.UtilScripts.StateMachineBehaviour;
using UnityEngine;

namespace _Project.Scripts.Core.EquippableWeapon
{
    public enum EquippableWeaponState
    {
        Equipping = -1,
        Idle = 0,
        Attack = 1,
        Charge = 2,
        Cooldown = 3
    }
    
    public class EquippableWeaponBehaviour : StateMachineBehaviour<EquippableWeaponState>
    {
        #region Fields

        [SerializeField] private bool _loadConfigFromInfo;
        [SerializeField] private EquippableWeaponConfig _equippableWeaponConfig;
        [Space] 
        [SerializeField] private EquippableWeaponView _equippableWeaponView;

        #endregion

        #region Properties

        public bool LoadConfigFromInfo
        {
            get => _loadConfigFromInfo;
            set => _loadConfigFromInfo = value;
        }

        #endregion

        #region Overrides

        protected override void InitStates()
        {
            
        }

        #endregion

        #region Public

        public void Equip(EquippableWeaponConfig equippableWeaponConfig)
        {
            gameObject.SetActive(true);
            
            _equippableWeaponConfig = _loadConfigFromInfo 
                ? equippableWeaponConfig 
                : _equippableWeaponConfig;
            
            ChangeState(_startState);
            ChangeAnimation(equippableWeaponConfig, _startState);
        }
       
        public void Unequip()
        {
            ChangeEquippableWeaponState(EquippableWeaponState.Idle);
            gameObject.SetActive(false);
        }

        public void Use()
        {
            
        }

        public void ChangeEquippableWeaponState(EquippableWeaponState equippableWeaponState) 
            => ChangeState(equippableWeaponState);

        #endregion

        #region Private

        private void ChangeAnimation(EquippableWeaponConfig equippableWeaponConfig, EquippableWeaponState state)
        {
            var anim = equippableWeaponConfig.GetAnimation(state);
            _equippableWeaponView.ChangeAnimation(anim.AnimationClip, anim.CrossFadeTime);
        }

        #endregion
        
    }
}