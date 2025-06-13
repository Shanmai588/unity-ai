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

        public StateConfigData GetStateConfig(string stateName)
        {
            foreach (var config in stateConfigs)
                if (config.stateName == stateName)
                    return config;
            return null;
        }

        public TransitionConfigData[] GetTransitions(string fromState)
        {
            var transitions = new List<TransitionConfigData>();
            foreach (var transition in transitionConfigs)
                if (transition.fromState == fromState)
                    transitions.Add(transition);
            return transitions.ToArray();
        }
    }
}