using Danil_dev.Scripts.Runtime.UtilScripts.InputSystem;
using UnityEngine;

namespace _Project.Scripts.Core
{
    [System.Serializable]
    public abstract class BaseInputReceiverModule
    {
        #region Fields

        protected Transform _baseTransform;
        protected InputRouter _inputRouter;

        #endregion

        #region Public

        public void Init(InputRouter inputRouter, Transform baseTransform)
        {
            _inputRouter = inputRouter;
            _baseTransform = baseTransform;
            OnInit();
        }
        
        public virtual void OnInit() {}
        public abstract void OnInputEnable();
        public abstract void OnInputDisable();
        public virtual void OnUpdate() {}

        #endregion
    }
}