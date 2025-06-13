using System.Collections.Generic;
using System.Linq;
using AI.Condition;
using AI.Controller;

namespace AI.FSM
{
    public class StateMachine
    {
        private ConditionManager conditionManager;
        private AIController controller;
        private State currentState;
        private Dictionary<string, State> states;
        public AIConfigData config { get; private set; }

        public void Initialize(AIConfigData config, AIController controller)
        {
            this.config = config;
            this.controller = controller;
            conditionManager = new ConditionManager();
            states = new Dictionary<string, State>();

            // Create states based on config
            foreach (var stateConfig in config.stateConfigs)
            {
                State state = null;
                switch (stateConfig.stateType)
                {
                    case StateType.Idle:
                        state = new IdleState(this, controller);
                        break;
                    // case StateType.Moving:
                    //     state = new MovingState(this, controller);
                    //     break;
                    // case StateType.Attacking:
                    //     state = new AttackingState(this, controller);
                    //     break;
                }

                if (state != null) states[stateConfig.stateName] = state;
            }

            // Set initial state
            if (states.Count > 0) ChangeState(states.Keys.First());
        }

        public void ChangeState(string stateName)
        {
            if (!states.TryGetValue(stateName, out var newState))
                return;

            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }

        public void Update()
        {
            currentState?.Update();
        }

        public bool CanTransition(string from, string to)
        {
            var transitions = config.GetTransitions(from);
            foreach (var transition in transitions)
                if (transition.toState == to)
                    return conditionManager.EvaluateConditions(
                        transition.conditions,
                        transition.logicOperator,
                        controller);

            return false;
        }
    }
}