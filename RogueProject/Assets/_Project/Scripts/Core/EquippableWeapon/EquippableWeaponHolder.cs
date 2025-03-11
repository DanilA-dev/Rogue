using System.Collections.Generic;
using D_Dev.UtilScripts.PositionConfig;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Core.EquippableWeapon
{
    public class EquippableWeaponHolder : MonoBehaviour
    {
        #region Fields

        [Title("Weapon Settings")] 
        [SerializeField] private EquippableWeaponInfo _startEquipmentEquippableWeaponInfo;
        [SerializeField, ReadOnly] private EquippableWeaponInfo _currentEquippableWeaponInfo;
        [SerializeField] private PositionConfig _equippableWeaponPosition;

        private Dictionary<EquippableWeaponInfo, EquippableWeaponBehaviour> _equippableWeapons = new();
        
        #endregion

        #region Monobehaviour

        private void Start()
        {
            TrySetStartEquippableWeapon();
        }

        #endregion

        #region Public

        public void UseEquippableWeapon()
        {
            if (_currentEquippableWeaponInfo != null)
                _equippableWeapons[_currentEquippableWeaponInfo].Use();
        }

        #endregion
        
        #region Private

        private void TrySetStartEquippableWeapon()
        {
            if(_startEquipmentEquippableWeaponInfo == null)
                return;
            
        }

        private void SetEquippableWeapon(EquippableWeaponInfo info)
        {
            if (info == null)
            {
                Debug.LogError($"Can't create equippable weapon, info is null");
                return;
            }

            if (_currentEquippableWeaponInfo != null)
                _equippableWeapons[_currentEquippableWeaponInfo].Unequip();
            
            if (_equippableWeapons.TryGetValue(info, out var equippableWeaponBehaviour))
            {
                equippableWeaponBehaviour.Equip(info.Config);
                _currentEquippableWeaponInfo = info;
                return;
            }

            var newEquippableWeapon = Instantiate(info.EntityPrefab, _equippableWeaponPosition.GetPosition(),
                _equippableWeaponPosition.GetRotation()).GetComponent<EquippableWeaponBehaviour>();
            
            newEquippableWeapon.Equip(info.Config);
            _currentEquippableWeaponInfo = info;
            _equippableWeapons.TryAdd(info, newEquippableWeapon);
        }

        #endregion

    }
}