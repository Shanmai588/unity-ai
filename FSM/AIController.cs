using AI.FSM;
using GenericGameplayInterface;
using UnityEngine;

namespace AI.Controller
{
    /// <summary>
    /// AI Controller would take over if target is not under command
    /// </summary>
    public class AIController : MonoBehaviour
    {
        private StateMachine stateMachine;
        [SerializeField] private AIConfigData aiConfig;
        private IHealth healthComponent;
        private IMover moverComponent;
        private IAttacker attackerComponent;
        private IDetector detectorComponent;
        private Transform target;
        private Vector3 targetDestination;
        
        public Vector3 TargetDestination => targetDestination;

        void Start()
        {
            // Get components
            healthComponent = GetComponent<IHealth>();
            moverComponent = GetComponent<IMover>();
            attackerComponent = GetComponent<IAttacker>();
            detectorComponent = GetComponent<IDetector>();
            
            // Initialize state machine
            stateMachine = new StateMachine();
            stateMachine.Initialize(aiConfig, this);
        }
        
        void Update()
        {
            // Update state machine
            stateMachine.Update();
        }
        
        /// <summary>
        ///     Ensure Getting Same Component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public new T GetComponent<T>()
        {
            if (typeof(T) == typeof(IHealth)) return (T)healthComponent;
            if (typeof(T) == typeof(IMover)) return (T)moverComponent;
            if (typeof(T) == typeof(IAttacker)) return (T)attackerComponent;
            if (typeof(T) == typeof(IDetector)) return (T)detectorComponent;
            return base.GetComponent<T>();
        }
    }
}