using _Project.Scripts.Core.Player.States;
using _Project.Scripts.Core.Weapon;
using D_dev.Scripts.EventHandler;
using D_Dev.Scripts.Runtime.UtilScripts.SimpleStateMachine;
using D_Dev.Scripts.Runtime.UtilScripts.StateMachineBehaviour;
using D_Dev.UtilScripts.DamagableSystem;
using D_Dev.UtilScripts.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Core.Player
{
    public enum PlayerState
    {
        Idle = 0,
        Move = 1,
        Combat = 2,
        Dead = 3,
    }
    
    public class PlayerControllerBehaviour : StateMachineBehaviour<PlayerState>
    {
        #region Fields

        [SerializeField] private DamagableObject _damagableObject;
        [SerializeField] private WeaponHolder _weaponHolder;
        [Title("View")]
        [SerializeField] private PlayerView _view;
        [SerializeField] private Rigidbody _rigidbody;
        [FoldoutGroup("Move State")] 
        [SerializeField] private float _rotateMoveSpeed;
        [FoldoutGroup("AimMove State")]
        [SerializeField] private float _rotateAimSpeed;
        
        private IMover _mover;
        private Vector3 _movementVelocity;

        #endregion

        #region Properties

        public IMover Mover => _mover;
        public float RotateMoveSpeed => _rotateMoveSpeed;
        public float RotateAimSpeed => _rotateAimSpeed;
        public PlayerView View => _view;
        public WeaponHolder WeaponHolder => _weaponHolder;

        #endregion
        
        #region Monobehaviour

        private void OnEnable()
        {
            TryGetComponent(out _mover);
            _damagableObject.OnDeath.AddListener((() => ChangeState(PlayerState.Dead)));
            
            CustomEventHandler.AddListener(CustomEventType.CombatStart, () => ChangeState(PlayerState.Combat));
        }
      
        private void OnDisable()
        {
            _damagableObject.OnDeath.RemoveListener((() => ChangeState(PlayerState.Dead)));
            CustomEventHandler.RemoveListener(CustomEventType.CombatStart, () => ChangeState(PlayerState.Combat));
        }

        #endregion
        
        #region Overrides

        protected override void InitStates()
        {
            AddState(PlayerState.Idle, new IdlePlayerState(this));
            AddState(PlayerState.Move, new MovePlayerState(this));
            AddState(PlayerState.Dead, new DeadPlayerState(this));
            AddState(PlayerState.Combat, new CombatPlayerState(this));
            ChangeState(_startState);
            
            AddTransition(new [] { PlayerState.Idle }, PlayerState.Move, new FuncCondition(IsMoving));
            AddTransition(new [] { PlayerState.Move }, PlayerState.Idle, new FuncCondition(() => !IsMoving()));
        }

        #endregion

        #region Public

        public void RotateTowards(Vector3 dir, Vector3 upwards, float speed,
            bool constrainX = false, bool constrainY = false, bool constrainZ = false)
        {
            if(_mover.Target != Vector3.zero)
                transform.RotateTowards(dir, upwards,
                    speed * Time.deltaTime, constrainX, constrainY, constrainZ);
        }

        #endregion
        
        #region Helpers

        private bool IsMoving() => _mover != null && _mover.Target != Vector3.zero;


        #endregion
    }
}