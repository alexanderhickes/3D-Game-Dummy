using Mogre;
using System;

namespace Tutorial
{
    class Player : Character
    {

        Armoury playerArmoury;
        public Armoury PlayerArmoury
        {
            get { return playerArmoury; }
        }

        public Player(SceneManager mSceneMgr)
        {
            model = new PlayerModel(mSceneMgr);
            controller = new PlayerController(this);
            stats = new PlayerStats();
            playerArmoury = new Armoury();
        }

        public override void Shoot()
        {
            playerArmoury.ActiveGun.Fire();
        }

        public override void Update(FrameEvent evt)
        {
            controller.Update(evt);

            if (playerArmoury.GunChanged)
            {
                ((PlayerModel)model).AttachGun(playerArmoury.ActiveGun);
                playerArmoury.GunChanged = false;
            }
        }
    }
}
