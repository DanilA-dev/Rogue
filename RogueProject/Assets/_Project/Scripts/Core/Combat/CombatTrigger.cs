using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Core.Characters;
using Cinemachine;
using D_dev.Scripts.EventHandler;
using D_Dev.UtilScripts.ColliderEvents;
using UnityEngine;

namespace _Project.Scripts.Core.Combat
{
    public class CombatTrigger : MonoBehaviour
    {
        #region Fields
        
        [SerializeField] private ActionCharacter[] _enemiesCharacters;
        [SerializeField] private CinemachineVirtualCamera _combatTriggerCamera;
        [SerializeField] private CinemachineTargetGroup _combatTriggerGroup;
        [SerializeField] private int _cameraPriority;
        [SerializeField] private TriggerColliderEvents _trigger;

        private Transform _mainPlayerUnit;
            
        #endregion

        #region Monobehaviour
        private void Awake() => _trigger.OnEnter.AddListener(OnCombatTriggerEnter);
        private void OnDestroy() => _trigger.OnEnter.RemoveListener(OnCombatTriggerEnter);

        #endregion

        #region Private

        private void SetCamera()
        {
            _combatTriggerCamera.gameObject.SetActive(true);
            _combatTriggerCamera.Priority = _cameraPriority;
            List<CinemachineTargetGroup.Target> targets = new();
            List<Transform> targetTransforms = new() { _mainPlayerUnit };
            
            targetTransforms.AddRange(_enemiesCharacters.Select(gameObjectScriptableVariable => gameObjectScriptableVariable.transform));
            targets.AddRange(targetTransforms.Select(target => new CinemachineTargetGroup.Target {weight = 1, radius = 1, target = target}));
            _combatTriggerGroup.m_Targets = targets.ToArray();
        }

        #endregion

        #region Listeners
        private void OnCombatTriggerEnter(Collider collider)
        {
            _mainPlayerUnit = collider.gameObject.transform;
            
            SetCamera();
            CustomEventHandler.Invoke(CustomEventType.CombatTriggered);
        }
        
        #endregion
    }
}