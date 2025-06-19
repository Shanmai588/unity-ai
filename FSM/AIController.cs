using System;
using System.Collections.Generic;
using AI.FSM;
using GenericGameplayInterface;
using UnityEngine;

namespace AI.Controller
{
    /// <summary>
    ///     AI Controller would take over if target is not under command
    /// </summary>
    public class AIController : MonoBehaviour
    {
        [SerializeField] private AIConfigData aiConfigData;
        private IAttacker attackerComponent;
        private IDetector detectorComponent;
        private IHealth healthComponent;
        private IMover moverComponent;
        private StateMachine stateMachine;
        private Transform target;
        private AIConfig aiConfig;
        public Vector3 TargetDestination { get; }
        private readonly Dictionary<Type, object> _cached = new();

        private void Start()
        {
            // Get components
            healthComponent = GetCached<IHealth>();
            moverComponent = GetCached<IMover>();
            attackerComponent = GetCached<IAttacker>();
            detectorComponent = GetCached<IDetector>();

            // Initialize state machine
            stateMachine = new StateMachine();
            aiConfig = AIConfigProvider.GetConfig(aiConfigData);
            stateMachine.Initialize(aiConfig, this);
        }

        private void Update()
        {
            // Update state machine
            stateMachine.Update();
        }
        
        /// <summary>
        /// Get a component or interface, cached after first lookup.
        /// </summary>
        public T GetCached<T>() where T : class          // ① constrain T to a reference type
        {
            var key = typeof(T);

            if (_cached.TryGetValue(key, out var obj))
                return (T)obj;                           // ② cast from object, not Component

            // plain GetComponent(Type) returns Component – we cast *via* object
            var found = base.GetComponent(key) as T;
            _cached[key] = found;
            return found;
        }

    }
}