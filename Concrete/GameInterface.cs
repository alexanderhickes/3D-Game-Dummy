using System;
using System.Collections.Generic;
using Mogre;

namespace Tutorial
{
    /// <summary>
    /// This class implements an example of interface
    /// </summary>
    class GameInterface:HMD     // Game interface inherits form the Head Mounted Dispaly (HMD) class
    {
        private PanelOverlayElement panel;
        private OverlayElement scoreText;
        private OverlayElement timeText;

        private OverlayElement healthBar;
        private OverlayElement shieldBar;
        private Overlay overlay3D;
        private Entity lifeEntity;
        private List<SceneNode> lives;
        private Timer time;
        public Timer Time
        {
            set { time = value; }
        }
        
        private float hRatio;
        private float sRatio;
        private string score = "Score: ";
        private string timeDisplay = "Time: ";

        public bool gameWon = false;
        private OverlayElement gameWonText;
        private string wonDisplay = "Game Won!";
        public bool gameLost = false;
        private OverlayElement gameLostText;
        private string lostDisplay = "Game Over!";
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mSceneMgr">A reference of a scene manager</param>
        /// <param name="playerStats">A reference to a character stats</param>
        public GameInterface(SceneManager mSceneMgr, 
            RenderWindow mWindow, CharacterStats playerStats)
            : base(mSceneMgr, mWindow, playerStats)  // this calls the constructor of the parent class
        {
            Load("GameInterface");
        }

        /// <summary>
        /// This method initializes the element of the interface
        /// </summary>
        /// <param name="name"> A name to pass to generate the overaly </param>
        protected override void Load(string name)
        {
            base.Load(name);

            lives = new List<SceneNode>();

            healthBar = OverlayManager.Singleton.GetOverlayElement("HealthBar");
            hRatio = healthBar.Width / (float)characterStats.Health.Max;
            
            shieldBar = OverlayManager.Singleton.GetOverlayElement("ShieldBar");
            //sRatio = shieldBar.Width / (float)characterStats.Shield.Max;
            sRatio = shieldBar.Width / (float)characterStats.Shield.Value;
            scoreText = OverlayManager.Singleton.GetOverlayElement("ScoreText");
            scoreText.Caption = score;
            scoreText.Left = mWindow.Width * 0.5f;

            panel = 
           (PanelOverlayElement) OverlayManager.Singleton.GetOverlayElement("GreenBackground");
            panel.Width = mWindow.Width;
            LoadOverlay3D();

            time = new Timer();
            timeText = OverlayManager.Singleton.GetOverlayElement("TimeText");
            timeText.Caption = timeDisplay;
            timeText.Left = mWindow.Width * 0.5f;

            gameWonText = OverlayManager.Singleton.GetOverlayElement("GameWonText");
            gameWonText.Caption = wonDisplay;
            gameWonText.Left = mWindow.Width * 0.5f;
            //gameWonText.Top = mWindow.Height * 0.5f;
            gameWonText.Hide();

            gameLostText = OverlayManager.Singleton.GetOverlayElement("GameLostText");
            gameLostText.Caption = "Game Over!";
            gameLostText.Left = mWindow.Width * 0.5f;
            //gameLostText.Top = mWindow.Height * 0.5f;
            gameLostText.Hide();
        }

        /// <summary>
        /// This method initalize a 3D overlay
        /// </summary>
        private void LoadOverlay3D()
        {
            overlay3D = OverlayManager.Singleton.Create("3DOverlay");
            overlay3D.ZOrder = 15000;

            CreateHearts();

            overlay3D.Show();
        }

        /// <summary>
        /// This method generate as many hearts as the number of lives left
        /// </summary>
        private void CreateHearts()
        {
            for (int i = 0; i < characterStats.Lives.Value; i++)
                AddHeart(i);
        }
        
