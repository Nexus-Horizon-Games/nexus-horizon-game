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
    internal class CicleFiringPattern1 : AbstractFiringPattern, IFiringPattern
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

            float arcInterval = MathHelper.TwoPi / 27;

            for (int burst = 0; burst < 2; burst++)
            {
                timerContainer.StartTemporaryTimer(new DelayTimer(0.5f * burst, (gameTime, data) =>
                {
                    for (int j = 0; j < 27; j++)
                    {
                        Vector2 direction = new Vector2((float)Math.Cos(j * arcInterval), (float)Math.Sin(j * arcInterval));
                        SpawnEntity(position, direction, 5.0f, prefab);
                    }
                }));
            }
            return;
        }
    }
}
