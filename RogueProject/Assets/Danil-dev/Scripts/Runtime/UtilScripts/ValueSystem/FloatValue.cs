using UnityEngine;

namespace D_Dev.UtilScripts.ValueSystem
{
    [System.Serializable]
    public class FloatValue : BaseValue<float>
    {
        public override float Value
        {
            get => _defaultValue;
            set
            {
                _defaultValue = Mathf.Clamp(value, 0, Mathf.Infinity);
                OnValueChanged?.Invoke(_defaultValue);
            }
        }
    }
    
}