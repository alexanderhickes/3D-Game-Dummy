using System;
using Mogre;

namespace Tutorial
{
    /// <summary>
    /// This class shows how to assemble a model using multiple meshes as a sub-graph of the scenegraph
    /// </summary>
    class CompoundModel
    {
 
        SceneManager mSceneMgr;

        Entity power;
        Entity hull;
        Entity sphere;

        SceneNode hullGroupNode;
        SceneNode wheelsGroupNode;
        SceneNode gunsGroupNode;

        Radian angle;
        Vector3 direction;
        float radius;

        const float maxTime = 2000f;        // Time when the animation have to be changed
        Timer time;                         // Timer for animation changes
        AnimationState animationState;      // Animation state, retrieves and store an animation from an Entity
        bool animationChanged;              // Flag which tells when the mesh animation has changed

        string animationName;               // Name of the animation to use
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

        public Vector3 Position
        {
            get { return hullGroupNode.Position; }
        }

        //SceneNode model;                            // Root for the sub-graph

        /// <summary>
        /// This method returns the root node for the sub-scenegraph representing the compound model
        /// </summary>
        public SceneNode Model
        {
            get { return model; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mSceneMgr">A reference to the scene graph</param>
        public CompoundModel(SceneManager mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;

            Load();
            AssembleModel();
        }

        /// <summary>
        /// This method loads the nodes and entities needed by the compound model
        /// </summary>
        private void Load()
        { }
        //    hullGroupNode = mSceneMgr.CreateSceneNode();
        //        hull = mSceneMgr.CreateEntity("Main.mesh");
        //        hull.GetMesh().BuildEdgeList();
                
        //        power = mSceneMgr.CreateEntity("PowerCells.mesh");
        //        power.GetMesh().BuildEdgeList();

        //    wheelsGroupNode = mSceneMgr.CreateSceneNode();
        //        sphere = mSceneMgr.CreateEntity("Sphere.mesh");
        //        sphere.GetMesh().BuildEdgeList();

        //    gunsGroupNode = mSceneMgr.CreateSceneNode();

        //    model = mSceneMgr.CreateSceneNode();
        //}

        /// <summary>
        /// This method assemble the model attaching the entities to 
        /// each node and appending the nodes to each other
        /// </summary>
        private void AssembleModel()
        {
            hullGroupNode.AttachObject(hull);
            hullGroupNode.AttachObject(power);
            wheelsGroupNode.AttachObject(sphere);

            hullGroupNode.AddChild(wheelsGroupNode);
            hullGroupNode.AddChild(gunsGroupNode);
            model.AddChild(hullGroupNode);
            
            mSceneMgr.RootSceneNode.AddChild(model);
        }

        public void Animate(FrameEvent evt)
        {
            CircularMotion(evt);
            AnimateMesh(evt);
        }

        private void AnimationSetup()
        {
            radius = 0.01f;
            direction = Vector3.ZERO;
            angle = 0f;

            time = new Timer();
            PrintAnimationNames();
            animationChanged = false;
            animationName = "Walk";
            LoadAnimation();
        }

        private void HasAnimationChanged(string newName)
        {
            if (newName != animationName)
                animationChanged = true;
        }

        private void PrintAnimationNames()
        {
            AnimationStateSet animStateSet = hull.AllAnimationStates;     // Getd the set of animation states in the Entity
            AnimationStateIterator animIterator = animStateSet.GetAnimationStateIterator();  // Iterates through the animation states

            while (animIterator.MoveNext())                                       // Gets the next animation state in the set
            {
                Console.WriteLine(animIterator.CurrentKey);                      // Print out the animation name in the current key
            }
        }

        private bool IsValidAnimationName(string newName)
        {
            bool nameFound = false;

            AnimationStateSet animStateSet = hull.AllAnimationStates;
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

        private void changeAnimationName()
        {
            switch ((int)Mogre.Math.RangeRandom(0, 4.5f))       // Gets a random number between 0 and 4.5f
            {
                case 0:
                    {
                        AnimationName = "Walk";                 // I use the porperty here instead of the field to determine whether I am actualy changing the animation
                        break;
                    }
                case 1:
                    {
                        AnimationName = "Shoot";
                        break;
                    }
                case 2:
                    {
                        AnimationName = "Idle";
                        break;
                    }
                case 3:
                    {
                        AnimationName = "Slump";
                        break;
                    }
                case 4:
                    {
                        AnimationName = "Die";
                        break;
                    }
                    //case 5:
                    //    {
                    //        AnimationName = "Walk";
                    //        AnimationName = "Die";
                    //        break;
                    //    }
            }
        }

        private void LoadAnimation()
        {
            animationState = hull.GetAnimationState(animationName);
            animationState.Loop = true;
            animationState.Enabled = true;
        }

        private void CircularMotion(FrameEvent evt)
        {
            angle += (Radian)evt.timeSinceLastFrame;
            direction = radius * new Vector3(Mogre.Math.Cos(angle), 0, Mogre.Math.Sin(angle));
            hullGroupNode.Translate(direction);
            hullGroupNode.Yaw(-evt.timeSinceLastFrame);
            //hullGroupNode.Pitch(-evt.timeSinceLastFrame);
            //hullGroupNode.Roll(-evt.timeSinceLastFrame);
        }

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
        /// This method moves the model as a whole
        /// </summary>
        /// <param name="direction">The direction along which move the model</param>
        public void Move(Vector3 direction)
        {
            model.Translate(direction);             // Notice that only the root od the sub-scenegraph is transformed, 
                                                    // all the sub-nodes are tranformed as a consequence of this transformation
        }

        /// <summary>
        /// This method rotate the model as a whole
        /// </summary>
        /// <param name="quaternion">The quaternion describing the rotation</param>
        /// <param name="transformSpace">The transformation on which rotate the model</param>
        public void Rotate(Quaternion quaternion, Node.TransformSpace transformSpace = Node.TransformSpace.TS_LOCAL)
        {
            model.Rotate(quaternion, transformSpace);
        }

        /// <summary>
        /// This method detaches and dispode of all the elements of the compound model
        /// </summary>
        public void Dispose()
        {
            if (wheelsGroupNode != null)                     // Start removing from the leaves of the sub-graph
            {
                if (wheelsGroupNode.Parent != null)
                    wheelsGroupNode.Parent.RemoveChild(wheelsGroupNode);
                wheelsGroupNode.DetachAllObjects();
                wheelsGroupNode.Dispose();
                sphere.Dispose();
            }

            if (hullGroupNode != null)
            {
                if (hullGroupNode.Parent != null)
                    hullGroupNode.Parent.RemoveChild(hullGroupNode);
                hullGroupNode.DetachAllObjects();
                hullGroupNode.Dispose();
                hull.Dispose();
            }

            if (model != null)                      // Stop removing with the sub-graph root
            {
                if (model.Parent != null)
                    model.Parent.RemoveChild(model);
                model.Dispose();
            }
        }
    }
}
