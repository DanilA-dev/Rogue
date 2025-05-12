using D_Dev.UtilScripts.ScriptableVaiables;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class SliderStatItem : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Slider _slider;
        [SerializeField] private FloatScriptableVariable _variable;

        #endregion

        #region Monobehaviour

        private void OnEnable() => _variable.OnVariableUpdate += OnVariableValueUpdate;

        private void Start()
        {
            if (_variable.Value != 0)
            {
                _slider.maxValue = _variable.Value;
                _slider.value = _slider.maxValue;
            }
        }

        private void OnDisable() => _variable.OnVariableUpdate -= OnVariableValueUpdate;

        #endregion

        #region Listeners

        private void OnVariableValueUpdate(float value) => _slider.value = value;

        #endregion
    }
}