using System.Linq;
using D_Dev.UtilScripts.DamagableSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Core.EquippableWeapon
{
    [System.Serializable]
    public class EquippableWeaponConfig
    {
        #region Fields

        [SerializeField] private DamageInfo _damageInfo;
        [Space]
        [SerializeField] private bool _isChargable;
        [ShowIf(nameof(_isChargable))]
        [SerializeField] private float _chargeTime;
        [Space]
        [SerializeField] private float _attackingTime;
        [SerializeField] private float _cooldownTime;
        [Space]
        [SerializeField] private EquippableWeaponAnimation[] _animations;

        #endregion

        #region Properties

        public DamageInfo DamageInfo => _damageInfo;
        public float AttackingTime => _attackingTime;
        public float CooldownTime => _cooldownTime;
        public bool IsChargable => _isChargable;
        public EquippableWeaponAnimation[] Animations => _animations;
        public float ChargeTime => _chargeTime;

        #endregion

        #region Public

        public EquippableWeaponAnimation GetAnimation(EquippableWeaponState state)
        {
            return _animations.FirstOrDefault(a => a.State == state);
        }

        #endregion
    }
}