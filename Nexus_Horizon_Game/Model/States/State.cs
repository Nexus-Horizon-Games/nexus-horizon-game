using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;

namespace Nexus_Horizon_Game.States
{
    internal abstract class State
    {
        private int entity = 0;
        private int animBuffer = 0;
        protected State() { }

        protected State(int entity)
        {
            this.entity = entity;
        }

        protected int Entity
        {
            get => entity;
        }

        public delegate void OnStopped();

        public event OnStopped OnStopEvent;

        public virtual void OnUpdate(GameTime gameTime) { }
        public virtual void Initalize(int entity) { this.entity = entity; }
        public virtual void OnStart() { }
        public virtual void OnStop()
        {
            OnStopEvent?.Invoke();
        }

        public virtual State Clone()
        {
            return this;
        }

        public void incrementAnimationBuffered()
        {
            if (animBuffer % 4 != 0)
            {
                animBuffer++;
                return;
            }
            animBuffer = 0;
            var sprite = Scene.Loaded.ECS.GetComponentFromEntity<SpriteComponent>(this.Entity);
            sprite.incrementAnimation();
            Scene.Loaded.ECS.SetComponentInEntity<SpriteComponent>(this.Entity, sprite);
            animBuffer++;

        }
        public void incrementAnimationBufferedWrapped()
        {
            if (animBuffer % 4 != 0)
            {
                animBuffer++;
                return;
            }
            animBuffer = 0;
            var sprite = Scene.Loaded.ECS.GetComponentFromEntity<SpriteComponent>(this.Entity);
            sprite.incrementAnimationWrap();
            Scene.Loaded.ECS.SetComponentInEntity<SpriteComponent>(this.Entity, sprite);
            animBuffer++;

        }
        public void decrementAnimationBuffered()
        {
            if(animBuffer % 4 != 0)
            {
                animBuffer++;
                return;
            }
            animBuffer = 0;
            var sprite = Scene.Loaded.ECS.GetComponentFromEntity<SpriteComponent>(this.Entity);
            sprite.decrementAnimation();
            Scene.Loaded.ECS.SetComponentInEntity<SpriteComponent>(this.Entity, sprite);
            animBuffer++;

        }

        public void incrementAnimation()
        {
            var sprite = Scene.Loaded.ECS.GetComponentFromEntity<SpriteComponent>(this.Entity);
            sprite.incrementAnimation();
            Scene.Loaded.ECS.SetComponentInEntity<SpriteComponent>(this.Entity, sprite);

        }
        public void incrementAnimationWrapped()
        {
            var sprite = Scene.Loaded.ECS.GetComponentFromEntity<SpriteComponent>(this.Entity);
            sprite.incrementAnimationWrap();
            Scene.Loaded.ECS.SetComponentInEntity<SpriteComponent>(this.Entity, sprite);

        }
        public void decrementAnimation()
        {
            var sprite = Scene.Loaded.ECS.GetComponentFromEntity<SpriteComponent>(this.Entity);
            sprite.decrementAnimation();
            Scene.Loaded.ECS.SetComponentInEntity<SpriteComponent>(this.Entity, sprite);

        }
    }
}
