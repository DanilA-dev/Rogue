using System.Collections.Generic;
using Danil_dev.Scripts.Runtime.UtilScripts.InputSystem;
using UnityEngine;

namespace _Project.Scripts.Core.InputReceiver
{
    public class MainInputReceiver : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Transform _root;
        [SerializeField] private InputRouter _inputRouter;
        [SerializeReference] private List<BaseInputReceiverModule> _inputReceiverModules;

        private bool _isModulesInitialized;

        #endregion

        #region Monobehaviour

        private void Awake() => InitModules();

        private void OnEnable()
        {
            _inputRouter?.Enable();
            OnInputModulesEnable();
        }
        private void OnDisable()
        {
            _inputRouter?.Disable();
            OnInputModulesDisable();
        }
        private void Update() => OnInputModulesUpdate();

        #endregion

        #region Private

        private void InitModules()
        {
            if(_inputReceiverModules == null 
               || _inputReceiverModules.Count == 0
               || _inputRouter == null)
                return;

            foreach (var inputModule in _inputReceiverModules)
                inputModule.Init(_inputRouter, _root);

            _isModulesInitialized = true;
        }
        
        private void OnInputModulesEnable()
        {
            if(!_isModulesInitialized)
                return;
            
            foreach (var inputModule in _inputReceiverModules)
                inputModule.OnInputEnable();
        }
        
        private void OnInputModulesDisable()
        {
            if(!_isModulesInitialized)
                return;
            
            foreach (var inputModule in _inputReceiverModules)
                inputModule.OnInputDisable();
        }
        
        private void OnInputModulesUpdate()
        {
            if(!_isModulesInitialized)
                return;
            
            foreach (var inputModule in _inputReceiverModules)
                inputModule.OnUpdate();
        }

        #endregion
    }
}