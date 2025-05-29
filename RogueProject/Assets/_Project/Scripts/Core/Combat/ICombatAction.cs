namespace _Project.Scripts.Core.Combat
{
    public interface ICombatAction : IAction
    {
        public int ActionPoints { get; set; }
    }
}