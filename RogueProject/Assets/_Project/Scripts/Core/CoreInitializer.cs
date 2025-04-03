using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Core
{
    public class CoreInitializer : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onStart;
        private void Start() => _onStart?.Invoke();
    }
}