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
        public DamageInfo DamageInfo => _damageInfo;
        public float AttackActionTime => _attackActionTime;
        public float CooldownTime => _cooldownTime;
        public bool IsChargable => _isChargable;
        public AnimationPlayableClipConfig WeaponAnimation => _weaponAnimation;
        public float ChargeTime => _chargeTime;
        public float AttackActionDelayTime => _attackActionDelayTime;

        public bool StopMovementWhileAttacking
        {
            get => _stopMovementWhileAttacking;
            set => _stopMovementWhileAttacking = value;
        }

        #endregion
    }
}