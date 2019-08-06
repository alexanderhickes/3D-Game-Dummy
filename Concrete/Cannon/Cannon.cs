using System;
using Mogre;

namespace Tutorial
{
    class Cannon : Gun
    {
        public Cannon(SceneManager mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;
            maxAmmo = 10;
            ammo = new Stat();
            ammo.InitValue(maxAmmo);
            LoadModel();
        }

        protected override void LoadModel()
        {
            gameEntity = mSceneMgr.CreateEntity("CannonGun.mesh");
            GameNode = mSceneMgr.CreateSceneNode();
            GameNode.AttachObject(gameEntity);
        }

        public override void Fire()
        {
            if (ammo.Value != 0)
            {
                Cannon cannon = new Cannon(mSceneMgr);
                cannon.SetPosition(GunPosition() + 1 * GunDirection());
                ammo.Decrease(1);
            }
        }

        public override void ReloadAmmo()
        {
            if (ammo.Value < maxAmmo)
            {
                ammo.Increase(maxAmmo - ammo.Value - 1);
            }
        }
    }
}
