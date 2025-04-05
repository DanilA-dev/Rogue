using D_Dev.UtilScripts.MenuHandler;
using D_Dev.UtilScripts.ScriptableVaiables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class CoreMenu : BaseMenu
    {
        #region Fields

        [Title("Data")]
        [SerializeField] private FloatScriptableVariable _playerHealthVariable;
        [Title("UI Elements")]
        [SerializeField] private Slider _playerHealthSlider;

        #endregion

        #region Monobehaviour

        private void OnEnable() => _playerHealthVariable.OnVariableUpdate += OnPlayerHealthUpdate;

        private void Start()
        {
            if (_playerHealthVariable.Variable != 0)
            {
               _playerHealthSlider.maxValue = _playerHealthVariable.Variable;
               _playerHealthSlider.value = _playerHealthSlider.maxValue;
            }
        }

        private void OnDisable() => _playerHealthVariable.OnVariableUpdate -= OnPlayerHealthUpdate;

        #endregion

        #region Listeners

        private void OnPlayerHealthUpdate(float value)
        {
            var sliderValue = value;
            _playerHealthSlider.value = sliderValue;
        }

        #endregion
    }
}