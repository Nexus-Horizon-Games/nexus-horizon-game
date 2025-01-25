using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Entity_Type_Behaviours;

namespace Nexus_Horizon_Game
{
    internal class Scene
    {
        private World world = new World();

        public void Update(GameTime gameTime)
        {
            PhysicsSystem.Update(world, gameTime);

            // definitely needs reworked to only pull a specfic entity and update only that.
            var entitiesWithPlayer = world.GetEntitiesWithComponent<PlayerComponent>();
            if (entitiesWithPlayer is not null)
            {
                foreach (var entity in entitiesWithPlayer)
                {
                    if (world.EntityHasComponent<PhysicsBody2DComponent>(entity))
                    {
                        PhysicsBody2DComponent physicsBodyComponent = world.GetComponentFromEntity<PhysicsBody2DComponent>(entity);
                        world.SetComponentInEntity<PhysicsBody2DComponent>(entity, Player.Update(gameTime, physicsBodyComponent));
                    }
                }
            }

        }

        public void Draw(GameTime gameTime)
        {
            RenderSystem.Draw(world, gameTime);
            // Get entities with component
            // get component enumeration then through each component do the draw for it.
        }

        public World World { get { return world; } }
    }
}
