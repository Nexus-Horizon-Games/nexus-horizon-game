using Nexus_Horizon_Game.Systems;
using Nexus_Horizon_Game.Components;
using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game
{
    // this particular scene is the gameplay
    internal abstract class Scene
    {
        // The current Loaded Scene.
        static private Scene loaded;

        // dont worry about this for now
        static private Scene dontDestroyScene;

        private ECS ecs = new ECS();

        public ECS ECS { get { return ecs; } }

        public Scene()
        {
            loaded = this;

            ECS.OnAddComponentEvent += (entity, componentType) =>
            {
                if (componentType == typeof(StateComponent))
                {
                    StateSystem.OnNewStateComponent(entity);
                }
                else if (componentType == typeof(BehaviourComponent))
                {
                    BehaviourSystem.OnNewBehaviourComponent(entity);
                }
            };
        }

        /// <summary>
        /// Gets the currently loaded scene
        /// </summary>
        public static Scene Loaded
        {
            get => loaded;
            set
            {
                loaded = value;
                loaded.LoadContent();
                loaded.Initialize();
                loaded.LoadScene();
            }
        }

        /// <summary>
        /// This will be implemented later if needing constant entities during transitions from one scene to another.
        /// </summary>
        public static Scene DontDestroyScene
        {
            get
            {
                if (dontDestroyScene == null)
                {
                    dontDestroyScene = new DontDestroyScene();
                    dontDestroyScene.LoadContent();
                    dontDestroyScene.LoadScene();
                    dontDestroyScene.Initialize();
                }
                return dontDestroyScene;
            }

        }

        protected abstract void LoadContent();

        protected abstract void LoadScene();

        protected abstract void Initialize();

        public virtual void Update(GameTime gameTime) { }
    }
}
