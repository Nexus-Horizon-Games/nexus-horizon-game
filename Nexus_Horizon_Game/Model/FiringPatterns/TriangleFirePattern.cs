using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Entity_Type_Behaviours;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Model.Prefab;
using Nexus_Horizon_Game.Timers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus_Horizon_Game.Model.EntityPatterns
{
    internal class TriangleFiringPattern : AbstractFiringPattern, IFiringPattern
    {
        public void Fire(PrefabEntity prefab, GameTime gameTime, TimerContainer timerContainer)
        {
            List<int> firedEntities = new List<int>();
            List<IComponent> components = prefab.Components;
            Vector2 position = ((TransformComponent)prefab.Components.FirstOrDefault(x => x.GetType() == typeof(TransformComponent))).position;
            float velocity = 7f;
            var playerPosition = GetPlayerPosition();
            double direction = Math.Atan2((double)(playerPosition.Y - position.Y), (double)(playerPosition.X - position.X));
            Vector2 fireDirection = GetVectFromDirection(direction, 0);

            float speed = 5f;
            float timeInterval = 0.08f;
            int bulletNum = 1;

            timerContainer.StartTemporaryTimer(new LoopTimer(timeInterval, (gameTime, data) =>
            {
                double startTime = (double)data;
                double time = gameTime.TotalGameTime.TotalSeconds - startTime;
                double direction = Math.Atan2((double)(playerPosition.Y - position.Y), (double)(playerPosition.X - position.X));
                if (bulletNum == 1)
                {
                    Vector2 fireDirection = GetVectFromDirection(direction, 0);
                    SpawnEntity(position, fireDirection, speed, prefab);
                }
                if (bulletNum == 2)
                {
                    Vector2 fireDirection = GetVectFromDirection(direction, MathHelper.ToRadians(1));
                    SpawnEntity(position, fireDirection, speed, prefab);
                    fireDirection = GetVectFromDirection(direction, MathHelper.ToRadians(-1));
                    SpawnEntity(position, fireDirection, speed, prefab);
                }
                if (bulletNum == 3)
                {
                    Vector2 fireDirection = GetVectFromDirection(direction, 0);
                    SpawnEntity(position, fireDirection, speed, prefab);
                    fireDirection = GetVectFromDirection(direction, MathHelper.ToRadians(2));
                    SpawnEntity(position, fireDirection, speed, prefab);
                    fireDirection = GetVectFromDirection(direction, MathHelper.ToRadians(-2));
                    SpawnEntity(position, fireDirection, speed, prefab);
                }
                bulletNum++;
            }, data: gameTime.TotalGameTime.TotalSeconds, stopAfter: timeInterval * 4));
            return;
        }
    }
}
