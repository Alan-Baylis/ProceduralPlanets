using UnityEngine;
using System.Collections;

public class MeshAlteration {

    public static void faceUp(float size, Vector3 position, Vector2 texture) {
        Vector3[] newVertices;
        Vector2[] newUV;
        int[] newTriangles;

        newVertices = new Vector3[4]{
            
            //Up
            new Vector3(size/2,size/2,size/2),
            new Vector3(size/2,size/2,-size/2),
            new Vector3(-size/2,size/2,-size/2),
            new Vector3(-size/2,size/2,size/2)
        };

        newTriangles = new int[6]{

            //Up
            0, 1, 3, 1, 2, 3
        };
    }

    public static GameObject Cube(float size, Vector3 position) {
        GameObject value = new GameObject();
        MeshFilter meshF = value.AddComponent<MeshFilter>();
        MeshRenderer meshR = value.AddComponent<MeshRenderer>();
        MeshCollider meshC = value.AddComponent<MeshCollider>();
        //Material mat = (Material)Resources.Load("Grass", typeof(Material));
        //meshR.material = mat;

        Vector3[] newVertices;
        Vector2[] newUV = meshF.mesh.uv;
        int[] newTriangles;

        newVertices = new Vector3[24]{
            
            //Up
            new Vector3(size/2,size/2,size/2),
            new Vector3(size/2,size/2,-size/2),
            new Vector3(-size/2,size/2,-size/2),
            new Vector3(-size/2,size/2,size/2),
            
            //Down
            new Vector3(size/2,-size/2,-size/2),
            new Vector3(size/2,-size/2,size/2),
            new Vector3(-size/2,-size/2,size/2),
            new Vector3(-size/2,-size/2,-size/2),

            //Forward
            new Vector3(size/2,-size/2,size/2),
            new Vector3(size/2,size/2,size/2),
            new Vector3(-size/2,size/2,size/2),
            new Vector3(-size/2,-size/2,size/2),

            //Back
            new Vector3(size/2,size/2,-size/2),
            new Vector3(size/2,-size/2,-size/2),
            new Vector3(-size/2,-size/2,-size/2),
            new Vector3(-size/2,size/2,-size/2),

            //Right
            new Vector3(size/2,size/2,-size/2),
            new Vector3(size/2,size/2,size/2),
            new Vector3(size/2,-size/2,size/2),
            new Vector3(size/2,-size/2,-size/2),

            //Left
            new Vector3(-size/2,size/2,size/2),
            new Vector3(-size/2,size/2,-size/2),
            new Vector3(-size/2,-size/2,-size/2),
            new Vector3(-size/2,-size/2,size/2)
        };

        newTriangles = new int[36]{

            //Up
            0, 1, 3, 1, 2, 3,

            //Down
            4, 5, 7, 5, 6, 7,

            //Forward
            8, 9, 11, 9, 10, 11,

            //Back
            12, 13, 15, 13, 14, 15,

            //Right
            16, 17, 19, 17, 18, 19,

            //Left
            20, 21, 23, 21, 22, 23
        };

        meshF.mesh.Clear();

        meshF.mesh.vertices = newVertices;
        meshF.mesh.triangles = newTriangles;
        meshF.mesh.uv = newUV;

        meshF.mesh.RecalculateBounds();
        meshF.mesh.RecalculateNormals();

        meshC.sharedMesh = null;
        meshC.sharedMesh = meshF.mesh;

        value.transform.position = position;

        return value;
    }

}
