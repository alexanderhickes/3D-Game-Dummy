using System;
using Mogre;
using PhysicsEng;

namespace Tutorial
{
    class LifePU : PowerUp
    {

        public LifePU(SceneManager mSceneMgr, Stat life) : base(mSceneMgr)
        {
            life = new Stat();
            increase = 1;
        }

        protected override void LoadModel()
        {
            RemoveMe = false;
            gameEntity = mSceneMgr.CreateEntity("ogrehead.mesh");

            GameNode = mSceneMgr.CreateSceneNode();
            GameNode.AttachObject(gameEntity);
            mSceneMgr.RootSceneNode.AddChild(GameNode);

            physObj = new PhysObj(10, "PowerUp", 0.1f, 0.5f);
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
            RemoveMe = IsCollidingWith("Player");
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
