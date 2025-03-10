using UnityEngine;

namespace _Project.Scripts.Core.Enemies
{
    public class EnemyStateDebugger : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TextMesh _text;
        [SerializeField] private EnemyBehaviour _enemyBehaviour;
        
        private Camera _camera;

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _camera = Camera.main;
            _enemyBehaviour.OnStateEnter.AddListener(OnStateEnter);
        }
        private void OnDisable()
        {
            _enemyBehaviour.OnStateEnter.RemoveListener(OnStateEnter);
        }

        private void LateUpdate()
        {
            transform.LookAt(_camera.transform.up
                , Vector3.up);
        }

        #endregion

        #region Listeners

        private void OnStateEnter(EnemyState state)
        {
            _text.text = state.ToString();
        }

        #endregion
    }
}