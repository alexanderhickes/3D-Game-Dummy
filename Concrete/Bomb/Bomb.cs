using System;
using Mogre;
using PhysicsEng;

namespace Tutorial
{
    class Bomb : Projectile
    {
        SceneManager mSceneManager;

        ModelElement bombElement;

        //PhysObj physObj;

        //bool removeMe;
        ///// <summary>
        ///// Read only. This property gets whether the bomb should be removed from the game
        ///// </summary>
        //public bool RemoveMe
        //{
        //    get { return removeMe; }
        //}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mSceneMgr">A reference to the SceneManager</param>
        public Bomb(SceneManager mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;
            healthDamage = 50;
            shieldDamage = 25;
            speed = 10;
            Load();
        }

        protected override void Load()
        {
            
            RemoveMe = false;
            bombElement = new ModelElement(mSceneMgr, "Bomb.mesh");
            bombElement.GameNode.Scale(2, 2, 2);
            mSceneMgr.RootSceneNode.AddChild(bombElement.GameNode);

            physObj = new PhysObj(10, "Bomb", 0.1f, 0.5f);
            physObj.SceneNode = bombElement.GameNode;
            physObj.AddForceToList(new WeightForce(physObj.InvMass));

            Physics.AddPhysObj(physObj);
        }

        //private void AddBomb()
        //{
        //    Console.WriteLine("Bomb should have dropped");
        //    Bomb bomb = new Bomb(mSceneMgr);
        //    bomb.SetPosition(new Vector3(Mogre.Math.RangeRandom(0, 100), 100, Mogre.Math.RangeRandom(0, 100)));
        //    bombs.Add(bomb);
        //}

        /// <summary>
        /// This method set the position of the bomb in the given location
        /// </summary>
        /// <param name="position">The location where to position the bomb</param>
        public override void SetPosition(Vector3 position)
        {
            bombElement.GameNode.Position = position;
            physObj.Position = position;
        }

        /// <summary>
        /// This method update the bomb state
        /// </summary>
        /// <param name="evt"></param>
        public override void Update(FrameEvent evt)
        {
            RemoveMe = IsCollidingWith("Robot");
        }


        /// <summary>
        /// This method determine wheter the bomb is colliding with a named object  type
        /// </summary>
        /// <param name="objName">The name of the potential colliding object</param>
        /// <returns>True if a collision with the named object type happens, false otherwhise</returns>
        private bool IsCollidingWith(string objName)
        {
            bool isColliding = false;
            foreach (Contacts c in physObj.CollisionList)
            {
                if (c.colliderObj.ID == objName || c.collidingObj.ID == objName)
                {
                    isColliding = true;
                    break;
                }
            }
            return isColliding;
        }


        /// <summary>
        /// This method dispose of the bomb, destroying the physics object, and removing the bomb and its mesh from the scenegraph
        /// </summary>
        public override void Dispose()
        {
            Physics.RemovePhysObj(physObj);
            physObj = null;

            bombElement.GameNode.Parent.RemoveChild(GameNode);
            bombElement.GameNode.DetachAllObjects();
            bombElement.GameNode.Dispose();
            bombElement.GameEntity.Dispose();
        }
    }
}
