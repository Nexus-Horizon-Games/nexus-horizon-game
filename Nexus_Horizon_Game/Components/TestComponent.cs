namespace Nexus_Horizon_Game.Components
{
    internal class TestComponent : IComponent
    {
        private int testValue;
        public TestComponent(int testValue)
        {
            this.testValue = testValue;
        }

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
    }
}
