using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using D_Dev.UtilScripts.ScriptableVaiables;
using D_Dev.UtilScripts.ValueSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Core.Combat
{
    public class CombatHandler : MonoBehaviour
    {
        #region Enums

        public enum CombatTurn
        {
            Player,
            Enemy
        }

        #endregion
        
        #region Fields

        [Title("Data")]
        //Make event driven
        [SerializeField] private GameObjectScriptableVariable[] _playerUnits;
        [SerializeField] private GameObjectScriptableVariable[] _enemyUnits;
        [Title("Camera Settings")]
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private CinemachineTargetGroup _targetGroup;
        [SerializeField] private int _cameraPriority;
        [Title("Positions")]
        [SerializeField] private Transform[] _playerUnitsPositions;
        [SerializeField] private Transform[] _enemyUnitsPositions;

        #endregion

        #region Properties

        public IntValue TurnIndex { get; private set; }

        public List<ICombatAction> CombatActions { get; private set; } = new();
        
        #endregion
        
        #region Public

        public void StartCombat()
        {
            SetCombatCamera();
            SetUnitsToPositions();
        }
        

        #endregion

        #region Private

        private void SetCombatCamera()
        {
            _camera.gameObject.SetActive(true);
            _camera.Priority = _cameraPriority;
            List<CinemachineTargetGroup.Target> targets = new();
            
            var transformTargets = _playerUnits.Select(playerUnitsPosition => playerUnitsPosition.Value.transform).ToList();
            transformTargets.AddRange(_enemyUnits.Select(gameObjectScriptableVariable => gameObjectScriptableVariable.Value.transform));
            targets.AddRange(transformTargets.Select(target => new CinemachineTargetGroup.Target {weight = 1, radius = 1, target = target}));
            _targetGroup.m_Targets = targets.ToArray();
        }

        private void SetUnitsToPositions()
        {
            for (var i = 0; i < _playerUnitsPositions.Length; i++)
            {
                for (var j = 0; j < _playerUnits.Length; j++)
                {
                    _playerUnits[j].Value.transform.SetParent(_playerUnitsPositions[i]);
                    _playerUnits[j].Value.transform.localPosition = Vector3.zero;
                    _playerUnits[j].Value.transform.localRotation = Quaternion.identity;
                }
            }

            for (var i = 0; i < _enemyUnitsPositions.Length; i++)
            {
                for (var j = 0; j < _enemyUnits.Length; j++)
                {
                    _enemyUnits[j].Value.transform.SetParent(_enemyUnitsPositions[i]);
                    _enemyUnits[j].Value.transform.localPosition = Vector3.zero;
                    _enemyUnits[j].Value.transform.localRotation = Quaternion.identity;
                }
            }
        }

        #endregion
    }
}