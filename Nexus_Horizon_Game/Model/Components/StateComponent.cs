using Nexus_Horizon_Game.States;
using System.Collections.Generic;

namespace Nexus_Horizon_Game.Components
{
    /// <summary>
    /// A component that stores a single state (an enum) for an entity.
    /// </summary>
    internal struct StateComponent : IComponent
    {
        private bool isEmpty;

        public int currentState = 0;
        public List<State> states;

        public Dictionary<int, int> transitions = new Dictionary<int, int>();

        public delegate int TransitionHandler(int currentState);

        public TransitionHandler transitionFunction;

        public StateComponent(List<State> states)
        {
            this.isEmpty = false;
            this.states = states;
            this.transitionFunction = LinearTransitionFunction;
        }

        public StateComponent(List<State> states, Dictionary<int, int> transitions)
        {
            this.isEmpty = false;
            this.states = states;
            this.transitions = transitions;
            this.transitionFunction = UseDictionaryTransitionFunction;
        }

        public StateComponent(List<State> states, TransitionHandler transitionFunction)
        {
            this.isEmpty = false;
            this.states = states;
            this.transitionFunction = transitionFunction;
        }

        public int LinearTransitionFunction(int currentState)
        {
            return ++currentState;
        }

        public int UseDictionaryTransitionFunction(int currentState)
        {
            int newState;
            if (transitions.TryGetValue(currentState, out newState))
            {
                return newState;
            }

            return currentState;
        }

        public void Transition()
        {
            currentState = transitionFunction.Invoke(currentState);
        }

        bool IComponent.IsEmpty
        {
            get => isEmpty;
            set => isEmpty = value;
        }

        /// <inheritdoc/>
        public bool Equals(IComponent other)
        {
            if (other is StateComponent o)
            {
                if (states == o.states)
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public bool IsEmptyComponent()
        {
            return isEmpty;
        }

        /// <inheritdoc/>
        public static IComponent MakeEmptyComponent()
        {
            StateComponent comp = new()
            {
                isEmpty = true
            };
            return comp;
        }
    }
}
