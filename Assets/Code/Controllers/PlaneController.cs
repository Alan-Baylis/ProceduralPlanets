using UnityEngine;
using System.Collections;
using DevconTools;

public class PlaneController : MonoBehaviour {

    // Use this for initialization
    void Start() {
        changeVerts();
    }

    // Update is called once per frame
    void Update() {
        //changeVerts(xO, yO);
        //xO += 1;
        //yO += 1;
    }

    //changes all of the verts of the plane.
    void changeVerts() {
        Vector3[] newVertices;
        Vector2[] newUV;
        int[] newTriangles;
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Mesh colMesh = GetComponent<MeshCollider>().sharedMesh;

        newVertices = mesh.vertices;
        newUV = mesh.uv;
        newTriangles = mesh.triangles;
        mesh.Clear();

        for (int i = 0; i < newVertices.Length; i++) {
            Vector3 thisVertice = newVertices[i];
            Vector3 realVertice = transform.TransformPoint(thisVertice);
            newVertices[i] = new Vector3(thisVertice.x,
                                    pnng.smoothNoise((int)realVertice.x, (int)realVertice.z, 8, 1, 10),
                                    thisVertice.z);
            //Debug.Log(newVertices[i]);
            //((pnng.smoothNoise2D(thisVertice.x+xOffset, thisVertice.y+yOffset, 1, 1, 1)+1)/2)
        }

        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;
        colMesh = mesh;
        mesh.RecalculateBounds();
    }
}
