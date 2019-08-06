using System;
using Mogre;

namespace Tutorial
{
    class Cube
    {
        ManualObject manual;
        SceneManager mSceneMgr;

        public Cube(SceneManager mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;
        }

        public MeshPtr getCube(string cubeName, string materialName, float width, float height, float depth)
        {
            manual = mSceneMgr.CreateManualObject(cubeName + "_ManObj");
            manual.Begin(materialName, RenderOperation.OperationTypes.OT_TRIANGLE_LIST);

            // --- Fills the Vertex buffer and define the texture coordinates for each vertex ---

            //--- Vertex 0 ---
            manual.Position(new Vector3(.5f * width, .5f * height, .5f * depth));
            //manual.TextureCoord(new Vector2(0, 0));
            manual.TextureCoord(new Vector2(1, 1));

            //--- Vertex 1 ---
            manual.Position(new Vector3(.5f * width, -.5f * height, .5f * depth));
            //manual.TextureCoord(new Vector2(2, 0));
            manual.TextureCoord(new Vector2(1, 0));

            //--- Vertex 2 ---
            manual.Position(new Vector3(.5f * width, .5f * height, -.5f * depth));
            //manual.TextureCoord(new Vector2(0, 2));
            manual.TextureCoord(new Vector2(0, 1));

            //--- Vertex 3 ---
            manual.Position(new Vector3(.5f * width, -.5f * height, -.5f * depth));
            //manual.TextureCoord(new Vector2(2, 2));
            manual.TextureCoord(new Vector2(0, 0));


            //--- Vertex 4 ---
            manual.Position(new Vector3(-.5f * width, .5f * height, .5f * depth));
            //manual.TextureCoord(new Vector2(0, 0));
            //manual.TextureCoord(new Vector2(0, 1));

            //--- Vertex 5 ---
            manual.Position(new Vector3(-.5f * width, -.5f * height, .5f * depth));
            //manual.TextureCoord(new Vector2(2, 0));
            //manual.TextureCoord(new Vector2(0, 0));

            //--- Vertex 6 ---
            manual.Position(new Vector3(-.5f * width, .5f * height, -.5f * depth));
            //manual.TextureCoord(new Vector2(0, 2));
            //manual.TextureCoord(new Vector2(1, 1));

            //--- Vertex 7 ---
            manual.Position(new Vector3(-.5f * width, -.5f * height, -.5f * depth));
            //manual.TextureCoord(new Vector2(2, 2));
            //manual.TextureCoord(new Vector2(1, 0));


            // --- Fills the Index Buffer ---
            //--------Face 1----------
            manual.Index(0);
            manual.Index(1);
            manual.Index(2);

            manual.Index(2);
            manual.Index(1);
            manual.Index(3);

            //--------Face 2----------
            manual.Index(4);
            manual.Index(6);
            manual.Index(5);

            manual.Index(6);
            manual.Index(7);
            manual.Index(5);

            //--------Face 3----------
            manual.Index(0);
            manual.Index(4);
            manual.Index(1);

            manual.Index(1);
            manual.Index(4);
            manual.Index(5);

            //--------Face 4----------
            manual.Index(0);
            manual.Index(6);
            manual.Index(4);

            manual.Index(0);
            manual.Index(2);
            manual.Index(6);

            //--------Face 5----------
            manual.Index(6);
            manual.Index(2);
            manual.Index(3);

            manual.Index(6);
            manual.Index(3);
            manual.Index(7);

            //--------Face 5----------
            manual.Index(3);
            manual.Index(1);
            manual.Index(7);

            manual.Index(1);
            manual.Index(5);
            manual.Index(7);

            manual.End();
            return manual.ConvertToMesh(cubeName);
        }
    }
}
