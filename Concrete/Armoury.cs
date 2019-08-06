using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tutorial
{
    class Armoury
    {
        private bool gunChanged;
        public bool GunChanged
        {
            set { gunChanged = value; }
            get { return gunChanged; }
        }

        protected 
        Gun activeGun; 
        public Gun ActiveGun
        {
            set { activeGun = value; }
            get { return activeGun; }
        }

        private List<Gun> collectedGuns;
        private List<Gun> CollectedGuns
        {
            get { return collectedGuns; }
        }

        public Armoury()
        {
            collectedGuns = new List<Gun>();
        }

        public void ChangeGun(Gun activeGun)
        {
            this.activeGun = activeGun;
            gunChanged = true;
        }

        public void SwapGun(int i)
        {
            if (collectedGuns != null && activeGun != null)
            {
                ChangeGun(collectedGuns[i % collectedGuns.Count()]);
            }
        }

        public void AddGun(Gun gun)
        {
            bool add = true;

            foreach(Gun g in collectedGuns)
            {
                if (add == true && g.GetType() == gun.GetType())
                {
                    g.ReloadAmmo();
                    ChangeGun(g);
                    add = false;
                }
            }

            if (add == true)
            {
                ChangeGun(gun);
                collectedGuns.Add(gun);

            }
            else
            {
                gun.Dispose();
            }
        }

        public void Dispose()
        {
            collectedGuns.Clear();

            if(activeGun != null)
            {
                activeGun.Dispose();
            }
        }
    }
}
