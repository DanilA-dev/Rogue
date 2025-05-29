using D_Dev.UtilScripts.ScriptableVaiables;
using D_Dev.UtilScripts.TimerSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.UtilScripts.StatsSystem
{
    [System.Serializable]
    public class StatRegenConfig
    {
        #region Fields

        [SerializeField] private StringScriptableVariable _statName;
        [SerializeField] private bool _autoRegenerate;
        [SerializeField] private float _delayAfterStatDecrease;
        [SerializeField] private float _regenerationRate;
        [SerializeField] private float _regenerationTime;
        [SerializeField] private float _regenerateValue;
        [Space]
        [FoldoutGroup("Events")]
        public UnityEvent<StatRegenConfig> OnRegenerateStart;
        [FoldoutGroup("Events")]
        public UnityEvent<StatRegenConfig> OnRegenerateInterrupt;
        [FoldoutGroup("Events")]
        public UnityEvent<StatRegenConfig> OnRegenerateEnd;

        private Stat _stat;
        private bool _isRegenerating = false;
        private float _initialStatValue;
        private float _oldStatValue;
        private float _newStatValue;
        
        private CountdownTimer _decreaseDelayTimer;
        private IncrementTimer _regenerateTimer;
        
        #endregion

        #region Properties
            
        public StringScriptableVariable StatName
        {
            get => _statName;
            set => _statName = value;
        }

        public float RegenerationRate
        {
            get => _regenerationRate;
            set => _regenerationRate = value;
        }

        public float RegenerateValue
        {
            get => _regenerateValue;
            set => _regenerateValue = value;
        }

        public bool AutoRegenerate
        {
            get => _autoRegenerate;
            set => _autoRegenerate = value;
        }

        public float DelayAfterStatDecrease
        {
            get => _delayAfterStatDecrease;
            set => _delayAfterStatDecrease = value;
        }

        public Coroutine RegenerationRoutine { get; private set; }
        
        #endregion

        #region Public

        public void GetStat(StatsContainer statsContainer)
        {
            _stat = statsContainer.GetStat(_statName);
            if (_stat != null)
            {
                _decreaseDelayTimer = new CountdownTimer(_delayAfterStatDecrease);
                _regenerateTimer = new IncrementTimer(0, _regenerationTime);
                _decreaseDelayTimer.OnTimerEnd += StartRegen;
                _regenerateTimer.OnTimerProgressUpdate += OnRegenerateProcess;
                _regenerateTimer.OnTimerEnd += EndRegenerate;
                _initialStatValue = _stat.StatValue.Value;
                _stat.OnStatUpdate.AddListener(OnStatUpdate);
            }
        }

       
        public void Tick()
        {
            _decreaseDelayTimer?.Tick(Time.deltaTime);
            _regenerateTimer?.Tick(Time.deltaTime);
        }
        
        public void DisposeStatRegen()
        {
            if(_stat == null)
                return;
            
            _stat.OnStatUpdate.RemoveListener(OnStatUpdate);
            _decreaseDelayTimer.OnTimerEnd -= StartRegen;
            _regenerateTimer.OnTimerProgressUpdate -= OnRegenerateProcess;
            _regenerateTimer.OnTimerEnd -= EndRegenerate;
        }

        #endregion

        #region Listiners

        private void OnStatUpdate(float statValue)
        {
            if(_initialStatValue >= statValue || _newStatValue >= statValue)
                return;

            _oldStatValue = _initialStatValue;
            _newStatValue = statValue;

            if (_newStatValue < _oldStatValue)
                InterruptRegen();
        }

        #endregion

        #region Private

        private void InterruptRegen()
        {
            _isRegenerating = false;
            if(!_decreaseDelayTimer.IsRunning)
                _decreaseDelayTimer.Start();
            else
            {
                _decreaseDelayTimer.Stop();
                _decreaseDelayTimer.Start();
            }
            OnRegenerateInterrupt?.Invoke(this);
        }
        
        private void StartRegen()
        {
            if (_isRegenerating)
                return;
            
            _isRegenerating = true;
            _regenerateTimer?.Start();
            OnRegenerateStart?.Invoke(this);
        }
        
        private void OnRegenerateProcess(float timeValue)
        {
            float regenerationAmount = _regenerateValue * Time.deltaTime / _regenerationRate;
            _stat.StatValue.Value += regenerationAmount;
        }

        
        private void EndRegenerate()
        {
            _isRegenerating = false;
            OnRegenerateEnd?.Invoke(this);
        }

        #endregion
    }
}