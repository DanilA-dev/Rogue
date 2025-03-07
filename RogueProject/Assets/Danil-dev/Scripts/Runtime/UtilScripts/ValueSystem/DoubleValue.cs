using UnityEngine;

namespace D_Dev.UtilScripts.ValueSystem
{
    public class DoubleValue : BaseValue<double>
    {
        public override double Value
        {
            get => _defaultValue;
            set
            {
                _defaultValue = Mathf.Clamp((float)value, 0, Mathf.Infinity);
                OnValueChanged?.Invoke(_defaultValue);
            }
        }
    }
}