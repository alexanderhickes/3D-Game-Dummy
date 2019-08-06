using System;
using Mogre;

using PhysicsEng;
using System.Collections.Generic;

namespace Tutorial
{ 
    abstract class Projectile : MovableElement
    {
        Timer time;
        protected int maxTime = 1000;
        protected Vector3 initialVelocity;
        protected float speed;
        protected Vector3 initialDirection;
        public Vector3 InitialDirection
        {
            set
            {
                initialDirection = value;
                physObj.Velocity = speed * initialDirection;
            }
        }
        protected float healthDamage;
        public float HealthDamage
        {
            get { return healthDamage; }
        }

        protected float shieldDamage;
        public float ShieldDamage
        {
            get { return shieldDamage; }
        }

        virtual protected void Load() { }

        protected Projectile()
        {
            time = new Timer();
        }

        public override void Dispose()
        {
            base.Dispose();
            this.remove = true;
        }

        virtual public void Update(FrameEvent evt)
        {
            // Projectile collision detection goes here
            // (ignore until week 8) ...

            if (!remove && time.Milliseconds > maxTime)
            {
                Dispose();
                remove = true;
            }
        }
    }
}
