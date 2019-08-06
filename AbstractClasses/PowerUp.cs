using System;
using Mogre;
using PhysicsEng;

namespace Tutorial
{
    abstract class PowerUp : Collectable
    {
        protected Stat stat;
        public Stat Stat
        {
            set { stat = value; }
        }

        protected int increase;

        protected PowerUp(SceneManager mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;
            LoadModel();
        }

        virtual protected void LoadModel() { }

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

        public override void Update(FrameEvent evt)
        {
            Animate(evt);

            RemoveMe = IsCollidingWith("Player");
            if (RemoveMe)
            {
                stat.Increase(increase);
                Dispose();
            }
        }
    }
}
