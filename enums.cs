namespace AI
{
    public enum StateType
    {
        Idle,
        Moving,
        Attacking,
        Gathering,
        Patrolling,
        Retreating,
        Dead
    }

    public enum ConditionType
    {
        Health,
        Distance,
        Command,
        Timer,
        EnemyInRange,
        ResourceInRange,
        DestinationReached
    }

    public enum TargetType
    {
        Self,
        NearestEnemy,
        NearestResource,
        AttackTarget,
        MoveDestination,
        Custom
    }

    public enum ValueType
    {
        Absolute,
        Percentage,
        Boolean,
        String
    }

    public enum ComparisonOperator
    {
        Equal,
        NotEqual,
        GreaterThan,
        LessThan,
        GreaterOrEqual,
        LessOrEqual
    }

    public enum LogicOperator
    {
        And,
        Or,
        Not
    }
}