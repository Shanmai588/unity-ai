using AI.Controller;

namespace AI.Condition
{
    public interface ICondition
    {
        bool Evaluate(AIController context);
    }
}