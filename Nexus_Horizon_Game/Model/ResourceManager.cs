using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Nexus_Horizon_Game
{
    /// <summary>
    /// A class that helps manage resources such as textures or sounds.
    /// </summary>
    /// <typeparam name="T">The type of resource to manage.</typeparam>
    internal class ResourceManager<T>
    {
        private Dictionary<string, T> resources = new();
        private ContentManager contentManager;

        /// <summary>
        /// Creates a new ResourceManager.
        /// </summary>
        /// <param name="contentManager">The content manager.</param>
        public ResourceManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        /// <summary>
        /// Used to preload a list of resources.
        /// </summary>
        /// <param name="names">The names of the resources to load.</param>
        public void LoadResources(List<string> names)
        {
            foreach (string name in names)
            {
                if (!resources.ContainsKey(name))
                {
                    resources.Add(name, contentManager.Load<T>(name));
                }
            }
        }

        /// <summary>
        /// Used to clear any resources from the resource manager.
        ///     Normally used when loading in new scene that will remove any previous reference to resources even if during scene alive time it may not use it.
        /// </summary>
        public void ClearResources()
        {
            resources.Clear();
        }

        /// <summary>
        /// Gets a specified resource.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        /// <returns>The specified resource.</returns>
        public T GetResource(string name)
        {
            if (resources.TryGetValue(name, out T resource)) { return resource; }

            // Load it if it has not been loaded before
            T newResource = contentManager.Load<T>(name);
            resources.Add(name, newResource);
            return newResource;
        }
    }
}
