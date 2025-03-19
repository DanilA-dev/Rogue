using System;
using D_Dev.Scripts.Runtime.UtilScripts.StateMachineBehaviour;
using _Project.Scripts.Core.Enemies.States;
using D_Dev.Scripts.Runtime.UtilScripts.SimpleStateMachine;
using Danil_dev.Scripts.Runtime.UtilScripts.TargetSensor;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Core.Enemies
{
    public enum EnemyState
    {
        Idle = 0,
        Patrol = 1,
        ChasePlayer = 2,
        Attack = 3,
        Retreat = 4,
        Dead = 5
    }

    public class EnemyBehaviour : StateMachineBehaviour<EnemyState>
    {
        #region Fields

        [Title("General Settings")] 
        [SerializeField] private TargetSensor _enemyVision;
        [SerializeField] private float _stoppingDistance;
        [Title("Idle Settings")] 
        [SerializeField] protected float _idleTime;
        [Title("Patrol Settings")]
        [SerializeField] private float _patrolMovementSpeed;
        [SerializeField] private Transform[] _patrolPoints;
        [Title("Chase Player Settings")]
        [SerializeField] private float _chaseMovementSpeed;
        
        private IMover _enemyMover;

        #endregion

        #region Properties

        public Transform[] PatrolPoints => _patrolPoints;
        public int PatrolPointIndex { get; set; }

        public float PatrolMovementSpeed
        {
            get => _patrolMovementSpeed;
            set => _patrolMovementSpeed = value;
        }
        protected float IdleTime
        {
            get => _idleTime;
            set => _idleTime = value;
        }

        public TargetSensor Vision => _enemyVision;

        public IMover EnemyMover => _enemyMover;

        public float ChaseMovementSpeed
        {
            get => _chaseMovementSpeed;
            set => _chaseMovementSpeed = value;
        }

        public float StoppingDistance
        {
            get => _stoppingDistance;
            set => _stoppingDistance = value;
        }

        #endregion

        #region Monobehaviour

        private void OnEnable() => _enemyVision.Init();

        private void OnDisable() => _enemyVision.Dispose();

        #endregion
        
        #region Overrides
        protected override void InitStates()
        {
            _enemyMover = GetComponent<IMover>();
            
            AddState(EnemyState.Idle, new IdleEnemyState(this));
            AddState(EnemyState.Patrol, new PatrolEnemyState(this));
            AddState(EnemyState.ChasePlayer, new ChasePlayerEnemyState(this));
            AddState(EnemyState.Attack, new AttackEnemyState(this));
            AddState(EnemyState.Retreat, new RetreatEnemyState(this));
            AddState(EnemyState.Dead, new DeadEnemyState(this));

            InitTransitions();
            ChangeState(_startState);
        }
        
        #endregion

        #region Private

        private void InitTransitions()
        {
            AddTransition(new[] { EnemyState.Idle }, EnemyState.Patrol, new DelayCondition(_idleTime));
            AddTransition(new [] { EnemyState.Patrol}, EnemyState.Idle, new FuncCondition(() 
                => IsTargetReached(PatrolPoints[PatrolPointIndex].position, _stoppingDistance)));
            
            AddTransition(new []
            {
                EnemyState.Idle,
                EnemyState.Patrol,
                
            }, EnemyState.ChasePlayer, new FuncCondition(IsTargetFound));
            
            AddTransition(new []
            {
                EnemyState.ChasePlayer,
                EnemyState.Attack,
                
            }, EnemyState.Idle, new FuncCondition(() => !IsTargetFound()));
        }
        
        private bool IsTargetFound() 
            => _enemyVision.IsTargetFound(out var c);

        #endregion
        
        #region Public
        public bool IsTargetReached(Vector3 targetPosition, float distance)
            => Vector3.Distance(transform.position, targetPosition) < distance;

        #endregion
    }
}