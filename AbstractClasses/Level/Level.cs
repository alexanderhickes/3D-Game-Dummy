using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tutorial
{
    abstract class Level
    {
        private int level;

        private LevelStats levelStats;

        private Player player;

        private Environment environment;

        private List<Gem> gems;

        private List<PowerUp> powerUps;

        private List<CollectableGun> guns;

        private List<Enemy> enemies;

        /// <summary>
        /// This virtual method updates the state of the character
        /// </summary>
        /// <param name="evt">A frame event that can be used for the update of the character</param>
        virtual public void Update(FrameEvent evt)
        {
        }
    }
}
