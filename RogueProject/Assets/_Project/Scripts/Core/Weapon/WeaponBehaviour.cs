using D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler;
using D_Dev.Scripts.Runtime.UtilScripts.SimpleStateMachine;
using D_Dev.Scripts.Runtime.UtilScripts.StateMachineBehaviour;
using D_Dev.UtilScripts.TimerSystem;
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
        private int _attackIndex;
        private CountdownTimer _attacksFlowTimer;
        #endregion

        #region Properties
        
        public bool LoadConfigFromInfo
        {
            get => _loadConfigFromInfo;
            set => _loadConfigFromInfo = value;
        }
        public WeaponData WeaponData => _weaponData;

        public bool StopMovementOnAttack { get; set; } = true;
        public WeaponAttackConfig LastAttackConfig => _lastAttackConfig;
        public WeaponState CurrentState => _currentState;

        public CountdownTimer AttacksFlowTimer => _attacksFlowTimer;

        #endregion

        #region Overrides

        protected override void InitStates()
        {
            _attacksFlowTimer = new CountdownTimer(_weaponData.AttacksTransitionWindowTime);
            
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

        protected override void Update()
        {
            base.Update();
            _attacksFlowTimer?.Tick(Time.deltaTime);
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


            _attacksFlowTimer?.Stop();
            _attacksFlowTimer?.Reset(_weaponData.AttacksTransitionWindowTime);
            _lastAttackConfig = _weaponData.WeaponAttacks[0];
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
            if(_weaponData.WeaponAttacks.Length == 1 &&
               _currentState != WeaponState.Idle)
                return;

            if (_attacksFlowTimer.IsRunning)
                return;
            
            if (_currentState != WeaponState.Idle &&
                _weaponData.WeaponAttacks.Length > 1 &&
                !_attacksFlowTimer.IsRunning)
            {
                _attacksFlowTimer.Start();
            }

            if (_attacksFlowTimer.IsRunning && _currentState != WeaponState.Idle)
            {
                _attackIndex++;
                ChangeState(WeaponState.Idle);
                ClearTransitions();
                _lastAttackConfig = _weaponData.WeaponAttacks[_attackIndex % _weaponData.WeaponAttacks.Length];
            }
            else
                _lastAttackConfig = _weaponData.WeaponAttacks[0];
            
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

        public void StopLastAttackConfigAnimation()
        {
            if(_lastAttackConfig == null)
                return;
            
            _animationClipPlayableMixer?.Stop(_lastAttackConfig.WeaponAnimation);
        }

        public float FullActionStateTime()
        {
            if (_lastAttackConfig == null)
                return 0;
            
            return _lastAttackConfig.AttackActionDelayTime +
                   _lastAttackConfig.AttackActionTime +
                   _lastAttackConfig.CooldownTime;
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