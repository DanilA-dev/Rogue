using System.Collections.Generic;
using D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler;
using D_Dev.UtilScripts.PositionConfig;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Core.Weapon
{
    public class WeaponHolder : MonoBehaviour
    {
        #region Fields

        [Title("Weapon Settings")] 
        [SerializeField] private WeaponInfo _startWeaponInfo;
        [SerializeField, ReadOnly] private WeaponInfo _currentWeaponInfo;
        [SerializeField] private PositionConfig _weaponPosition;
        [Space]
        [Title("View Settings")]
        [SerializeField] private AnimationClipPlayableMixer _weaponAnimationClipPlayableMixer;
        [FoldoutGroup("Events")] 
        public UnityEvent<WeaponBehaviour> OnEquip;
        [FoldoutGroup("Events")] 
        public UnityEvent<WeaponBehaviour> OnUse;

        private Dictionary<WeaponInfo, WeaponBehaviour> _equippedWeapons = new();
        
        #endregion

        #region Properties

        public WeaponBehaviour CurrentWeaponItem {get; private set;}

        #endregion

        #region Monobehaviour

        private void Start()
        {
            TrySetStartWeapon();
        }

        #endregion

        #region Public

        public void UseWeapon()
        {
            if (_currentWeaponInfo != null)
            {
                CurrentWeaponItem = _equippedWeapons[_currentWeaponInfo];
                CurrentWeaponItem?.Use();
                OnUse?.Invoke(CurrentWeaponItem);
            }
        }

        #endregion
        
        #region Private

        private void TrySetStartWeapon()
        {
            if(_startWeaponInfo == null)
                return;
            
            SetWeapon(_startWeaponInfo);
        }

        private void SetWeapon(WeaponInfo info)
        {
            if (info == null)
            {
                Debug.LogError($"Can't create equippable weapon, info is null");
                return;
            }

            if (_currentWeaponInfo != null)
                _equippedWeapons[_currentWeaponInfo].Unequip();
            
            if (_equippedWeapons.TryGetValue(info, out var weaponBehaviour))
            {
                weaponBehaviour.Equip(_weaponAnimationClipPlayableMixer,info.WeaponData);
                _currentWeaponInfo = info;
                return;
            }

            var newEquippableWeapon = Instantiate(info.EntityPrefab).GetComponent<WeaponBehaviour>();
            var newEquippableWeaponTransform = newEquippableWeapon.transform;
            _weaponPosition.SetPosition(ref newEquippableWeaponTransform);
            _weaponPosition.SetRotation(ref newEquippableWeaponTransform);
            newEquippableWeapon.Equip(_weaponAnimationClipPlayableMixer,info.WeaponData);
            _currentWeaponInfo = info;
            _equippedWeapons.TryAdd(info, newEquippableWeapon);
            OnEquip?.Invoke(newEquippableWeapon);
        }

        #endregion

    }
}