using System;
using Mogre;
using PhysicsEng;

namespace Tutorial
{
    class RedGem : Gem
    {
        ModelElement redGemElement;

        public RedGem(SceneManager mScenceMgr, Stat score) : base(mScenceMgr, score)
        {
            increase = 5;
            LoadModel();
        }

        protected override void LoadModel()
        {
            RemoveMe = false;

            redGemElement = new ModelElement(mSceneMgr, "Gem.mesh");
            redGemElement.GameEntity.SetMaterialName("Red");
            redGemElement.GameNode.Scale(2, 2, 2);
            mSceneMgr.RootSceneNode.AddChild(redGemElement.GameNode);

            physObj = new PhysObj(10, "Gem", 0.1f, 0.5f);
            physObj.SceneNode = redGemElement.GameNode;
            physObj.AddForceToList(new WeightForce(physObj.InvMass));
            physObj.AddForceToList(new FrictionForce(physObj));
            Physics.AddPhysObj(physObj);

        }

        public override void SetPosition(Vector3 position)
        {
            redGemElement.GameNode.Position = position;
            physObj.Position = position;
        }

        public override void Animate(FrameEvent evt)
        {
            redGemElement.GameNode.Yaw(Mogre.Math.AngleUnitsToRadians(20) * evt.timeSinceLastFrame);
        }

        public override void Dispose()
        {
            if (redGemElement.GameNode != null)
            {
                if (redGemElement.GameNode.Parent != null)
                {
                    redGemElement.GameNode.DetachAllObjects();
                    redGemElement.GameNode.RemoveAndDestroyAllChildren();
                    redGemElement.GameNode.Parent.RemoveChild(redGemElement.GameNode.Name);
                }
                redGemElement.GameNode.Dispose();
                Physics.RemovePhysObj(physObj);
            }

            if (redGemElement.GameEntity != null)
            {
                redGemElement.GameEntity.Dispose();
                Physics.RemovePhysObj(physObj);
            }
        }
    }
}
