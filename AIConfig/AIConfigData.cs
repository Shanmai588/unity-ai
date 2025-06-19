using System.Collections.Generic;
using AI.FSM;
using UnityEngine;

namespace AI
{
    [CreateAssetMenu(fileName = "AIConfig", menuName = "AI/AI Config Data")]
    public class AIConfigData : ScriptableObject
    {
        public StateConfigData[] stateConfigs;
        public TransitionConfigData[] transitionConfigs;
        private List<TransitionConfigData> globalTransitions;

        private Dictionary<string, StateConfigData> stateConfigCache;
        private Dictionary<string, List<TransitionConfigData>> transitionCache;

        private void OnEnable()
        {
            BuildCaches();
        }

        private void OnValidate()
        {
            BuildCaches();
        }

        private void BuildCaches()
        {
            // Build state config cache
            stateConfigCache = new Dictionary<string, StateConfigData>();
            foreach (var config in stateConfigs)
                if (!string.IsNullOrEmpty(config.stateName))
                    stateConfigCache[config.stateName] = config;

            // Build transition cache
            transitionCache = new Dictionary<string, List<TransitionConfigData>>();
            globalTransitions = new List<TransitionConfigData>();

            foreach (var transition in transitionConfigs)
                if (string.IsNullOrEmpty(transition.fromState) || transition.fromState == "*")
                {
                    // Global transition that can happen from any state
                    globalTransitions.Add(transition);
                }
                else
                {
                    if (!transitionCache.ContainsKey(transition.fromState))
                        transitionCache[transition.fromState] = new List<TransitionConfigData>();
                    transitionCache[transition.fromState].Add(transition);
                }

            // Sort transitions by priority
            foreach (var transitions in transitionCache.Values)
                transitions.Sort((a, b) => b.priority.CompareTo(a.priority));
            globalTransitions.Sort((a, b) => b.priority.CompareTo(a.priority));
        }

        public StateConfigData GetStateConfig(string stateName)
        {
            if (stateConfigCache == null) BuildCaches();
            stateConfigCache.TryGetValue(stateName, out var config);
            return config;
        }

        public TransitionConfigData[] GetTransitions(string fromState)
        {
            if (transitionCache == null) BuildCaches();
            if (transitionCache == null) return null;
            var result = new List<TransitionConfigData>();

            // Add state-specific transitions
            if (transitionCache.TryGetValue(fromState, out var stateTransitions)) result.AddRange(stateTransitions);

            // Add global transitions
            result.AddRange(globalTransitions);

            return result.ToArray();
        }

        public TransitionConfigData[] GetGlobalTransitions()
        {
            if (globalTransitions == null) BuildCaches();
            return globalTransitions?.ToArray();
        }
    }
}