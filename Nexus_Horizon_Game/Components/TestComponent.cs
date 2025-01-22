namespace Nexus_Horizon_Game.Components
{
    internal struct TestComponent : IComponent
    {
        private int testValue;
        public TestComponent(int testValue)
        {
            this.testValue = testValue;
        }

        /// <inheritdoc/>
        public bool Equals(IComponent other)
        {
            if (other is TestComponent o)
            {
                if (testValue == o.testValue)
                {
                    return true;
                }
            }
            return false;
        }

        /// <inheritdoc/>
        public bool IsEmptyComponent()
        {
            return Equals(MakeEmptyComponent());
        }

        /// <inheritdoc/>
        public static IComponent MakeEmptyComponent()
        {
            return new TestComponent(-1);
        }
    }
}
