using System;
using Mogre;
using PhysicsEng;

namespace Tutorial
{
    class CollectableGun : Collectable
    {

        SceneNode collectabelGunNode;

        Gun gun;

        public Gun Gun
        {
            get { return gun; }
        }

        Armoury playerArmoury;

        public Armoury PlayerArmoury
        {
            set { playerArmoury = value; }
        }

        public CollectableGun(SceneManager mSceneMgr, Gun gun, Armoury playerArmoury)
        {
            
            this.mSceneMgr = mSceneMgr;
            this.gun = gun;
            this.playerArmoury = playerArmoury;

            // Initialize the gameNode here, scale it by 1.5f using the Scale funtion, and add as its child the gameNode contained in the Gun object.
            // Finally attach the gameNode to the sceneGraph.

            collectabelGunNode = mSceneMgr.CreateSceneNode();
            collectabelGunNode.Scale(1.5f, 1.5f, 1.5f);
            collectabelGunNode.AddChild(Gun.GameNode);
            mSceneMgr.RootSceneNode.AddChild(collectabelGunNode);

            // Here goes the link to the physics engine
            // (ignore until week 8) ...

            physObj = new PhysObj(10, "CollectabelGun", 0.1f, 0.5f);
            physObj.SceneNode = collectabelGunNode;
            physObj.AddForceToList(new WeightForce(physObj.InvMass));

            Physics.AddPhysObj(physObj);
        }

        public override void Update(FrameEvent evt)
        {
            Animate(evt);
            //Here goes the collision detection with the player
            // (ignore until week 8) ...

            (gun.GameNode.Parent).RemoveChild(gun.GameNode.Name);
            playerArmoury.AddGun(gun);

            Dispose();

            base.Update(evt);
        }

        public override void Animate(FrameEvent evt)
        {
            collectabelGunNode.Rotate(new Quaternion(Mogre.Math.AngleUnitsToRadians(evt.timeSinceLastFrame * 10), Vector3.UNIT_Y));
        }
    }
}
