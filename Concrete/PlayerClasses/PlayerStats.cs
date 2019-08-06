using System;
using Mogre;

namespace Tutorial
{
    class PlayerStats : CharacterStats
    {
        protected Stat score;

        public Stat Score
        {
            get { return score;  }
        }



        protected override void InitStats()
        {
            base.InitStats();
            score = new Score();

            score.InitValue(0);
            health.InitValue(100);
            shield.InitValue(50);
            lives.InitValue(3);

        }
    }

}

