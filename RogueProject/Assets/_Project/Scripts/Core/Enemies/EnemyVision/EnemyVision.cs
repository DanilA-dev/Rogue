using D_Dev.UtilScripts.ColliderEvents;
using D_Dev.UtilScripts.Raycaster;
using UnityEngine;

namespace _Project.Scripts.Core.Enemies.EnemyVision
{
    public class EnemyVision : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TriggerColliderEvents _trigger;
        [SerializeField] private Linecaster _obstacleLinecaster;

        private RaycastPoint _targetRayPoint;
        
        #endregion

        #region Properties

        public Collider Target { get; private set; }
        public bool IsTargetVisible { get; private set; }

        #endregion
        
        #region Monobehaviour

        private void OnEnable()
        {
            _targetRayPoint = new();
            _trigger.OnEnter.AddListener(OnTargetEnter);
            _trigger.OnExit.AddListener(OnTargetExit);
        }

        private void OnDisable()
        {
            _trigger.OnEnter.RemoveListener(OnTargetEnter);
            _trigger.OnExit.RemoveListener(OnTargetExit);
        }

        private void FixedUpdate()
        {
            if(!Target)
                return;
         
            IsTargetVisible = _obstacleLinecaster.IsIntersect();
        }

        #endregion

        #region Public

        public bool IsTargetFound(out Collider target)
        {
            target = Target;
            return target && IsTargetVisible;
        }

        #endregion
        
        #region Listeners

        private void OnTargetEnter(Collider target)
        {
            Target = target;
            _targetRayPoint.PointType = RaycastPointType.Transform;
            _targetRayPoint.RaycastTransformPoint = Target.transform;
            _obstacleLinecaster.Direction = _targetRayPoint;
        }

        private void OnTargetExit(Collider target) => Target = null;

        #endregion
       
    }
}