using _Project.Scripts.Core.Player.States;
using D_Dev.Scripts.Runtime.UtilScripts.SimpleStateMachine;
using D_Dev.Scripts.Runtime.UtilScripts.StateMachineBehaviour;
using D_Dev.Scripts.Runtime.UtilScripts.TargetSensor;
using D_Dev.UtilScripts.Extensions;
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

        [Title("View")]
        [SerializeField] private PlayerView _view;
        [Title("Sensors")]
        [SerializeField] private TargetSensor _targetSensor;
        [FoldoutGroup("Move State")] 
        [SerializeField] private float _rotateMoveSpeed;
        [FoldoutGroup("AimMove State")]
        [SerializeField] private float _rotateAimSpeed;
        
        private IMover _mover;
        private Vector3 _movementVelocity;

        #endregion

        #region Properties

        public IMover Mover => _mover;
        public PlayerView View => _view;
        public TargetSensor Sensor => _targetSensor;
        public float RotateMoveSpeed => _rotateMoveSpeed;
        public float RotateAimSpeed => _rotateAimSpeed;
        public Vector3 MovementVelocity => _movementVelocity;
        public Vector3 RotationDirection { get; set; }

        #endregion
        
        #region Monobehaviour

        private void OnEnable()
        {
            _targetSensor.Init();
            if (TryGetComponent(out _mover))
                _mover.OnMove += (vel) => _movementVelocity = vel;
        }

        private void OnDisable()
        {
            _targetSensor.Dispose();
            if (_mover != null)
                _mover.OnMove -= (vel) => _movementVelocity = vel;
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
                
            }, PlayerState.AimMove, new FuncCondition(IsEnemyNearby));
            AddTransition(new [] { PlayerState.AimMove }, PlayerState.Idle, new FuncCondition(() => !IsEnemyNearby()));
        }

        #endregion

        #region Public

        public void RotateTowards(Vector3 dir, Vector3 upwards, float speed,
            bool constrainX = false, bool constrainY = false, bool constrainZ = false)
        {
            if(MovementVelocity != Vector3.zero)
                transform.RotateTowards(dir, upwards,
                    speed * Time.deltaTime, constrainX, constrainY, constrainZ);
        }

        #endregion
        
        #region Helpers

        private bool IsMoving() => _movementVelocity != Vector3.zero;
        private bool IsEnemyNearby() => _targetSensor.Trigger.Colliders.Count > 0
                                        && _targetSensor.IsTargetFound(out var c);

        #endregion


    }
}