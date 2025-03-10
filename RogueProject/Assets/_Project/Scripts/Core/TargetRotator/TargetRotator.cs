using System.Collections.Generic;
using System.Linq;
using D_Dev.UtilScripts.ColliderEvents;
using D_Dev.UtilScripts.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Core.TargetRotator
{
    public class TargetRotator : MonoBehaviour
    {
        #region Fields

        [Title("Triggers")]
        [SerializeField, ReadOnly] private List<Transform> _targets = new();
        [SerializeField] private TriggerColliderEvents _trigger;
        [Title("Rotate settings")] 
        [SerializeField] private Transform _moverRoot;
        [SerializeField] private float _rotateSpeed;

        private IMover _mover;
        
        #endregion

        #region Monobehaviour

        private void Awake()
        {
            _moverRoot.TryGetComponent(out _mover);
        }

        private void OnEnable()
        {
            _trigger.OnEnter.AddListener(OnTargetEnter);
            _trigger.OnExit.AddListener(OnTargetExit);
        }

        private void OnDisable()
        {
            _trigger.OnEnter.RemoveListener(OnTargetEnter);
            _trigger.OnExit.RemoveListener(OnTargetExit);
        }

        private void LateUpdate()
        {
            if(_targets.Count <= 0)
                return;
            
            var minDistance = _targets.Min(t => Vector3.Distance(t.position, transform.position));
            foreach (var target in _targets)
            {
                if (Vector3.Distance(transform.position, target.position) <= minDistance)
                    transform.RotateTowardsDirection(target.position, Vector3.up, _rotateSpeed * Time.deltaTime);
            }
        }

        #endregion

        #region Listeners

        private void OnTargetEnter(Collider targetCollider)
        {
            if(!_targets.Contains(targetCollider.transform))
                _targets.Add(targetCollider.transform);
            
            if(_targets.Count > 0 && _mover != null)
                _mover.EnableDirectionRotation = false;
        }
        
        private void OnTargetExit(Collider targetCollider)
        {
            _targets.TryRemove(targetCollider.transform);
            if(_targets.Count <= 0 && _mover != null)
                _mover.EnableDirectionRotation = true;
        }

        #endregion
        
        
        
    }
}