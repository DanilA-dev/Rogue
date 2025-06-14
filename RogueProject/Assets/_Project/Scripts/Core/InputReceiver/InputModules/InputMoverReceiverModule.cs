using UnityEngine;
using Sirenix.OdinInspector;

namespace _Project.Scripts.Core
{
    [System.Serializable]
    public class InputMoverReceiverModule : BaseInputReceiverModule
    {
        #region Fields

        private IMover _mover;

        #endregion

        #region Properties

        [ShowInInspector, DisplayAsString]
        public Vector3 MovementDirection { get; private set; }

        #endregion

        #region Overrides

        public override void OnInit()
        {
            var root = _baseTransform;
            _mover = root.GetComponentInChildren<IMover>();
        }

        public override void OnInputEnable()
        {
            if(_mover == null)
                return;
    
            _inputRouter.Move += OnMove;
        }

        public override void OnInputDisable()
        {
            if(_mover == null)
                return;
    
            _inputRouter.Move -= OnMove;
        }

        private void OnMove(Vector2 direction)
        {
            var movement = new Vector3(direction.x, 0, direction.y);
            _mover.Target = movement;
            MovementDirection = movement;
        }

            
        #endregion
    }
}