using System.Linq;

namespace D_Dev.Scripts.Runtime.UtilScripts.SimpleStateMachine
{
    public class GroupAndCondition : IStateCondition
    {
        private IStateCondition[] _conditions;

        public GroupAndCondition(IStateCondition[] conditions)
        {
            _conditions = conditions;
        }

        public bool IsMatched()
        {
            return _conditions.All(c => c.IsMatched());
        }
    }
}