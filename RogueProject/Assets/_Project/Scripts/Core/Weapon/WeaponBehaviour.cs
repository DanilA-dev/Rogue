using System;
using D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler;
using D_Dev.Scripts.Runtime.UtilScripts.SimpleStateMachine;
using D_Dev.Scripts.Runtime.UtilScripts.StateMachineBehaviour;
using D_Dev.UtilScripts.ScriptableVaiables;
using D_Dev.UtilScripts.TimerSystem;
using Danil_dev.Scripts.Runtime.UtilScripts.DamagableSystem.DamagableCollider;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

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

        [SerializeField] private Rigidbody _mainRigidBody;
        [SerializeField] private FloatScriptableVariable _staminaVariable;
        [Space]
        [SerializeField] private bool _loadConfigFromInfo;
        [SerializeField] private DamageCollider _damageCollider;
        [SerializeField] private WeaponData _weaponData;
        [Space]
        [SerializeField] private AnimationClipPlayableMixer _animationClipPlayableMixer;

        [FoldoutGroup("Events")]
        public UnityEvent OnEquip;
        [FoldoutGroup("Events")]
        public UnityEvent OnUnequip;

        private WeaponAttackConfig _lastAttackConfig;
        
        private bool _isTransitionsInitialized;
        private int _attackIndex;
        private CountdownTimer _attackTransitionTimer;
        #endregion

        #region Properties
        
        public bool LoadConfigFromInfo
        {
            get => _loadConfigFromInfo;
            set => _loadConfigFromInfo = value;
        }
        public WeaponData WeaponData
        {
            get => _weaponData;
            set => _weaponData = value;
        }

        public bool StopMovementOnAttack { get; set; } = true;
        public WeaponAttackConfig LastAttackConfig => _lastAttackConfig;
        public WeaponState CurrentState => _currentState;

        public Rigidbody MainRigidBody => _mainRigidBody;

        public FloatScriptableVariable StaminaVariable => _staminaVariable;

        #endregion

        #region Overrides

        protected override void InitStates()
        {
            _attackTransitionTimer = new CountdownTimer(_weaponData.AttacksTransitionWindowTime);
            _attackTransitionTimer.OnTimerEnd += ResetAttackIndex;
            
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

            _lastAttackConfig = _weaponData.WeaponAttacks[0];
            InitAttackTransitions(_lastAttackConfig);
        }

        private void OnDisable() => _attackTransitionTimer.OnTimerEnd -= ResetAttackIndex;

        protected override void Update()
        {
            base.Update();
            _attackTransitionTimer?.Tick(Time.deltaTime);
        }

        #endregion
        
        #region Public

        public void Equip(AnimationClipPlayableMixer playableHandler,
            WeaponData loadedEquippableWeaponData, Rigidbody mainRigidBody)
        {
            gameObject.SetActive(true);
            _animationClipPlayableMixer = playableHandler;
            _weaponData = _loadConfigFromInfo 
                ? loadedEquippableWeaponData 
                : _weaponData;

            _mainRigidBody = mainRigidBody;
            _lastAttackConfig = _weaponData.WeaponAttacks[0];
            ClearTransitions();
            InitAttackTransitions(_lastAttackConfig);
            _attackTransitionTimer?.Reset(_weaponData.AttacksTransitionWindowTime);
            OnEquip?.Invoke();
        }
        
        public void Unequip()
        {
            _lastAttackConfig = null;
            ChangeState(WeaponState.Idle);
            OnUnequip?.Invoke();
        }

        public void Use()
        {
            if(_currentState != WeaponState.Idle)
                return;
            
            _lastAttackConfig = _weaponData.WeaponAttacks[_attackIndex % _weaponData.WeaponAttacks.Length];
            if(_staminaVariable.Value < _lastAttackConfig.StaminaCost)
                return;
            
            _attackTransitionTimer?.Start();
            _attackIndex++;
            ChangeState(WeaponState.AttackStart);
        }

        
        public void PlayEquippableWeaponAnimation(WeaponState state)
        {
            
        }

        public void PlayAttackConfigAnimation()
        {
            if(_lastAttackConfig == null)
                return;
            
            _animationClipPlayableMixer?.Play(_lastAttackConfig.WeaponAnimation);
        }
       

        public float FullActionStateTime()
        {
            if (_lastAttackConfig == null)
                return 0;
            
            return _lastAttackConfig.AttackStartTime +
                   _lastAttackConfig.AttackActionTime +
                   _lastAttackConfig.CooldownTime;
        }
        
        #endregion

        #region Private
        
        private void ResetAttackIndex()
        {
            _attackIndex = 0;
        }

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
                new DelayCondition(attackConfig.AttackStartTime));
            
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