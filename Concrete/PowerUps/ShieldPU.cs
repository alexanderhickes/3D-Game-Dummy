using System;
using Mogre;
using PhysicsEng;

namespace Tutorial
{
    class ShieldPU : PowerUp
    {
        ModelElement shieldElement;
        //protected Stat shield;

        public ShieldPU(SceneManager mSceneMgr, Stat shield) : base(mSceneMgr)
        {
            this.stat = shield;
            increase = 25;
        }

        protected override void LoadModel()
        {
            RemoveMe = false;

            shieldElement = new ModelElement(mSceneMgr, "Shield.mesh");
            mSceneMgr.RootSceneNode.AddChild(shieldElement.GameNode);

            physObj = new PhysObj(10, "Shield", 0.1f, 0.5f);
            physObj.SceneNode = shieldElement.GameNode;
            physObj.AddForceToList(new WeightForce(physObj.InvMass));

            Physics.AddPhysObj(physObj);
        }

        /// <summary>
        /// This method set the position of the bomb in the given location
        /// </summary>
        /// <param name="position">The location where to position the bomb</param>
        public override void SetPosition(Vector3 position)
        {
            shieldElement.GameNode.Position = position;
            physObj.Position = position;
        }

        public override void Animate(FrameEvent evt)
        {
            shieldElement.GameNode.Yaw(Mogre.Math.AngleUnitsToRadians(30) * evt.timeSinceLastFrame);
        }

        /// <summary>
        /// This method dispose of the bomb, destroying the physics object, and removing the bomb and its mesh from the scenegraph
        /// </summary>
        public override void Dispose()
        {          

            if (shieldElement.GameNode != null)
            {
                if (shieldElement.GameNode.Parent != null)
                {
                    shieldElement.GameNode.DetachAllObjects();
                    shieldElement.GameNode.RemoveAndDestroyAllChildren();
                    shieldElement.GameNode.Parent.RemoveChild(shieldElement.GameNode.Name);
                }
                shieldElement.GameNode.Dispose();                
            }

            if (shieldElement.GameEntity != null)
            {
                shieldElement.GameEntity.Dispose();
            }

            //Physics.RemovePhysObj(physObj);
            //physObj = null;
        }
    }
}
