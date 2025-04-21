using D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler;
using D_Dev.Scripts.Runtime.UtilScripts.SimpleStateMachine;
using D_Dev.Scripts.Runtime.UtilScripts.StateMachineBehaviour;
using Danil_dev.Scripts.Runtime.UtilScripts.DamagableSystem.DamagableCollider;
using UnityEngine;

namespace _Project.Scripts.Core.EquippableWeapon
{
    public enum EquippableWeaponState
    {
        Equipping = -1,
        Idle = 0,
        AttackStart = 1,
        AttackAction = 2,
        ChargeStart = 3,
        ChargeEnd = 4,
        Cooldown = 5
    }
    
    public class EquippableWeaponBehaviour : StateMachineBehaviour<EquippableWeaponState>
    {
        #region Fields

        [SerializeField] private bool _loadConfigFromInfo;
        [SerializeField] private DamageCollider _damageCollider;
        [SerializeField] private EquippableWeaponData _equippableWeaponData;
        [SerializeField] private AnimationClipPlayableMixer _clipPlayableMixer;

        private EquippableWeaponAttackConfig _lastAttackConfig;
        private bool _isTransitionsInitialized;
            
        #endregion

        #region Properties
        
        public bool LoadConfigFromInfo
        {
            get => _loadConfigFromInfo;
            set => _loadConfigFromInfo = value;
        }
        public EquippableWeaponData WeaponData => _equippableWeaponData;

        public float FullActionStateTime => _equippableWeaponData.GetAttackConfig().AttackActionDelayTime +
                                            _equippableWeaponData.GetAttackConfig().AttackActionTime +
                                            _equippableWeaponData.GetAttackConfig().CooldownTime;

        #endregion

        #region Overrides

        protected override void InitStates()
        {
            AddState(EquippableWeaponState.Equipping,new EquippingEquippableWeaponState(this));
            AddState(EquippableWeaponState.Idle, new IdleEquippableWeaponState(this));
            AddState(EquippableWeaponState.AttackStart, new AttackStartEquippableWeaponState(this));
            AddState(EquippableWeaponState.AttackAction, new AttackActionEquippableWeaponState(this));
            AddState(EquippableWeaponState.ChargeStart, new ChargeStartEquippableWeaponState(this));
            AddState(EquippableWeaponState.ChargeEnd, new ChargeEndEquippableWeaponState(this));
            AddState(EquippableWeaponState.Cooldown, new CooldownEquippableWeaponState(this));
            
            ChangeState(_startState);
            
            if(_loadConfigFromInfo)
                return;
            
            InitAttackTransitions(_equippableWeaponData.GetAttackConfig());
        }

        #endregion

        #region Public

        public void Equip(AnimationClipPlayableMixer playableHandler, EquippableWeaponData loadedEquippableWeaponData)
        {
            gameObject.SetActive(true);
            _clipPlayableMixer = playableHandler;
            _equippableWeaponData = _loadConfigFromInfo 
                ? loadedEquippableWeaponData 
                : _equippableWeaponData;


            _lastAttackConfig = _equippableWeaponData.GetAttackConfig();
            ClearTransitions();
            InitAttackTransitions(_lastAttackConfig);
        }
        
        public void Unequip()
        {
            ChangeState(EquippableWeaponState.Idle);
            gameObject.SetActive(false);
        }

        public void Use()
        {
            if(_currentState != EquippableWeaponState.Idle)
                return;

            _lastAttackConfig = _equippableWeaponData.GetAttackConfig();
            _damageCollider.DamageInfo = _lastAttackConfig.DamageInfo;
            ClearTransitions();
            InitAttackTransitions(_lastAttackConfig);
            
            ChangeState(_lastAttackConfig.IsChargable
                ? EquippableWeaponState.ChargeStart
                : EquippableWeaponState.AttackStart);
        }

        
        public void PlayEquippableWeaponAnimation(EquippableWeaponState state)
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
            
            _clipPlayableMixer?.Play(_lastAttackConfig.WeaponAnimation);
        }
            
        #endregion

        #region Private

        private void ClearTransitions()
        {
            RemoveTransition(EquippableWeaponState.AttackStart);
            RemoveTransition(EquippableWeaponState.AttackAction);
            RemoveTransition(EquippableWeaponState.ChargeStart);
            RemoveTransition(EquippableWeaponState.Cooldown);
            
            _isTransitionsInitialized = false;
        }
        
        private void InitAttackTransitions(EquippableWeaponAttackConfig attackConfig)
        {
            if(_isTransitionsInitialized)
                return;
            
            AddTransition(new [] { EquippableWeaponState.AttackStart }, EquippableWeaponState.AttackAction,
                new DelayCondition(attackConfig.AttackActionDelayTime));
            
            AddTransition(new [] { EquippableWeaponState.AttackAction }, EquippableWeaponState.Cooldown,
                new DelayCondition(attackConfig.AttackActionTime));
            
            AddTransition(new [] { EquippableWeaponState.ChargeStart}, EquippableWeaponState.ChargeEnd,
                new DelayCondition(attackConfig.ChargeTime));
            
            AddTransition(new [] { EquippableWeaponState.Cooldown }, EquippableWeaponState.Idle,
                new DelayCondition(attackConfig.CooldownTime));

            _isTransitionsInitialized = true;
        }

        #endregion
    }
}