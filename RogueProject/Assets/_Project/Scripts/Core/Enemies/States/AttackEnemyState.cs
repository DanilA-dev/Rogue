namespace _Project.Scripts.Core.Enemies.States
{
    public class AttackEnemyState : BaseEnemyState
    {
        public AttackEnemyState(EnemyBehaviour enemyBehaviour) : base(enemyBehaviour) {}

        public override float ExitTime { get; }
    }
}