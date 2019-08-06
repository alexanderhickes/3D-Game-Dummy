using System;
using Mogre;
using PhysicsEng;

namespace Tutorial
{
    class CannonBall : Projectile
    {

        public CannonBall(SceneManager mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;
            healthDamage = 15;
            shieldDamage = 20;
            speed = 3;
            Load();
        }

        protected override void Load()
        {

            RemoveMe = false;

            gameEntity = mSceneMgr.CreateEntity("Sphere.mesh");
            GameNode = mSceneMgr.CreateSceneNode();
            GameNode.Scale(2, 2, 2);
            GameNode.AttachObject(gameEntity);
            mSceneMgr.RootSceneNode.AddChild(GameNode);

            physObj = new PhysObj(10, "CannonBall", 0.1f, 0.5f);
            physObj.SceneNode = GameNode;
            physObj.AddForceToList(new WeightForce(physObj.InvMass));

            Physics.AddPhysObj(physObj);
        }

        /// <summary>
        /// This method set the position of the bomb in the given location
        /// </summary>
        /// <param name="position">The location where to position the bomb</param>
        public override void SetPosition(Vector3 position)
        {
            GameNode.Position = position;
            physObj.Position = position;
        }

        /// <summary>
        /// This method update the bomb state
        /// </summary>
        /// <param name="evt"></param>
        public override void Update(FrameEvent evt)
        {
            RemoveMe = IsCollidingWith("Robot");
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

        /// <summary>
        /// This method dispose of the bomb, destroying the physics object, and removing the bomb and its mesh from the scenegraph
        /// </summary>
        public override void Dispose()
        {
            Physics.RemovePhysObj(physObj);
            physObj = null;

            GameNode.Parent.RemoveChild(GameNode);
            GameNode.DetachAllObjects();
            GameNode.Dispose();
            gameEntity.Dispose();
        }
    }
}
