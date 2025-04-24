using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Core.Weapon
{
    [System.Serializable]
    public class WeaponData
    {
        #region Fields

        [HideIf(nameof(_useRandomAttackConfigs))]
        [SerializeField] private WeaponAttackConfig _weaponAttackConfig;
        [SerializeField] private bool _useRandomAttackConfigs;
        [ShowIf(nameof(_useRandomAttackConfigs))]
        [SerializeField] private List<WeaponAttackConfig> _randomWeaponAttackConfigs;

        #endregion

        #region Properties

        public List<WeaponAttackConfig> RandomWeaponAttackConfigs
        {
            get => _randomWeaponAttackConfigs;
            set => _randomWeaponAttackConfigs = value;
        }

        #endregion

        #region Public

        public WeaponAttackConfig GetAttackConfig()
        {
            return _useRandomAttackConfigs
                ? _randomWeaponAttackConfigs[Random.Range(0, _randomWeaponAttackConfigs.Count)]
                : _weaponAttackConfig;
        }

        #endregion
    }
}