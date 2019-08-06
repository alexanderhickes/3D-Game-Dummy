
using Mogre;
using Mogre.TutorialFramework;
using System;
using System.Collections.Generic;
using PhysicsEng;


namespace Tutorial
{
    class Level2: BaseApplication
    {
        Physics physics;

        Player player;
        InputsManager inputsManager = InputsManager.Instance;

        PlayerStats playerStats;

        //Bomb testBomb;
        BombDropper testGun;

        List<BlueGem> blueGems;
        List<BlueGem> blueGemsToRemove;

        List<RedGem> redGems;
        List<RedGem> redGemsToRemove;

        List<ShieldPU> shieldPU;
        List<ShieldPU> shieldPUToRemove;

        Environment environment;

        Robot robot;

        static public bool shoot;

        SceneNode cameraNode;

        GameInterface hudElement;

        protected override void CreateScene()
        {
            physics = new Physics();

            player = new Player(mSceneMgr);
            player.Model.SetPosition(new Vector3(50, 10, 100));
            playerStats = new PlayerStats();

            environment = new Environment(mSceneMgr, mWindow);

            cameraNode = mSceneMgr.CreateSceneNode();
            cameraNode.AttachObject(mCamera);
            player.Model.GameNode.AddChild(cameraNode);
            inputsManager.PlayerController = (PlayerController)player.Controller;

            hudElement = new GameInterface(mSceneMgr, mWindow, playerStats);
            mSceneMgr.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_MODULATIVE;

            testGun = new BombDropper(mSceneMgr);

            blueGems = new List<BlueGem>();
            blueGemsToRemove = new List<BlueGem>();
            for (int i = 0; i < 4; i++)
            {
                BlueGem aBlueGem = new BlueGem(mSceneMgr, playerStats.Score);
                aBlueGem.SetPosition(new Vector3(Mogre.Math.RangeRandom(-500, 500), 10, Mogre.Math.RangeRandom(-500, 500)));

                blueGems.Add(aBlueGem);
            }

            redGems = new List<RedGem>();
            redGemsToRemove = new List<RedGem>();
            for (int i = 0; i < 4; i++)
            {
                RedGem aRedGem = new RedGem(mSceneMgr, playerStats.Score);
                aRedGem.SetPosition(new Vector3(Mogre.Math.RangeRandom(-500, 500), 10, Mogre.Math.RangeRandom(-500, 500)));

                redGems.Add(aRedGem);
            }

            shieldPU = new List<ShieldPU>();
            shieldPUToRemove = new List<ShieldPU>();
            for (int i = 0; i < 2; i++)
            {
                ShieldPU aShieldPU = new ShieldPU(mSceneMgr, playerStats.Shield);
                aShieldPU.SetPosition(new Vector3(Mogre.Math.RangeRandom(-500, 500), 10, Mogre.Math.RangeRandom(-500, 500)));

                shieldPU.Add(aShieldPU);
            }

            robot = new Robot(mSceneMgr, playerStats.Lives);
            robot.InitialPosition(new Vector3(-300, 0, -200));

            physics.StartSimTimer();
        }

        protected override void DestroyScene()
        {

            base.DestroyScene();
            player.Model.GameNode.Dispose();
            robot.Dispose();
            cameraNode.DetachAllObjects();
            cameraNode.Dispose();

            foreach (Bomb bomb in testGun.bombs)
            {
                bomb.Dispose();
            }

            foreach (BlueGem blueGem in blueGems)
            {
                blueGem.Dispose();
            }

            foreach (RedGem redGem in redGems)
            {
                redGem.Dispose();
            }

            foreach (ShieldPU shield in shieldPU)
            {
                shield.Dispose();
            }

            hudElement.Dispose();
            physics.Dispose();
            environment.Dispose();

        }

        protected override void CreateCamera()
        {
            mCamera = mSceneMgr.CreateCamera("PlayerCam");
            mCamera.Position = new Vector3(0, 100, -300);
            mCamera.LookAt(new Vector3(0, 0, 0));
            mCamera.NearClipDistance = 5;
            mCamera.FarClipDistance = 1000;
            mCamera.FOVy = new Degree(70);

            mCameraMan = new CameraMan(mCamera);
            mCameraMan.Freeze = true;
            //mCameraMan.Freeze = false;
        }

        protected override void CreateViewports()
        {
            Viewport viewport = mWindow.AddViewport(mCamera);
            viewport.BackgroundColour = ColourValue.Black;
            mCamera.AspectRatio = viewport.ActualWidth / viewport.ActualHeight;
        }

        protected override void UpdateScene(FrameEvent evt)
        {

            physics.UpdatePhysics(0.01f);
            base.UpdateScene(evt);

            if (shoot)
            {
                testGun.AddBomb();
            }

            foreach (Bomb bomb in testGun.bombs)
            {
                bomb.Update(evt);
                if (bomb.RemoveMe)
                    testGun.bombsToRemove.Add(bomb);
            }

            foreach (Bomb bomb in testGun.bombsToRemove)
            {
                testGun.bombs.Remove(bomb);
                bomb.Dispose();
            }
            testGun.bombsToRemove.Clear();

            foreach (BlueGem blueGem in blueGems)
            {
                blueGem.Update(evt);
                if (blueGem.RemoveMe)
                    blueGemsToRemove.Add(blueGem);
            }

            foreach (BlueGem blueGem in blueGemsToRemove)
            {
                blueGems.Remove(blueGem);
                blueGem.Dispose();
            }
            blueGemsToRemove.Clear();

            foreach (RedGem redGem in redGems)
            {
                redGem.Update(evt);
                if (redGem.RemoveMe)
                    redGemsToRemove.Add(redGem);
            }

            foreach (RedGem redGem in redGemsToRemove)
            {
                redGems.Remove(redGem);
                redGem.Dispose();
            }
            redGemsToRemove.Clear();

            foreach (ShieldPU shield in shieldPU)
            {
                shield.Update(evt);
                if (shield.RemoveMe)
                    shieldPUToRemove.Add(shield);
            }

            foreach (ShieldPU shield in shieldPUToRemove)
            {
                shieldPU.Remove(shield);
                shield.Dispose();
            }
            shieldPUToRemove.Clear();


            player.Update(evt);
            robot.Animate(evt);
            //robot2.Animate(evt);
            //robot3.Animate(evt);
            hudElement.Update(evt);

            shoot = false;
            mCamera.LookAt(player.Position);

            if (hudElement.gameWon)
            {
                hudElement.gameWon = false;
                Console.WriteLine("You Won :)");
                //DestroyScene();
            }

            if (hudElement.gameLost)
            {
                hudElement.gameLost = true;
                Console.WriteLine("You Lost :(");
                Console.WriteLine("Attempt to Destroying Scene...");
                DestroyScene();
            }
        }

        //private void AddBomb()
        //{
        //    Console.WriteLine("Bomb should have dropped");
        //    Bomb bomb = new Bomb(mSceneMgr);
        //    bomb.SetPosition(new Vector3(Mogre.Math.RangeRandom(0, 100), 100, Mogre.Math.RangeRandom(0, 100)));
        //    bombs.Add(bomb);
        //}

        protected override void CreateFrameListeners()
        {
            base.CreateFrameListeners();
            mRoot.FrameRenderingQueued += new FrameListener.FrameRenderingQueuedHandler(inputsManager.ProcessInput);
        }

        protected override void InitializeInput()
        {
            base.InitializeInput();
            int windowHandle;
            mWindow.GetCustomAttribute("WINDOW", out windowHandle);
            inputsManager.InitInput(ref windowHandle);
        }



    }
}