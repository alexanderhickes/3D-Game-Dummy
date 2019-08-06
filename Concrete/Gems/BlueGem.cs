using System;
using Mogre;
using PhysicsEng;

namespace Tutorial
{
    class BlueGem : Gem
    {
        ModelElement blueGemElement;

        public BlueGem(SceneManager mScenceMgr, Stat score) : base(mScenceMgr, score)
        {
            increase = 20;
            LoadModel();
        }

        protected override void LoadModel()
        {
            RemoveMe = false;

            blueGemElement = new ModelElement(mSceneMgr, "Gem.mesh");
            blueGemElement.GameNode.Scale(2, 2, 2);
            mSceneMgr.RootSceneNode.AddChild(blueGemElement.GameNode);

            physObj = new PhysObj(10, "Gem", 0.1f, 0.5f, 5f);
            physObj.AddForceToList(new WeightForce(physObj.InvMass));
            physObj.AddForceToList(new FrictionForce(physObj));
            physObj.SceneNode = blueGemElement.GameNode;
            Physics.AddPhysObj(physObj);

        }

        public override void SetPosition(Vector3 position)
        {
            blueGemElement.GameNode.Position = position;
            physObj.Position = position;
        }

        public override void Animate(FrameEvent evt)
        {
            blueGemElement.GameNode.Yaw(Mogre.Math.AngleUnitsToRadians(30) * evt.timeSinceLastFrame);
        }

        public override void Dispose()
        {
            if (blueGemElement.GameNode != null)
            {
                if (blueGemElement.GameNode.Parent != null)
                {
                    blueGemElement.GameNode.DetachAllObjects();
                    blueGemElement.GameNode.RemoveAndDestroyAllChildren();
                    blueGemElement.GameNode.Parent.RemoveChild(blueGemElement.GameNode.Name);
                }
                blueGemElement.GameNode.Dispose();
                Physics.RemovePhysObj(physObj);
            }

            if (blueGemElement.GameEntity != null)
            {
                blueGemElement.GameEntity.Dispose();
                Physics.RemovePhysObj(physObj);
            }
        }
    }
}
