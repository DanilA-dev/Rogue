using UnityEngine;
using Danil_dev.Scripts.Runtime.UtilScripts.InputSystem;
using Sirenix.OdinInspector;

namespace _Project.Scripts.Core
{
    public class InputMoverReceiver : MonoBehaviour
    {
        #region Fields

        [SerializeField] private InputRouter _router;

        private IMover _mover;


        #endregion

        #region Properties

        [ShowInInspector, DisplayAsString]
        public Vector3 MovementDirection { get; private set; }

        #endregion

        #region Monobehavior

        private void Awake() => TryGetComponent(out _mover);

        private void OnEnable()
        {
            _router.Enable();
            _router.Move += (direction) =>
            {
                var movement = new Vector3(direction.x, 0, direction.y);
                MovementDirection = movement;
            };
        }

        private void OnDisable()
        {
            _router.Disable();
            _router.Move -= (direction) =>
            {
                var movement = new Vector3(direction.x, 0, direction.y);
                MovementDirection = movement;
            };
        }

        private void Update()
        {
            _mover?.Move(MovementDirection);
        }

        #endregion
    }
}