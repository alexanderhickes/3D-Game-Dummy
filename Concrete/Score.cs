using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tutorial
{
    class Score : Stat
    {

        public Score()
        {

        }

        public override void Increase(int val)
        {
            value += val;
        }
    }

}
