using AI.Controller;
using GenericGameplayInterface;

namespace AI.FSM
{
    public class IdleState : State
    {
        public IdleState(StateMachine machine, AIController controller)
            : base("Idle", machine, controller)
        {
        }

        public override void Enter()
        {
            var mover = controller.GetComponent<IMover>();
            mover?.Cancel();
        }

        public override void Update()
        {
            CheckTransitions();
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