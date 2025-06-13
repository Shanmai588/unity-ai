using System;

namespace AI.Condition
{
    [Serializable]
    public class ConditionData
    {
        public DataValue expectedValue;
        public ComparisonOperator @operator;
        public string[] parameters;
        public TargetType target;
    }
}