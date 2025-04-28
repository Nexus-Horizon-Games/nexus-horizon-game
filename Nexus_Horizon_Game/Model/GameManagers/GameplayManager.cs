using Microsoft.Xna.Framework.Graphics;
using Nexus_Horizon_Game.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Nexus_Horizon_Game.Model.EntityFactory;
using Nexus_Horizon_Game.Timers;
using System.IO;
using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game.Model.GameManagers
{
    internal class GameplayManager
    {
        public event System.ComponentModel.PropertyChangedEventHandler? PointSystemChanged;

        private static GameplayManager instance;

        private int lives = 3; 

        private long points = 0;
        private const long pointsMax = 999999999999;
        private const long pointsMin = 0;

        private float power = 0.00f;
        private const float powerMin = 0;
        private const float powerMax = 5;

        public GameplayManager()
        {
            PointSystemChanged = null;
            instance = this;
        }

        public static GameplayManager Instance
        {
            get => instance;
        }

        public int Lives
        {
            get => lives;
        }

        public long Points
        {
            get => points;
        }

        public float Power
        {
            get => power;
        }

        public float PowerMultiplier()
        {
            return 1 + ((power * 0.75f) * 0.25f); // Adding one to keep 100% of original then any other added onto 1 is extra // 0.25 for scaling to normal damage // 0.75 for scaling the power
        }

        public void PickedUpAPower()
        {
            power += 0.10f;
            power = Math.Min(power, powerMax);
            PointSystemChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Power)));
        }

        public void PickedUpAPoint()
        {
            points += 250;
            points = Math.Min(points, pointsMax);
            PointSystemChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Points)));
        }

        /// <summary>
        /// entering in the age will add point corresponding to that tag
        /// </summary>
        /// <param name="enemyTagType"> bit of enemy type </param>
        public void KilledEnemy(int entityID)
        {
            int powerDropCount = 0;
            int pointDropCount = 0;

            Tag enemyTagType = Scene.Loaded.ECS.GetComponentFromEntity<TagComponent>(entityID).Tag;

            if ((enemyTagType & Tag.SMALLGRUNT) == Tag.SMALLGRUNT)
            {
                powerDropCount = RandomGenerator.GetInteger(1,2);
                pointDropCount = RandomGenerator.GetInteger(0, 2);
                DropFactory.SpawnDrops(powerDropCount, Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(entityID).position, Tag.POWERDROP, "PowerCarrot");
                DropFactory.SpawnDrops(pointDropCount, Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(entityID).position, Tag.POINTDROP, "PointCarrot");
                points = Math.Min((points + 1000) * (int)PowerMultiplier(), pointsMax);
            }
            else if ((enemyTagType & Tag.MEDIUMGRUNT) == Tag.MEDIUMGRUNT)
            {
                powerDropCount = RandomGenerator.GetInteger(0, 2);
                pointDropCount = RandomGenerator.GetInteger(2, 3);
                DropFactory.SpawnDrops(powerDropCount, Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(entityID).position, Tag.POWERDROP, "PowerCarrot");
                DropFactory.SpawnDrops(pointDropCount, Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(entityID).position, Tag.POINTDROP, "PointCarrot");
                points = Math.Min((points + 6500) * (int)PowerMultiplier(), pointsMax);
            }
            else if ((enemyTagType & Tag.HALFBOSS) == Tag.HALFBOSS)
            {
                powerDropCount = RandomGenerator.GetInteger(0, 25);
                pointDropCount = 25 - powerDropCount;
                DropFactory.SpawnDrops(powerDropCount, Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(entityID).position, Tag.POWERDROP, "PowerCarrot");
                DropFactory.SpawnDrops(pointDropCount, Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(entityID).position, Tag.POINTDROP, "PointCarrot");
                points = Math.Min((points + 21000) * (int)PowerMultiplier(), pointsMax);
            }
            else if ((enemyTagType & Tag.BOSS) == Tag.BOSS)
            {
                powerDropCount = RandomGenerator.GetInteger(0, 30);
                pointDropCount = 25 - powerDropCount;
                DropFactory.SpawnDrops(powerDropCount, Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(entityID).position, Tag.POWERDROP, "PowerCarrot");
                DropFactory.SpawnDrops(pointDropCount, Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(entityID).position, Tag.POINTDROP, "PointCarrot");
                points = Math.Min((points + 50000) * (int)PowerMultiplier(), pointsMax);
            }

            PointSystemChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Points)));
        }

        public void DealtDamage()
        {
            points = Math.Min((points + 10) * (int)PowerMultiplier(), pointsMax);
            PointSystemChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Points)));
        }

        public void PlayerDied()
        {
            lives = Math.Max(lives - 1, 0);

            points = Math.Max(points - 28750, pointsMin);
            power = Math.Max(power / 3, powerMin);
            PointSystemChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Power)));
            PointSystemChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Points)));
            PointSystemChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Lives)));
        }
    }
}
