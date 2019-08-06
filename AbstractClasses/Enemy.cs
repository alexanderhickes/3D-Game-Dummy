using Mogre;
using System;
using PhysicsEng;

namespace Tutorial
{
    abstract class Enemy : Collectable
    {

        protected Stat stat;
        public Stat Stat
        {
            set { stat = value; }
        }

        protected int decrease;

        public Enemy(SceneManager mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;
        }

        public void shoot()
        {

        }

        public void update(FrameEvent evt)
        {
            RemoveMe = IsCollidingWith("Player");
            if (RemoveMe)
            {
                Console.WriteLine("Collided with Robot, Lose a life");
                stat.Decrease(decrease);
                SetPosition(new Vector3(0, 0, 0));
                Console.WriteLine("lives: " + stat.Value);
                //Dispose();
            }
        }

        /// <summary>
        /// This method determine wheter the bomb is colliding with a named object  type
        /// </summary>
        /// <param name="objName">The name of the potential colliding object</param>
        /// <returns>True if a collision with the named object type happens, false otherwhise</returns>
        private bool IsCollidingWith(string objName)
        {
            bool isColliding = false;
            foreach (Contacts c in physObj.CollisionList)
            {
                if (c.colliderObj.ID == objName || c.collidingObj.ID == objName)
                {
                    isColliding = true;
                    break;
                }
            }
            return isColliding;
        }

    }
}