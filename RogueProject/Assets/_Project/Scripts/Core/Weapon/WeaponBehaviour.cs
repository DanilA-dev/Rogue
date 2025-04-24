using D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler;
using D_Dev.Scripts.Runtime.UtilScripts.SimpleStateMachine;
using D_Dev.Scripts.Runtime.UtilScripts.StateMachineBehaviour;
using Danil_dev.Scripts.Runtime.UtilScripts.DamagableSystem.DamagableCollider;
using UnityEngine;

namespace _Project.Scripts.Core.Weapon
{
    public enum WeaponState
    {
        Equipping = -1,
        Idle = 0,
        AttackStart = 1,
        AttackAction = 2,
        ChargeStart = 3,
        ChargeEnd = 4,
        Cooldown = 5
    }
    
    public class WeaponBehaviour : StateMachineBehaviour<WeaponState>
    {
        #region Fields

        [SerializeField] private bool _loadConfigFromInfo;
        [SerializeField] private DamageCollider _damageCollider;
        [SerializeField] private WeaponData _weaponData;
        [Space]
        [SerializeField] private AnimationClipPlayableMixer _animationClipPlayableMixer;

        private WeaponAttackConfig _lastAttackConfig;
        private bool _isTransitionsInitialized;
            
        #endregion

        #region Properties
        
        public bool LoadConfigFromInfo
        {
            get => _loadConfigFromInfo;
            set => _loadConfigFromInfo = value;
        }
        public WeaponData WeaponData => _weaponData;

        public float FullActionStateTime => _weaponData.GetAttackConfig().AttackActionDelayTime +
                                            _weaponData.GetAttackConfig().AttackActionTime +
                                            _weaponData.GetAttackConfig().CooldownTime;

        public bool StopMovementOnAttack { get; set; } = true;
        
        public WeaponAttackConfig LastAttackConfig => _lastAttackConfig;
        
        public WeaponState CurrentState => _currentState;

        #endregion

        #region Overrides

        protected override void InitStates()
        {
            AddState(WeaponState.Equipping,new EquippingWeaponState(this));
            AddState(WeaponState.Idle, new IdleWeaponState(this));
            AddState(WeaponState.AttackStart, new AttackStartWeaponState(this));
            AddState(WeaponState.AttackAction, new AttackActionWeaponState(this));
            AddState(WeaponState.ChargeStart, new ChargeStartWeaponState(this));
            AddState(WeaponState.ChargeEnd, new ChargeEndWeaponState(this));
            AddState(WeaponState.Cooldown, new CooldownWeaponState(this));
            
            ChangeState(_startState);
            
            if(_loadConfigFromInfo)
                return;
            
            InitAttackTransitions(_weaponData.GetAttackConfig());
        }

        #endregion

        #region Public

        public void Equip(AnimationClipPlayableMixer playableHandler, WeaponData loadedEquippableWeaponData)
        {
            gameObject.SetActive(true);
            _animationClipPlayableMixer = playableHandler;
            _weaponData = _loadConfigFromInfo 
                ? loadedEquippableWeaponData 
                : _weaponData;


            _lastAttackConfig = _weaponData.GetAttackConfig();
            ClearTransitions();
            InitAttackTransitions(_lastAttackConfig);
        }
        
        public void Unequip()
        {
            ChangeState(WeaponState.Idle);
            gameObject.SetActive(false);
        }

        public void Use()
        {
            if(_currentState != WeaponState.Idle)
                return;

            _lastAttackConfig = _weaponData.GetAttackConfig();
            _damageCollider.DamageInfo = _lastAttackConfig.DamageInfo;
            
            ClearTransitions();
            InitAttackTransitions(_lastAttackConfig);
            
            ChangeState(_lastAttackConfig.IsChargable
                ? WeaponState.ChargeStart
                : WeaponState.AttackStart);
        }

        
        public void PlayEquippableWeaponAnimation(WeaponState state)
        {
            // var anim = _equippableWeaponConfig.GetAnimation(state);
            // if(anim == null)
            //     return;
            //
            // _playableHandler?.Play(anim);
        }

        public void PlayAttackConfigAnimation()
        {
            if(_lastAttackConfig == null)
                return;
            
            _animationClipPlayableMixer?.Play(_lastAttackConfig.WeaponAnimation);
        }
            
        #endregion

        #region Private

        private void ClearTransitions()
        {
            RemoveTransition(WeaponState.AttackStart);
            RemoveTransition(WeaponState.AttackAction);
            RemoveTransition(WeaponState.ChargeStart);
            RemoveTransition(WeaponState.Cooldown);
            
            _isTransitionsInitialized = false;
        }
        
        private void InitAttackTransitions(WeaponAttackConfig attackConfig)
        {
            if(_isTransitionsInitialized)
                return;
            
            AddTransition(new [] { WeaponState.AttackStart }, WeaponState.AttackAction,
                new DelayCondition(attackConfig.AttackActionDelayTime));
            
            AddTransition(new [] { WeaponState.AttackAction }, WeaponState.Cooldown,
                new DelayCondition(attackConfig.AttackActionTime));
            
            AddTransition(new [] { WeaponState.ChargeStart}, WeaponState.ChargeEnd,
                new DelayCondition(attackConfig.ChargeTime));
            
            AddTransition(new [] { WeaponState.Cooldown }, WeaponState.Idle,
                new DelayCondition(attackConfig.CooldownTime));

            _isTransitionsInitialized = true;
        }

        #endregion
    }
}