using Mogre;
using System;
using PhysicsEng;

namespace Tutorial
{
    class PlayerModel : CharacterModel
    {

        ModelElement model;

        ModelElement powerElement;

        ModelElement hullGroupElement;
        ModelElement wheelsGroupElement;
        ModelElement gunGroupElement;

        public PlayerModel(SceneManager mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;
            LoadModelElements();
            AssembleModel();
            mSceneMgr.RootSceneNode.AddChild(gameNode);
        }

        protected override void LoadModelElements()
        {
            model = new ModelElement(mSceneMgr, "");

            hullGroupElement = new ModelElement(mSceneMgr, "Main.mesh");
            wheelsGroupElement = new ModelElement(mSceneMgr, "Sphere.mesh");

            powerElement = new ModelElement(mSceneMgr, "PowerCells.mesh");

            gunGroupElement = new ModelElement(mSceneMgr, "");
        }

        protected override void AssembleModel()
        {

            hullGroupElement.AddChild(powerElement.GameNode);
            hullGroupElement.AddChild(wheelsGroupElement.GameNode);
            hullGroupElement.AddChild(gunGroupElement.GameNode);

            model.AddChild(hullGroupElement.GameNode);

            gameNode = model.GameNode;

            physObj = new PhysObj(10, "Player", 0.1f, 0.5f, 0.3f);

            physObj.AddForceToList(new WeightForce(physObj.InvMass));
            physObj.AddForceToList(new FrictionForce(physObj));

            physObj.SceneNode = gameNode;
            Physics.AddPhysObj(physObj);
        }

        public void AttachGun(Gun gun)
        {
            if (gunGroupElement.GameNode.NumChildren() != 0)
            {
                GameNode.RemoveAllChildren();
            }

            gunGroupElement.GameNode.AddChild(gun.GameNode);
        }

        public override void Move(Vector3 direction)
        {
            GameNode.Translate(direction);
            physObj.Translate(direction);
        }

        public override void Dispose()
        {
            model.Dispose();

            powerElement.Dispose();

            wheelsGroupElement.Dispose();
            gunGroupElement.Dispose();
            hullGroupElement.Dispose();

            Physics.RemovePhysObj(physObj);
            physObj = null;
        }
    }
}
