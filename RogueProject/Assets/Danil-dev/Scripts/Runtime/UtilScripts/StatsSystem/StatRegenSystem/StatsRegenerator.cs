using System;
using System.Collections.Generic;
using UnityEngine;

namespace D_Dev.UtilScripts.StatsSystem
{
    public class StatsRegenerator : MonoBehaviour
    {
        #region Fields

        [SerializeField] private StatsContainer _statsContainer;
        [SerializeField] private bool _allowRegeneration = true;
        [SerializeField] private List<StatRegenConfig> _regenConfigs = new();

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            if (_regenConfigs.Count <= 0)
                return;

            foreach (var regenConfig in _regenConfigs)
                regenConfig.GetStat(_statsContainer);
        }

        private void Update()
        {
            if (_allowRegeneration)
                TickRegenConfigs();
        }

        private void TickRegenConfigs()
        {
            if (_regenConfigs.Count <= 0)
                return;

            foreach (var regenConfig in _regenConfigs)
                regenConfig.Tick();
        }

        #endregion

        #region Public

        public void AddRegenConfig(StatRegenConfig regenConfig)
        {
            if (!_regenConfigs.Contains(regenConfig))
            {
                _regenConfigs.Add(regenConfig);
                regenConfig.GetStat(_statsContainer);
            }
        }
        
        public void RemoveRegenConfig(StatRegenConfig regenConfig)
        {
            if (_regenConfigs.Contains(regenConfig))
            {
                _regenConfigs.Remove(regenConfig);
                regenConfig.DisposeStatRegen();
            }
        }

        #endregion
    }
}