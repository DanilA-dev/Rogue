namespace _Project.Scripts.Core.Enemies.States
{
    public class IdleEnemyState : BaseEnemyState
    {
        public IdleEnemyState(EnemyBehaviour enemyBehaviour) : base(enemyBehaviour) {}
        public override float ExitTime { get; }

        public override void OnEnter()
        {
            _enemyBehaviour.View.PlayIdleAnimation();
        }
    }
}