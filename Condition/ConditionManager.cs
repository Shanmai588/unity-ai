using AI.Controller;

namespace AI.Condition
{
    public static class ConditionManager
    {
        public static bool EvaluateCondition(ICondition config, AIController context)
        {
            // if (config.subConditions != null && config.subConditions.Length > 0)
            //     return EvaluateLogicGroup(config.subConditions, context);

            return config.Evaluate(context);
        }
        
        public static bool EvaluateCondition(ConditionConfigData config, AIController context)
        {
            // if (config.subConditions != null && config.subConditions.Length > 0)
            //     return EvaluateLogicGroup(config.subConditions, context);

            return config.conditionData.Evaluate(context);
        }

        public static bool EvaluateConditions(ConditionConfigData[] configs, LogicOperator @operator,
            AIController context)
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

        // private static bool EvaluateLogicGroup(ConditionConfigData[] configs, AIController context)
        // {
        //     if (configs.Length == 0) return true;
        //     var firstConfig = configs[0];
        //     return EvaluateConditions(configs, firstConfig.logicOperator, context);
        // }
    }
}