using System;
using Mogre;

using PhysicsEng;

namespace Tutorial
{
    /// <summary>
    /// This class implements a robot
    /// </summary>
    class Robot : Enemy
    {
        //SceneNode controlNode;

        ModelElement robotElement;

        Radian angle;           // Angle for the mesh rotation
        Vector3 direction;      // Direction of motion of the mesh for a single frame
        float radius;           // Radius of the circular trajectory of the mesh

        const float maxTime = 2000f;        // Time when the animation have to be changed
        Timer time;                         // Timer for animation changes
        AnimationState animationState;      // Animation state, retrieves and store an animation from an Entity
        bool animationChanged;              // Flag which tells when the mesh animation has changed
        string animationName;               // Name of the animation to use

        bool removeMe;

        /// <summary>
        /// Write only. This property allows to change the animation 
        /// passing the name of one of the animations in the animation state set
        /// </summary>
        public string AnimationName
        {
            set
            {
                HasAnimationChanged(value);
                if (IsValidAnimationName(value))
                    animationName = value;
                else
                    animationName = "Idle";
            }
        }

        /// <summary>
        /// Read only. This property gets the postion of the robot in the scene
        /// </summary>
        //public Vector3 Position
        //{
        //    get { return robotElement.GameNode.Position; }
        //}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mSceneMgr">A reference to the scene manager</param>
        
        public Robot(SceneManager mSceneMgr, Stat lives) : base(mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;
            this.stat = lives;
            decrease = 1;
            Load();
            AnimationSetup();
        }

        /// <summary>
        /// This method loads the mesh and attaches it to a node and to the schenegraph
        /// </summary>
        private void Load()
        {
            removeMe = false;
            robotElement = new ModelElement(mSceneMgr, "robot.mesh");

            float radius = 10;
            robotElement.GameNode.Position -= radius * Vector3.UNIT_Y;

            mSceneMgr.RootSceneNode.AddChild(robotElement.GameNode);

            physObj = new PhysObj(radius, "Robot", 0.1f, 0.7f, 0.3f);
            physObj.SceneNode = robotElement.GameNode;

            physObj.AddForceToList(new WeightForce(physObj.InvMass));
            physObj.AddForceToList(new FrictionForce(physObj));
            Physics.AddPhysObj(physObj);
        }

        /// <summary>
        /// This method detaches the robot node from the scene graph and destroies it and the robot enetity
        /// </summary>
        public void Dispose()
        {
            robotElement.GameNode.RemoveAllChildren();
            robotElement.GameNode.Parent.RemoveChild(robotElement.GameNode);
            robotElement.GameNode.DetachAllObjects();
            robotElement.GameNode.Dispose();
            robotElement.GameEntity.Dispose();

            Physics.RemovePhysObj(physObj);
            physObj = null;
        }

        /// <summary>
        /// This methods set the position of the robot
        /// </summary>
        /// <param name="position"></param>
        public override void SetPosition(Vector3 position)
        {
            robotElement.GameNode.Translate(position);
        }

        /// <summary>
        /// This methods set the initial position of the robot
        /// </summary>
        /// <param name="position"></param>
        public void InitialPosition(Vector3 position)
        {
            robotElement.GameNode.Translate(position);
            //mSceneMgr.RootSceneNode.AddChild(robotElement.GameNode);
            //controlNode.AddChild(robotElement.GameNode);
            //controlNode.Position += radius * Vector3.UNIT_Y;
            //mSceneMgr.RootSceneNode.AddChild(controlNode);
        }

        /// <summary>
        /// This method set up all the field needed for animation
        /// </summary>
        private void AnimationSetup()
        {
            radius = 3f;
            direction = Vector3.ZERO;
            angle = 0f;

            time = new Timer();
            PrintAnimationNames();
            animationChanged = false;
            animationName = "Walk";
            LoadAnimation();
        }

        /// <summary>
        /// This method this method makes the mesh move in circle
        /// </summary>
        /// <param name="evt">A frame event which can be used to tune the animation timings</param>
        private void CircularMotion(FrameEvent evt)
        {
            angle += (Radian)evt.timeSinceLastFrame;
            direction = radius * new Vector3(Mogre.Math.Cos(angle), 0, Mogre.Math.Sin(angle));
            robotElement.GameNode.Translate(direction);
            robotElement.GameNode.Yaw(-evt.timeSinceLastFrame);
        }

