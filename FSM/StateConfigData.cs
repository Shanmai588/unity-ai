using System;
using AI.Condition;

namespace AI.FSM
{
    [Serializable]
    public class StateConfigData
    {
        public string stateName;
        public StateType stateType;
        public ConditionConfigData[] enterConditions;
        public ConditionConfigData[] exitConditions;
    }
}