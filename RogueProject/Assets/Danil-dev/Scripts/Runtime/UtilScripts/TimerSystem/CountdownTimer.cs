namespace D_Dev.UtilScripts.TimerSystem
{
    public class CountdownTimer : BaseTimer
    {
        #region Properties
        public float RemainingTime => Time;

        #endregion

        #region Construct
        public CountdownTimer(float initialTime) : base(initialTime) {}

        #endregion

        #region Override

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            if (IsRunning && Time > 0)
                Time -= deltaTime;
      
            if(IsRunning && Time <= 0)
                Stop();
        }

        public override void Reset() => Time = _initialTime;

        #endregion

        #region Public

        public void Reset(float newInitTime)
        {
            base.Reset();
            _initialTime = newInitTime;
            Reset();
        }

        #endregion
    }
}