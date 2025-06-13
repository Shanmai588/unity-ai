using System;

namespace AI.Condition
{
    [Serializable]
    public class ConditionConfigData
    {
        public string name;
        public ConditionType conditionType;
        public int priority;
        public LogicOperator logicOperator;
        public ConditionConfigData[] subConditions;
        public ConditionData data;
    }
}