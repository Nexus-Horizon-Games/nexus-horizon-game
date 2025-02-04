using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;

namespace Nexus_Horizon_Game.Entity_Type_Behaviours
{
    internal class Bullet : Behaviour
    {
        public Bullet(int thisEntity) : base(thisEntity)
        {
        }

        public override void OnUpdate(GameTime gameTime)
        {
            DeleteOnOutOfBounds(this.Entity);
        }

        /// <summary>
        /// deletes the entity of a bullet when out of the radius of the play area
        /// </summary>
        /// <param name="entity"></param>
        private void DeleteOnOutOfBounds(int entity)
        {
            TransformComponent transform = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(entity);

            if ((transform.position.X > GameM.CurrentScene.ArenaRight || transform.position.X < GameM.CurrentScene.ArenaLeft) ||
                (transform.position.Y > GameM.CurrentScene.ArenaBottom || transform.position.Y < GameM.CurrentScene.ArenaTop))
            {
                GameM.CurrentScene.World.DestroyEntity(entity);
            }
        }
    }
}
