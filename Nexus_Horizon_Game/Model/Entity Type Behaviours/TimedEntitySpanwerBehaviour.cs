
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Model.EntityPatterns;
using Nexus_Horizon_Game.Model.Prefab;
using Nexus_Horizon_Game.Timers;
using System.Diagnostics;

namespace Nexus_Horizon_Game.Model.Entity_Type_Behaviours
{
    internal class TimedEntitySpanwerBehaviour : EntitySpawnerBehaviour
    {
        private Timer timer;
        private Timer lifeTimer;
        private IFiringPattern? firingPattern = null;
        private TimerContainer timerContainer = new TimerContainer();

        /// <summary>
        /// Creates a new <see cref="TimedEntitySpanwerBehaviour"/>.
        /// </summary>
        /// <param name="thisEntity">This entity.</param>
        /// <param name="prefab">The prefab to spawn.</param>
        /// <param name="timer">The timer that determines when the entity spawns (the entity spawns when the timer's OnElapsedEvent is called).</param>
        /// <param name="lifetime">If the lifetime is negative then this entity will be destoried when the timer is destoried,
        /// otherwise it will be destroyed after the given lifetime.</param>
        /// <param name="firingPattern">The firing pattern that will be used to spawn the entities.</param>
        public TimedEntitySpanwerBehaviour(PrefabEntity prefab, Timer timer, float lifetime = -1, IFiringPattern? firingPattern = null) : base(prefab)
        {
            this.timer = timer;
            this.firingPattern = firingPattern;

            // Spawn entitys based on the given timer
            this.timer.OnElapsedEvent += (gameTime, _) =>
            {
                if (this.firingPattern != null)
                {
                    SpawnEntitiesWithPattern(firingPattern, gameTime, timerContainer);
                }
                else
                {
                    SpawnEntity();
                }

                Debug.WriteLine("Spawning an entity in timer");
            };

            if (lifetime < 0) // then destory the entity when the timer stops
            {
                timer.OnStopEvent += () =>
                {
                    Scene.Loaded.ECS.DestroyEntity(this.Entity);
                    Debug.WriteLine("Destorying spawner");
                };
            }
            else // then destory the entity after the given lifetime
            {
                lifeTimer = new DelayTimer(lifetime, (_, _) =>
                {
                    Scene.Loaded.ECS.DestroyEntity(this.Entity);
                    Debug.WriteLine("Destorying spawner");
                });
            }

            this.timer.Start();
            lifeTimer.Start();
        }

        public override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            this.timer.Update(gameTime);
            this.lifeTimer.Update(gameTime);
            this.timerContainer.Update(gameTime);
        }
    }
}
