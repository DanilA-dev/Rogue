using System;
using UnityEngine;

namespace D_Dev.UtilScripts.ValueSystem
{
    public class BaseValue<T>
    {
        #region Fields

        [SerializeField] protected T _defaultValue;

        public Action<T> OnValueChanged;
        #endregion

        #region Properties

        public virtual T Value
        {
            get => _defaultValue;
            set
            {
                _defaultValue = value;
                OnValueChanged?.Invoke(_defaultValue);
            }
        }

        #endregion

       
    }
}