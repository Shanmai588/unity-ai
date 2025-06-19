using AI.Controller;
using GenericGameplayInterface;

namespace AI.FSM
{
    public class MovingState : State
    {
        private IMover mover;

        public MovingState(StateMachine machine, AIController controller)
            : base("Moving", machine, controller)
        {
        }

        public override void Enter()
        {
            if (controller && mover == null) mover = controller.GetComponent<IMover>();

            // Movement

            mover.BeginMove(controller.TargetDestination);
        }

        public override void Update()
        {
            // Moving tho
        }

        public override void Exit()
        {
            mover.Cancel();
        }

        public override void CheckTransitions()
        {
            var transitions = stateMachine.config.GetTransitions(stateName);
            foreach (var transition in transitions)
                if (stateMachine.CanTransition(stateName, transition.toState))
                {
                    stateMachine.ChangeState(transition.toState);
                    break;
                }
        }
    }
}