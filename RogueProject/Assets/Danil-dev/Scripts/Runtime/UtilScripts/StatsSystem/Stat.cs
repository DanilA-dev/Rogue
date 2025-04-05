﻿using D_Dev.UtilScripts.ScriptableVaiables;
using D_Dev.UtilScripts.ValueSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.UtilScripts.StatsSystem
{
    [System.Serializable]
    public class Stat
    {
        #region Fields

        [field: SerializeField] public StringScriptableVariable StatName { get; private set; }
        [field: SerializeField] public FloatValue StatValue { get; private set; }

        [FoldoutGroup("Events")]
        public UnityEvent<float> OnStatInit;
        [FoldoutGroup("Events")]
        public UnityEvent<float> OnStatUpdate;

        #endregion

        #region Properties

        public bool IsInitialized { get; private set; }

        #endregion

        #region Public

        public void Init()
        {
            if(IsInitialized)
                return;
            
            OnStatInit?.Invoke(StatValue.Value);
            StatValue.OnValueChanged += UpdateStat;
            IsInitialized = true;
        }

        public void Dispose()
        {
            StatValue.OnValueChanged -= UpdateStat;
            IsInitialized = false;
        }

        #endregion

        #region Listeners

        private void UpdateStat(float value) => OnStatUpdate?.Invoke(value);

        #endregion
    }
}