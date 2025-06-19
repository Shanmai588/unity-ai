using AI.Controller;
using UnityEngine;

namespace AI.Condition
{
    public abstract class ConditionData : ScriptableObject, ICondition
    {
        public abstract bool Evaluate(AIController context);
    }
}