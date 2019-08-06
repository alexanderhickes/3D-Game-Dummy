using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tutorial
{
    abstract class AI
    {
        protected Character character;      // A reference to the character to control

        protected float speed;
        /// <summary>
        /// Write only. This property allows to set the speed
        /// </summary>
        public float Speed
        {
            set { speed = value; }
        }

    }
}
