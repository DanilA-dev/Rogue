using System.Collections;
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
        Attack = 1,
        ChargeStart = 2,
        ChargeEnd = 3,
        Cooldown = 4
    }
    
    public class EquippableWeaponBehaviour : StateMachineBehaviour<EquippableWeaponState>
    {
        #region Fields

        [SerializeField] private bool _loadConfigFromInfo;
        [SerializeField] private DamageCollider _damageCollider;
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

        #region Monobehaviour

        private void OnDisable() => OnAnyStateEnter.RemoveListener(PlayAnimation);

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
            
        }

        #endregion

        #region Public

        public void Equip(Animator animator,EquippableWeaponConfig equippableWeaponConfig)
        {
            gameObject.SetActive(true);
            _equippableWeaponView.Animator = animator;
            _equippableWeaponConfig = _loadConfigFromInfo 
                ? equippableWeaponConfig 
                : _equippableWeaponConfig;

            _damageCollider.DamageInfo = _equippableWeaponConfig.DamageInfo;
            ChangeState(_startState);
            OnAnyStateEnter.AddListener(PlayAnimation);
            
            AddTransition(new [] { EquippableWeaponState.Attack }, EquippableWeaponState.Cooldown,
                new DelayCondition(_equippableWeaponConfig.AttackingTime));
            
            AddTransition(new [] { EquippableWeaponState.ChargeStart}, EquippableWeaponState.ChargeEnd,
                new DelayCondition(_equippableWeaponConfig.ChargeTime));
            
            AddTransition(new [] { EquippableWeaponState.Cooldown }, EquippableWeaponState.Idle,
                new DelayCondition(_equippableWeaponConfig.CooldownTime));
        }
        public void Equip()
        {
            gameObject.SetActive(true);
            _damageCollider.DamageInfo = _equippableWeaponConfig.DamageInfo;
            ChangeState(_startState);
            OnAnyStateEnter.AddListener(PlayAnimation);
            
            AddTransition(new [] { EquippableWeaponState.Attack }, EquippableWeaponState.Cooldown,
                new DelayCondition(_equippableWeaponConfig.AttackingTime));
            
            AddTransition(new [] { EquippableWeaponState.ChargeStart}, EquippableWeaponState.ChargeEnd,
                new DelayCondition(_equippableWeaponConfig.ChargeTime));
            
            AddTransition(new [] { EquippableWeaponState.Cooldown }, EquippableWeaponState.Idle,
                new DelayCondition(_equippableWeaponConfig.CooldownTime));
        }
        
        public void Unequip()
        {
            ChangeEquippableWeaponState(EquippableWeaponState.Idle);
            OnAnyStateEnter.RemoveListener(PlayAnimation);
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

        public void UseUpdate()
        {
            StartCoroutine(UseRoutine());
        }
        
        public void ChangeEquippableWeaponState(EquippableWeaponState equippableWeaponState) 
            => ChangeState(equippableWeaponState);

        
        public void PlayAnimation(EquippableWeaponState state)
        {
            var anim = _equippableWeaponConfig.GetAnimation(state);
            if(anim == null)
                return;
            
            _equippableWeaponView.PlayAnimation(anim.AnimationConfig);
        }
            
        #endregion

        #region Coroutines

        private IEnumerator UseRoutine()
        {
            while (true)
            {
                Use();
                yield return null;
            }
        }

        #endregion
    }
}