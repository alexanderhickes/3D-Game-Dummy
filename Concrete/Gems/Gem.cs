using System;
using Mogre;
using PhysicsEng;

namespace Tutorial
{
    class Gem : Collectable
    {
        protected Stat score;
        protected int increase;

        protected Gem(SceneManager mSceneMgr, Stat score)
        {
            this.mSceneMgr = mSceneMgr;
            this.score = score;
        }

        protected virtual void LoadModel()
        {          
        }

        public override void Update(FrameEvent evt)
        {
            Animate(evt);

            RemoveMe = IsCollidingWith("Player");
            if (RemoveMe)
            {
                score.Increase(increase);
                Dispose();
            }
            base.Update(evt);
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
