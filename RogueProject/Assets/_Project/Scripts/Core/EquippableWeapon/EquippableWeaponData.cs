using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Core.EquippableWeapon
{
    [System.Serializable]
    public class EquippableWeaponData
    {
        #region Fields

        [HideIf(nameof(_useRandomAttackConfigs))]
        [SerializeField] private EquippableWeaponAttackConfig _weaponAttackConfig;
        [SerializeField] private bool _useRandomAttackConfigs;
        [ShowIf(nameof(_useRandomAttackConfigs))]
        [SerializeField] private List<EquippableWeaponAttackConfig> _randomWeaponAttackConfigs;

        #endregion

        #region Properties

        public List<EquippableWeaponAttackConfig> RandomWeaponAttackConfigs
        {
            get => _randomWeaponAttackConfigs;
            set => _randomWeaponAttackConfigs = value;
        }

        #endregion

        #region Public

        public EquippableWeaponAttackConfig GetAttackConfig()
        {
            return _useRandomAttackConfigs
                ? _randomWeaponAttackConfigs[Random.Range(0, _randomWeaponAttackConfigs.Count)]
                : _weaponAttackConfig;
        }

        #endregion
    }
}