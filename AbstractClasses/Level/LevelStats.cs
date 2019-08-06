using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tutorial
{
    abstract class LevelStats
    {
        private int numGems;
        private int numHealthPU;
        private int numShieldPU;
        private int numLivesPU;
        private int numEnemies;
        private int numCollectableGuns;

        virtual public void InitStats()
        {

        }
    }
}
