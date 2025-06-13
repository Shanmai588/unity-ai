using AI.Controller;

namespace AI.Condition
{
    public interface ICondition
    {
        bool Evaluate(ConditionData data, AIController context);
    }
}