using System;

namespace AI.Condition
{
    [Serializable]
    public class ConditionConfigData
    {
        public string id;
        public int priority;
        public ConditionData conditionData;
    }
} 