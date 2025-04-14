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
        [SerializeField] private EquippableWeaponConfig _equippableWeaponConfig;
        [SerializeField] private AnimationClipPlayableHandler _playableHandler;
            
        #endregion

        #region Properties
        
        public bool LoadConfigFromInfo
        {
            get => _loadConfigFromInfo;
            set => _loadConfigFromInfo = value;
        }
        public EquippableWeaponConfig WeaponConfig => _equippableWeaponConfig;

        public float FullActionStateTime => _equippableWeaponConfig.AttackActionDelayTime +
                                            _equippableWeaponConfig.AttackActionTime +
                                            _equippableWeaponConfig.CooldownTime;

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
            
            InitTransitions();
        }

        #endregion

        #region Public

        public void Equip(AnimationClipPlayableHandler playableHandler,EquippableWeaponConfig equippableWeaponConfig)
        {
            gameObject.SetActive(true);
            _playableHandler = playableHandler;
            _equippableWeaponConfig = _loadConfigFromInfo 
                ? equippableWeaponConfig 
                : _equippableWeaponConfig;

            _damageCollider.DamageInfo = _equippableWeaponConfig.DamageInfo;
            ClearTransitions();
            InitTransitions();
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
            
            ChangeState(_equippableWeaponConfig.IsChargable
                ? EquippableWeaponState.ChargeStart
                : EquippableWeaponState.AttackStart);
        }

        
        public void PlayEquippableWeaponAnimation(EquippableWeaponState state)
        {
            var anim = _equippableWeaponConfig.GetAnimation(state);
            if(anim == null)
                return;
            
            _playableHandler?.Play(anim.AnimationConfig);
        }
            
        #endregion

        #region Private

        private void ClearTransitions()
        {
            RemoveTransition(EquippableWeaponState.AttackStart);
            RemoveTransition(EquippableWeaponState.AttackAction);
            RemoveTransition(EquippableWeaponState.ChargeStart);
            RemoveTransition(EquippableWeaponState.Cooldown);
        }
        
        private void InitTransitions()
        {
            AddTransition(new [] { EquippableWeaponState.AttackStart }, EquippableWeaponState.AttackAction,
                new DelayCondition(_equippableWeaponConfig.AttackActionDelayTime));
            
            AddTransition(new [] { EquippableWeaponState.AttackAction }, EquippableWeaponState.Cooldown,
                new DelayCondition(_equippableWeaponConfig.AttackActionTime));
            
            AddTransition(new [] { EquippableWeaponState.ChargeStart}, EquippableWeaponState.ChargeEnd,
                new DelayCondition(_equippableWeaponConfig.ChargeTime));
            
            AddTransition(new [] { EquippableWeaponState.Cooldown }, EquippableWeaponState.Idle,
                new DelayCondition(_equippableWeaponConfig.CooldownTime));
        }

        #endregion
    }
}