using Nexus_Horizon_Game.Components;
using System.Collections.Generic;
using System.Linq;

namespace Nexus_Horizon_Game.Model.Components
{
    internal struct ChildrenComponent : IComponent
    {
        private bool isEmpty;

        Dictionary<string, int> children = new Dictionary<string, int>();

        public ChildrenComponent() { }

        bool IComponent.IsEmpty
        {
            get => isEmpty;
            set => isEmpty = value;
        }

        /// <summary>
        /// get the child entityIDby name other wise returns null.
        /// </summary>
        /// <param name="childName"> child in entity. </param>
        /// <returns> child entityID. </returns>
        public int? GetChildIDByName(string childName)
        {
            return children.ContainsKey(childName) ? children[childName] : null;
        }

        public List<int> GetChildren()
        {
            return children.Values.ToList();
        }

        public void AddChild(string childName, int childID)
        {
            children.Add(childName, childID);
        }

        /// <inheritdoc/>
        public bool Equals(IComponent other)
        {
            if (other is ChildrenComponent o)
            {
                if (children == o.children)
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
            ChildrenComponent transfrom = new ChildrenComponent();
            transfrom.isEmpty = true;
            return transfrom;
        }
    }
}
