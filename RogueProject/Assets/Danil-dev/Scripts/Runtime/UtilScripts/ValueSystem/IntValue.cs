using UnityEngine;

namespace D_Dev.UtilScripts.ValueSystem
{
    [System.Serializable]
    public class IntValue : BaseValue<int>
    {
        public override int Value
        {
            get => _defaultValue;
            set
            {
                _defaultValue = (int)Mathf.Clamp(value, 0, Mathf.Infinity);
                OnValueChanged?.Invoke(_defaultValue);
            }
        }
    }
   
}