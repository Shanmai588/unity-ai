using System.Collections.Generic;
using AI.Controller;

namespace AI.Condition
{
    public class ConditionManager
    {
        private readonly Dictionary<ConditionType, ICondition> conditions;

        public ConditionManager()
        {
            conditions = new Dictionary<ConditionType, ICondition>();
            RegisterDefaultConditions();
        }

        private void RegisterDefaultConditions()
        {
            // RegisterCondition(ConditionType.Health, new HealthCondition());
            // RegisterCondition(ConditionType.Distance, new DistanceCondition());
            // RegisterCondition(ConditionType.Command, new CommandCondition());
            // RegisterCondition(ConditionType.Timer, new TimerCondition());
        }

        public void RegisterCondition(ConditionType type, ICondition condition)
        {
            conditions[type] = condition;
        }

        public bool EvaluateCondition(ConditionConfigData config, AIController context)
        {
            if (!conditions.TryGetValue(config.conditionType, out var condition))
                return false;

            if (config.subConditions != null && config.subConditions.Length > 0)
                return EvaluateLogicGroup(config.subConditions, context);

            return condition.Evaluate(config.data, context);
        }

        public bool EvaluateConditions(ConditionConfigData[] configs, LogicOperator @operator, AIController context)
        {
            if (configs == null || configs.Length == 0) return true;

            switch (@operator)
            {
                case LogicOperator.And:
                    foreach (var config in configs)
                        if (!EvaluateCondition(config, context))
                            return false;
                    return true;

                case LogicOperator.Or:
                    foreach (var config in configs)
                        if (EvaluateCondition(config, context))
                            return true;
                    return false;

                case LogicOperator.Not:
                    return !EvaluateCondition(configs[0], context);

                default:
                    return true;
            }
        }

        private bool EvaluateLogicGroup(ConditionConfigData[] configs, AIController context)
        {
            if (configs.Length == 0) return true;
            var firstConfig = configs[0];
            return EvaluateConditions(configs, firstConfig.logicOperator, context);
        }
    }
}