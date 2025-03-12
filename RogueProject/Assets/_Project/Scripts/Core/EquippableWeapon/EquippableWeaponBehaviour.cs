using D_Dev.Scripts.Runtime.UtilScripts.SimpleStateMachine;
using D_Dev.Scripts.Runtime.UtilScripts.StateMachineBehaviour;
using UnityEngine;

namespace _Project.Scripts.Core.EquippableWeapon
{
    public enum EquippableWeaponState
    {
        Equipping = -1,
        Idle = 0,
        Attack = 1,
        ChargeStart = 2,
        ChargeEnd = 3,
        Cooldown = 4
    }
    
    public class EquippableWeaponBehaviour : StateMachineBehaviour<EquippableWeaponState>
    {
        #region Fields

        [SerializeField] private bool _loadConfigFromInfo;
        [SerializeField] private EquippableWeaponConfig _equippableWeaponConfig;
        [Space] 
        [SerializeField] private EquippableWeaponView _equippableWeaponView;

        #endregion

        #region Properties
        
        public bool LoadConfigFromInfo
        {
            get => _loadConfigFromInfo;
            set => _loadConfigFromInfo = value;
        }
        public EquippableWeaponConfig WeaponConfig => _equippableWeaponConfig;
        public EquippableWeaponView WeaponView => _equippableWeaponView;

        #endregion

        #region Overrides

        protected override void InitStates()
        {
            AddState(EquippableWeaponState.Equipping,new EquippingEquippableWeaponState(this));
            AddState(EquippableWeaponState.Idle, new IdleEquippableWeaponState(this));
            AddState(EquippableWeaponState.Attack, new AttackEquippableWeaponState(this));
            AddState(EquippableWeaponState.ChargeStart, new ChargeStartEquippableWeaponState(this));
            AddState(EquippableWeaponState.ChargeEnd, new ChargeEndEquippableWeaponState(this));
            AddState(EquippableWeaponState.Cooldown, new CooldownEquippableWeaponState(this));
            
            AddTransition(new [] { EquippableWeaponState.Attack }, EquippableWeaponState.Cooldown,
                new DelayCondition(_equippableWeaponConfig.AttackingTime));
            
            AddTransition(new [] { EquippableWeaponState.ChargeStart}, EquippableWeaponState.ChargeEnd,
                new DelayCondition(_equippableWeaponConfig.ChargeTime));
            
            AddTransition(new [] { EquippableWeaponState.Cooldown }, EquippableWeaponState.Idle,
                new DelayCondition(_equippableWeaponConfig.CooldownTime));
        }

        #endregion

        #region Public

        public void Equip(Animator animator,EquippableWeaponConfig equippableWeaponConfig)
        {
            gameObject.SetActive(true);
            _equippableWeaponView.Init(animator);
            _equippableWeaponConfig = _loadConfigFromInfo 
                ? equippableWeaponConfig 
                : _equippableWeaponConfig;
            
            ChangeState(_startState);
            ChangeAnimation(_startState);
        }
       
        public void Unequip()
        {
            ChangeEquippableWeaponState(EquippableWeaponState.Idle);
            gameObject.SetActive(false);
        }

        public void Use()
        {
            if(_currentState != EquippableWeaponState.Idle)
                return;
            
            ChangeEquippableWeaponState(_equippableWeaponConfig.IsChargable
                ? EquippableWeaponState.ChargeStart
                : EquippableWeaponState.Attack);
        }

        public void ChangeEquippableWeaponState(EquippableWeaponState equippableWeaponState) 
            => ChangeState(equippableWeaponState);

        public void ChangeAnimation(EquippableWeaponState state)
        {
            var anim = _equippableWeaponConfig.GetAnimation(state);
            if(anim == null)
                return;
            
            _equippableWeaponView.ChangeAnimation(anim.AnimationClip, anim.CrossFadeTime);
        }
        
        public void PlayAnimation(EquippableWeaponState state)
        {
            var anim = _equippableWeaponConfig.GetAnimation(state);
            if(anim == null)
                return;
            
            _equippableWeaponView.PlayAnimation(anim.AnimationClip);
        }
            
        #endregion

       
        
    }
}