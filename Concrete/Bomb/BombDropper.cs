using System;
using System.Collections;
using Mogre;
using System.Collections.Generic;

namespace Tutorial
{
    class BombDropper : Gun
    {
        ModelElement bombDropperElement;

        public List<Bomb> bombs;
        public List<Bomb> bombsToRemove;



        public BombDropper(SceneManager mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;
            maxAmmo = 3;
            ammo = new Stat();
            ammo.InitValue(maxAmmo);
            LoadModel();

            bombs = new List<Bomb>();
            bombsToRemove = new List<Bomb>();
        }

        protected override void LoadModel()
        {
            bombDropperElement = new ModelElement(mSceneMgr, "BombDropperGun.mesh");        
        }

        public override void Fire()
        {
            //if (!ammo.Value)
            if (ammo.Value != 0)
            {
                Bomb bomb = new Bomb(mSceneMgr);
                bomb.SetPosition(GunPosition() + 1 * GunDirection());
                ammo.Decrease(1);
            }
        }

        public void AddBomb()
        {
            Console.WriteLine("Bomb should have dropped");
            Bomb bomb = new Bomb(mSceneMgr);
            bomb.SetPosition(new Vector3(Mogre.Math.RangeRandom(0, 100), 100, Mogre.Math.RangeRandom(0, 100)));
            bombs.Add(bomb);
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
