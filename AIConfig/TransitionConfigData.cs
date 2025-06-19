using System;
using AI.Condition;

namespace AI
{
    [Serializable]
    public class TransitionConfigData
    {
        public string id;
        public string fromState; 
        public string toState;
        public int priority;
        public ConditionConfigData[] conditions;
        public LogicOperator logicOperator;
        public bool canInterruptSelf; 
    }
}