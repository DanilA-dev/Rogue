namespace _Project.Scripts.Core.Enemies.States
{
    public class DeadEnemyState : BaseEnemyState
    {
        public DeadEnemyState(EnemyBehaviour enemyBehaviour) : base(enemyBehaviour) {}
        public override float ExitTime { get; }

        public override void OnEnter()
        {
            _enemyBehaviour.View.PlayDeathAnimation();
        }
    }
}