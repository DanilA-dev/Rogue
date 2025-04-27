using UnityEngine;

namespace _Project.Scripts.Core.Weapon
{
    [System.Serializable]
    public class WeaponData
    {
        #region Fields

        [SerializeField] private float _attacksTransitionWindowTime;
        [SerializeField] private WeaponAttackConfig[] _weaponAttacks;

        #endregion

        #region Properties

        public WeaponAttackConfig[] WeaponAttacks
        {
            get => _weaponAttacks;
            set => _weaponAttacks = value;
        }

        public float AttacksTransitionWindowTime
        {
            get => _attacksTransitionWindowTime;
            set => _attacksTransitionWindowTime = value;
        }

        #endregion
    }
}