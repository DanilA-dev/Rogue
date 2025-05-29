﻿using System;
using UnityEngine;

namespace D_Dev.UtilScripts.ScriptableVaiables
{
    public class BaseScriptableVariable<T> : ScriptableObject
    {
        #region Fields

        [SerializeField] private T _value;

        public event Action<T> OnValueUpdate;
        
        #endregion

        #region Properties

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnValueUpdate?.Invoke(_value);
            }
        }

        #endregion
    }
}