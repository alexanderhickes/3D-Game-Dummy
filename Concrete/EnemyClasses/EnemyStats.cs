using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tutorial
{
    class EnemyStats : CharacterStats
    {
        
        protected override void InitStats()
        {
            base.InitStats();

            health.InitValue(100);
            shield.InitValue(100);
            lives.InitValue(3);

        }
    }

}


