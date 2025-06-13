using AI.Condition;

namespace AI.FSM
{
    [System.Serializable]
    public class TransitionConfigData
    {
        public string name;
        public string fromState = "*"; // "*" means from any state
        public string toState;
        public int priority;
        public ConditionConfigData[] conditions;
        public LogicOperator logicOperator;
        public bool canInterruptSelf = false; // Allow transition to same state
    }
}