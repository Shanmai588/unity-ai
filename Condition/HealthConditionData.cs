using System.Linq;
using AI;
using AI.Condition;
using AI.Controller;
using GenericGameplayInterface;

[UnityEngine.CreateAssetMenu(fileName = "HealthCondition", menuName = "AI/Conditions/Health Condition")]
public class HealthConditionData : ConditionData
{
    public TargetType target;
    public ComparisonOperator comparisonOperator;
    public ValueType valueType = ValueType.Percentage;
    public float expectedValue;

    public ConditionType GetConditionType() => ConditionType.Health;

    public override bool Evaluate(AIController context)
    {
        var health = GetTargetHealth(target, context);
        if (health == null) return false;

        float currentValue = valueType == ValueType.Percentage 
            ? health.GetHealthPercentage() 
            : health.CurrentHealth;

        return CompareValues(currentValue, expectedValue, comparisonOperator);
    }

    private IHealth GetTargetHealth(TargetType target, AIController context)
    {
        switch (target)
        {
            case TargetType.Self:
                return context.GetCached<IHealth>();
            case TargetType.NearestEnemy:   
                var detector = context.GetCached<IDetector>();
                var enemy = detector?.Targets.FirstOrDefault();
                // BIG PERFORMANCE ISSUE: IMPROVE DETECTOR???
                return enemy?.GameObject.GetComponent<IHealth>();
            default:
                return null;
        }
    }

    private bool CompareValues(float current, float expected, ComparisonOperator op)
    {
        switch (op)
        {
            case ComparisonOperator.Equal:
                return UnityEngine.Mathf.Approximately(current, expected);
            case ComparisonOperator.NotEqual:
                return !UnityEngine.Mathf.Approximately(current, expected);
            case ComparisonOperator.GreaterThan:
                return current > expected;
            case ComparisonOperator.LessThan:
                return current < expected;
            case ComparisonOperator.GreaterOrEqual:
                return current >= expected;
            case ComparisonOperator.LessOrEqual:
                return current <= expected;
            default:
                return false;
        }
    }
}