        /// <summary>
        /// This method sets the animationChanged field to true whenever the animation name changes
        /// </summary>
        /// <param name="newName"> The new animation name </param>
        private void HasAnimationChanged(string newName)
        {
            if (newName != animationName)
                animationChanged = true;
        }

        /// <summary>
        /// This method prints on the console the list of animation tags
        /// </summary>
        private void PrintAnimationNames()
        {
            AnimationStateSet animStateSet = robotElement.GameEntity.AllAnimationStates;                    // Getd the set of animation states in the Entity
            AnimationStateIterator animIterator = animStateSet.GetAnimationStateIterator();     // Iterates through the animation states

            while (animIterator.MoveNext())                                                     // Gets the next animation state in the set
            {
                Console.WriteLine(animIterator.CurrentKey);                                     // Print out the animation name in the current key
            }
        }

        /// <summary>
        /// This method deternimes whether the name inserted is in the list of valid animation names
        /// </summary>
        /// <param name="newName">An animation name</param>
        /// <returns></returns>
        private bool IsValidAnimationName(string newName)
        {
            bool nameFound = false;

            AnimationStateSet animStateSet = robotElement.GameEntity.AllAnimationStates;
            AnimationStateIterator animIterator = animStateSet.GetAnimationStateIterator();

            while (animIterator.MoveNext() && !nameFound)
            {
                if (newName == animIterator.CurrentKey)
                {
                    nameFound = true;
                }
            }

            return nameFound;
        }

        /// <summary>
        /// This method changes the animation name randomly
        /// </summary>
        private void changeAnimationName()
        {
            switch ((int)Mogre.Math.RangeRandom(0, 4.5f))       // Gets a random number between 0 and 4.5f
            {
                case 0:
                    {
                        animationName = "Walk";
                        break;
                    }
                case 1:
                    {
                        animationName = "Shoot";
                        break;
                    }
                case 2:
                    {
                        animationName = "Idle";
                        break;
                    }
                case 3:
                    {
                        animationName = "Slump";
                        break;
                    }
                case 4:
                    {
                        animationName = "Die";
                        break;
                    }
            }
        }

        /// <summary>
        /// This method loads the animation from the animation name
        /// </summary>
        private void LoadAnimation()
        {
            animationState = robotElement.GameEntity.GetAnimationState(animationName);
            animationState.Loop = true;
            animationState.Enabled = true;
        }

        /// <summary>
        /// This method puts the mesh in motion
        /// </summary>
        /// <param name="evt">A frame event which can be used to tune the animation timings</param>
        private void AnimateMesh(FrameEvent evt)
        {
            if (time.Milliseconds > maxTime)
            {
                changeAnimationName();
                time.Reset();
            }

            if (animationChanged)
            {
                LoadAnimation();
                animationChanged = false;
            }

            animationState.AddTime(evt.timeSinceLastFrame);
        }

        /// <summary>
        /// This method animates the robot mesh
        /// </summary>
        /// <param name="evt">A frame event which can be used to tune the animation timings</param>
        public override void Animate(FrameEvent evt)
        {
            CircularMotion(evt);

            AnimateMesh(evt);

            base.update(evt);
        }

        /// <summary>
        /// This method adds a child to the robot node
        /// </summary>
        /// <param name="child">The scene node to be set as a child</param>
        public void AddChild(SceneNode child)
        {
            robotElement.GameNode.AddChild(child);
        }

        /// <summary>
        /// This method moves the robot in the given direction
        /// </summary>
        /// <param name="direction">The direction along which move the robot</param>
        public override void Move(Vector3 direction)
        {
            robotElement.GameNode.Translate(direction);
        }

        /// <summary>
        /// This method rotate the robot accordingly  with the given angles
        /// </summary>
        /// <param name="angles">The angles by which rotate the robot along each main axis</param>
        public void Rotate(Vector3 angles)
        {
            robotElement.GameNode.Yaw(angles.x);
        }
    }
}