        /// <summary>
        /// This method add an heart to the 3D overlay
        /// </summary>
        /// <param name="n"> A numeric tag</param>
        private void AddHeart(int n)
        {
            SceneNode livesNode = CreateHeart(n);
            lives.Add(livesNode);
            overlay3D.Add3D(livesNode);
        }

        /// <summary>
        /// This method remove from the 3D overlay and destries the passed scene node
        /// </summary>
        /// <param name="life"></param>
        private void RemoveAndDestroyLife(SceneNode life)
        {
            overlay3D.Remove3D(life);
            lives.Remove(life);
            MovableObject heart = life.GetAttachedObject(0);
            life.DetachAllObjects();
            life.Dispose();
            heart.Dispose();
        }

        /// <summary>
        /// This method initializes the heart node and entity
        /// </summary>
        /// <param name="n"> A numeric tag used to determine the heart postion on sceen </param>
        /// <returns></returns>
        private SceneNode CreateHeart(int n)
        {
            lifeEntity = mSceneMgr.CreateEntity("Heart.mesh");
            lifeEntity.SetMaterialName("HeartHMD");
            SceneNode livesNode;
            livesNode = new SceneNode(mSceneMgr);
            livesNode.AttachObject(lifeEntity);
            livesNode.Scale(new Vector3(0.15f, 0.15f, 0.15f));
            livesNode.Position = new Vector3(3f, 5f, -8) - n * 0.5f * Vector3.UNIT_X; ;
            livesNode.SetVisible(true);
            return livesNode;
        }

        /// <summary>
        /// This method converts milliseconds in to minutes and second format mm:ss
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private string convertTime(float time)
        {
            string convTime;
            float secs = time / 1000f;
            int min = (int)(secs / 60);
            secs = (int) secs % 60f;
            if (secs < 10)
                convTime = min + ":0" + secs;
            else
                convTime = min + ":" + secs;
            return convTime;
        }

        /// <summary>
        /// This method updates the interface
        /// </summary>
        /// <param name="evt"></param>
        public override void Update(FrameEvent evt)
        {
            base.Update(evt);
            
            Animate(evt);

            if (lives.Count > characterStats.Lives.Value && characterStats.Lives.Value >= 0)
            {
                SceneNode life = lives[lives.Count - 1];
                RemoveAndDestroyLife(life);
                
            }
            if (lives.Count < characterStats.Lives.Value)
            {
                AddHeart(characterStats.Lives.Value);
            }

            if (((PlayerStats)characterStats).Score.Value == 100)
            {
                gameWonText.Show();
                gameWon = true;
            }

            if (((PlayerStats)characterStats).Lives.Value == 0)
            {
                gameLostText.Show();
                gameLost = true;
            }

            healthBar.Width = hRatio * characterStats.Health.Value;
            shieldBar.Width = (sRatio * characterStats.Shield.Value) / 2;
            scoreText.Caption = score + ((PlayerStats)characterStats).Score.Value;
            timeText.Caption = timeDisplay + convertTime(time.MillisecondsCPU);
        }

        /// <summary>
        /// This method animates the heart rotation
        /// </summary>
        /// <param name="evt"></param>
        protected override void Animate(FrameEvent evt)
        {
            foreach (SceneNode sn in lives)
                sn.Yaw(evt.timeSinceLastFrame);
        }

        /// <summary>
        /// This method disposes of the elements generated in the interface
        /// </summary>
        public override void Dispose()
        {
            List<SceneNode> toRemove = new List<SceneNode>();
            foreach (SceneNode life in lives)
            {
                toRemove.Add(life);
            }
            foreach (SceneNode life in toRemove)
            {
                RemoveAndDestroyLife(life);
            }
            lifeEntity.Dispose();
            toRemove.Clear();
            shieldBar.Dispose();
            healthBar.Dispose();
            scoreText.Dispose();
            //timeText.Dispose();
            panel.Dispose();
            overlay3D.Dispose();
            base.Dispose();
        }
    }
}
