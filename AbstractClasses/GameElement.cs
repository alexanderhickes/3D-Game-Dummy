using System;
using Mogre;
using PhysicsEng;

namespace Tutorial
{
    /// <summary>
    /// This abstract class is the basis of each element in the game. It contains the SceneNode, Enitity 
    /// and PhysObj necessary to build the game elements. It implements the Dispose method necessary to 
    /// destroy a GameElement at the end of its life in the game, and a method to position the GameElement 
    /// in the reference system of the game node's parent.
    /// The Enitity, the SceneNode and the PhysObj are to be initialized in the classes derived from this one.
    /// </summary>
    abstract class GameElement
    {
        protected SceneManager mSceneMgr;       // This protected field is to contain a reference to the SceneManager

        protected PhysObj physObj;            // This protected field is to contain physic object
        /// <summary>
        /// Read only. This property return the physic object associated to this game element
        /// </summary>
        public PhysObj PhysObj
        {
            get { return physObj; }
        }

        public Entity gameEntity;            // This protected field is to contain an entity in the game
        /// <summary>
        /// Read/Write. This property allows to read and write the game entity
        /// </summary>
        public Entity GameEntity
        {
            get { return gameEntity; }
            set { gameEntity = value; }
        }

        protected SceneNode gameNode;
        /// <summary>
        /// Read/Write. This property allows to read and write the game node
        /// </summary>
        public SceneNode GameNode
        {
            get { return gameNode; }
            set { gameNode = value; }
        }

        protected bool isMovable;               // This field determine whether this game element is movable
        /// <summary>
        /// Read/Write. This property allows to read and write whether this game element is movable
        /// </summary>
        public bool IsMovable
        {
            get { return isMovable; }
            set { isMovable = value; }
        }

        /// <summary>
        /// This method is detaches from the scene graph and derstroies the game node and the game entity
        /// </summary>
        public virtual void Dispose()
        {
            
            if (GameNode != null)
            {
                if (GameNode.Parent != null)
                {
                    GameNode.DetachAllObjects();
                    GameNode.RemoveAndDestroyAllChildren();
                    GameNode.Parent.RemoveChild(GameNode.Name);
                }
                GameNode.Dispose();
                //Physics.RemovePhysObj(physObj);
            }

            if (gameEntity != null)
            {
                gameEntity.Dispose();
                //Physics.RemovePhysObj(physObj);
            }
        }

        /// <summary>
        /// This virtual method allows to set the game element in the reference system of game node's parent
        /// </summary>
        /// <param name="position">The position in which to put the game element</param>
        virtual public void SetPosition(Vector3 position)
        {
            GameNode.Position = position;
            physObj.Position = gameNode.Position;
        }
    }
}
