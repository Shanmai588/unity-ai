using AI.Controller;

namespace AI.FSM
{
    public abstract class State
    {
        protected AIController controller;
        protected StateMachine stateMachine;
        protected string stateName;

        public State(string name, StateMachine machine, AIController controller)
        {
            stateName = name;
            stateMachine = machine;
            this.controller = controller;
        }

        public virtual void Enter()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void Exit()
        {
        }

        public abstract void CheckTransitions();
    }
}