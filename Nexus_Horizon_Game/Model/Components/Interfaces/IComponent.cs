using System;

namespace Nexus_Horizon_Game.Components
{
    internal interface IComponent : IEquatable<IComponent>
    {
        protected bool IsEmpty
        {
            get;
            set;
        }

        /// <summary>
        /// Whether this component is an empty component (ECVS uses this to test if an entity has a component).
        /// </summary>
        /// <returns>A boolean.</returns>
        bool IsEmptyComponent();

        IComponent Clone() => this;

        /// <summary>
        /// Makes an empty component (ECS uses this to test if an enity has a component).
        /// NOTE: for some reason, was not able to make this a "static abstract" method, this works for now though.
        /// </summary>
        /// <returns>The empty component.</returns>
        /// <exception cref="NotImplementedException"></exception>
        static virtual IComponent MakeEmptyComponent() => throw new NotImplementedException();
    }
}
