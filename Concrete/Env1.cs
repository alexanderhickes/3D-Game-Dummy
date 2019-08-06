using System;
using Mogre;

using PhysicsEng;

namespace Tutorial
{
    class Env1 : Environment
    {

        public Env1(SceneManager mSceneMgr, RenderWindow mWindow)
        {
            this.mSceneMgr = mSceneMgr;
            this.mWindow = mWindow;
        }

    }
}
