namespace _Project.Scripts.Core.Enemies.States
{
    public class RetreatEnemyState : BaseEnemyState
    {
        public RetreatEnemyState(EnemyBehaviour enemyBehaviour) : base(enemyBehaviour)
        {
        }

        public override float ExitTime { get; }
    }
}