using D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler;
using D_Dev.UtilScripts.DamagableSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Core.Weapon
{
    [System.Serializable]
    public class WeaponAttackConfig
    {
        #region Fields

        [SerializeField] private DamageInfo _damageInfo;
        [Space]
        [SerializeField] private bool _isChargable;
        [ShowIf(nameof(_isChargable))]
        [SerializeField] private float _chargeTime;
        [Space]
        [SerializeField] private bool _stopMovementWhileAttacking = true;
        [SerializeField] private float _attackActionDelayTime;
        [SerializeField] private float _attackActionTime;
        [SerializeField] private float _cooldownTime;
        [Space]
        [SerializeField] private AnimationPlayableClipConfig _weaponAnimation;

        #endregion

        #region Properties
        public DamageInfo DamageInfo
        {
            get => _damageInfo;
            set => _damageInfo = value;
        }

        public float AttackActionTime
        {
            get => _attackActionTime;
            set => _attackActionTime = value;
        }

        public float CooldownTime
        {
            get => _cooldownTime;
            set => _cooldownTime = value;
        }

        public bool IsChargable
        {
            get => _isChargable;
            set => _isChargable = value;
        }

        public AnimationPlayableClipConfig WeaponAnimation
        {
            get => _weaponAnimation;
            set => _weaponAnimation = value;
        }

        public float ChargeTime
        {
            get => _chargeTime;
            set => _chargeTime = value;
        }

        public float AttackActionDelayTime
        {
            get => _attackActionDelayTime;
            set => _attackActionDelayTime = value;
        }

        public bool StopMovementWhileAttacking
        {
            get => _stopMovementWhileAttacking;
            set => _stopMovementWhileAttacking = value;
        }

        #endregion
    }
}