using _Project.Scripts.Core.Player.States;
using D_Dev.Scripts.Runtime.UtilScripts.SimpleStateMachine;
using D_Dev.Scripts.Runtime.UtilScripts.StateMachineBehaviour;
using D_Dev.Scripts.Runtime.UtilScripts.TargetSensor;
using D_Dev.UtilScripts.DamagableSystem;
using D_Dev.UtilScripts.Extensions;
using D_Dev.UtilScripts.ScriptableVaiables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Core.Player
{
    public enum PlayerState
    {
        Idle = 0,
        Move = 1,
        AimMove = 2,
        Dead = 3,
    }
    
    public class PlayerControllerBehaviour : StateMachineBehaviour<PlayerState>
    {
        #region Fields

        [SerializeField] private DamagableObject _damagableObject;
        [Title("View")]
        [SerializeField] private PlayerView _view;
        [SerializeField] private Rigidbody _rigidbody;
        [Title("Sensors")]
        [SerializeField] private TargetSensor _targetSensor;
        [SerializeField] private GameObjectScriptableVariable _targetVariable;
        [SerializeField] private Vector3 _targetObjectPositionOffset;
        [Space]
        [FoldoutGroup("Move State")] 
        [SerializeField] private float _rotateMoveSpeed;
        [FoldoutGroup("AimMove State")]
        [SerializeField] private float _rotateAimSpeed;
        
        private IMover _mover;
        private Vector3 _movementVelocity;

        #endregion

        #region Properties

        public IMover Mover => _mover;
        public TargetSensor Sensor => _targetSensor;
        public float RotateMoveSpeed => _rotateMoveSpeed;
        public float RotateAimSpeed => _rotateAimSpeed;
        public GameObjectScriptableVariable TargetVariable => _targetVariable;
        public Vector3 TargetObjectPositionOffset => _targetObjectPositionOffset;
        public PlayerView View => _view;

        #endregion
        
        #region Monobehaviour

        private void OnEnable()
        {
            TryGetComponent(out _mover);
            _targetSensor.Init();
            _damagableObject.OnDeath.AddListener((() => ChangeState(PlayerState.Dead)));
        }
      
        private void OnDisable()
        {
            _targetSensor.Dispose();
            _damagableObject.OnDeath.RemoveListener((() => ChangeState(PlayerState.Dead)));
        }

        #endregion
        
        #region Overrides

        protected override void InitStates()
        {
            AddState(PlayerState.Idle, new IdlePlayerState(this));
            AddState(PlayerState.Move, new MovePlayerState(this));
            AddState(PlayerState.AimMove, new AimMovePlayerState(this));
            AddState(PlayerState.Dead, new DeadPlayerState(this));
            ChangeState(_startState);
            
            AddTransition(new [] { PlayerState.Idle }, PlayerState.Move, new FuncCondition(IsMoving));
            AddTransition(new [] { PlayerState.Move }, PlayerState.Idle, new FuncCondition(() => !IsMoving()));
            
            AddTransition(new []
            {
                PlayerState.Idle,
                PlayerState.Move,
                
            }, PlayerState.AimMove, new FuncCondition(IsTargetNearby));
            
            AddTransition(new [] { PlayerState.AimMove }, PlayerState.Idle, new FuncCondition(() => !IsTargetNearby()));
        }

        #endregion

        #region Public

        public void RotateTowards(Vector3 dir, Vector3 upwards, float speed,
            bool constrainX = false, bool constrainY = false, bool constrainZ = false)
        {
            if(_mover.Velocity != Vector3.zero)
                transform.RotateTowards(dir, upwards,
                    speed * Time.deltaTime, constrainX, constrainY, constrainZ);
        }

        #endregion
        
        #region Helpers

        private bool IsMoving() => _mover != null && _mover.Velocity != Vector3.zero;

        private bool IsTargetNearby()
        {
            return _targetSensor.Trigger.Colliders.Count > 0
                   && _targetSensor.IsTargetFound();
        }

        #endregion


    }
}