/* Purpose: This class takes the blockData dictionary and sets the blockType for each block within the chunk.
 * It then generates a mesh of all of the surfaces that are visible.
 * 
 * Special Notes: N/A.
 * 
 * Author: Devyn Cyphers; Devcon.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System;

public class TerrainController : MonoBehaviour {

    // Variables to use.
    private Dictionary<Vector3, int> blockData = new Dictionary<Vector3, int>();
    public List<Vector3> newVertices;
    public List<Vector2> newUV;
    public List<int> newTriangles;
    public TaskController taskCont;
    public GameObject master, bounds;
    private Mesh mesh;
    private MeshCollider col;
    private MeshRenderer ren;
    public int squareCount = 0;

    // This method takes setBlockData(Dictionary<vector3, int>) as the block data, it also passes along the setBounds(GameObject). This is called to start the class.
    public void PreGen(Dictionary<Vector3, int> setBlockData, GameObject setBounds) {

        // Set variables.
        mesh = gameObject.GetComponent<MeshFilter>().mesh;
        col = gameObject.GetComponent<MeshCollider>();
        ren = gameObject.GetComponent<MeshRenderer>();
        master = UnityTools.GetMasterController();
        taskCont = master.GetComponent<TaskController>();
        bounds = setBounds;
        blockData = setBlockData;

        // Start Coroutines.
        StartCoroutine(ThreadWork());
    }

    // This method updates the mesh and sets the gameobjects to it.
    public void UpdateMesh() {

        mesh.Clear();

        // get current mesh data.
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();

        // Calculate mesh.
        mesh.Optimize();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Update the collider mesh.
        col.sharedMesh = null;
        col.sharedMesh = mesh;

        // Set gameobject meshes to new mesh.
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshCollider>().sharedMesh = col.sharedMesh;

        // Set the material.
        Material mat = (Material)Resources.Load("Grass", typeof(Material));
        ren.material = mat;

        // Clear variables.
        newVertices.Clear();
        newUV.Clear();
        newTriangles.Clear();
    }

    // This Coroutine divides the work done into several frames, keeping FPS high.
    public IEnumerator ThreadWork() {
        int ticks = Environment.TickCount;

        foreach (KeyValuePair<Vector3, int> data in blockData) {
                CheckForAir(data);

            if (Environment.TickCount - ticks > 4096) {
                ticks = Environment.TickCount;
                yield return null;
            }
        }
        UpdateMesh();
        bounds.SetActive(false);
    }

    // This method takes data(KeyValuePair<Vector3, int>) and looks to see if there is an air blockType on each face, then adds to the mesh if so.
    private void CheckForAir(KeyValuePair<Vector3, int> data) {

        if (data.Value != 0) {
            int val;

            // Up.
            Vector3 alteredKey = data.Key + new Vector3(0, 1, 0);
            if (blockData.TryGetValue(alteredKey, out val)) {

                if (val == 0) {
                    FaceUp(new object[] { 1, data.Key, new Vector2(0, 0) });
                }
            }
            // Down.
            alteredKey = data.Key + new Vector3(0, -1, 0);
            if (blockData.TryGetValue(alteredKey, out val)) {
                if (val == 0) {
                    FaceDown(new object[] { 1, data.Key, new Vector2(0, 0) });
                }
            }
            // Forward.
            alteredKey = data.Key + new Vector3(0, 0, 1);
            if (blockData.TryGetValue(alteredKey, out val)) {
                if (val == 0) {
                    FaceForward(new object[] { 1, data.Key, new Vector2(0, 0) });
                }
            }
            // Back.
            alteredKey = data.Key + new Vector3(0, 0, -1);
            if (blockData.TryGetValue(alteredKey, out val)) {
                if (val == 0) {
                    FaceBack(new object[] { 1, data.Key, new Vector2(0, 0) });
                }
            }
            // Right.
            alteredKey = data.Key + new Vector3(1, 0, 0);
            if (blockData.TryGetValue(alteredKey, out val)) {
                if (val == 0) {
                    FaceRight(new object[] { 1, data.Key, new Vector2(0, 0) });
                }
            }
            // Left.
            alteredKey = data.Key + new Vector3(-1, 0, 0);
            if (blockData.TryGetValue(alteredKey, out val)) {
                if (val == 0) {
                    FaceLeft(new object[] { 1, data.Key, new Vector2(0, 0) });
                }
            }
        }
    }

    // Methods used to add a face to the mesh.
    #region FaceGenerationMethods.
    // Called to make a face on the mesh on the top of a block :: UP.
    public void FaceUp(object state) {
        object[] stateAr = state as object[];
        
        float size = (float)Convert.ToDouble(stateAr[0]);
        Vector3 position = (Vector3)stateAr[1];
        Vector2 texture = (Vector2)stateAr[2];

        // Add mesh vertices.
        lock (newVertices) {
            newVertices.Add(new Vector3(position.x + (size / 2), position.y + (size / 2), position.z + (size / 2)));
            newVertices.Add(new Vector3(position.x + (size / 2), position.y + (size / 2), position.z - (size / 2)));
            newVertices.Add(new Vector3(position.x - (size / 2), position.y + (size / 2), position.z - (size / 2)));
            newVertices.Add(new Vector3(position.x - (size / 2), position.y + (size / 2), position.z + (size / 2)));
        }

        // Add mesh triangles.
        lock (newTriangles) {
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 2);
            newTriangles.Add((squareCount * 4) + 3);
            newTriangles.Add((squareCount * 4));
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 3);
        }

        squareCount++;

    }

    // Called to make a face on the mesh on the bottom of a block :: DOWN.
    public void FaceDown(object state) {
        object[] stateAr = state as object[];
        
        float size = (float)Convert.ToDouble(stateAr[0]);
        Vector3 position = (Vector3)stateAr[1];
        Vector2 texture = (Vector2)stateAr[2];
        
        // Add mesh vertices.
        lock (newVertices) {
            newVertices.Add(new Vector3(position.x + (size / 2), position.y - (size / 2), position.z - (size / 2)));
            newVertices.Add(new Vector3(position.x + (size / 2), position.y - (size / 2), position.z + (size / 2)));
            newVertices.Add(new Vector3(position.x - (size / 2), position.y - (size / 2), position.z + (size / 2)));
            newVertices.Add(new Vector3(position.x - (size / 2), position.y - (size / 2), position.z - (size / 2)));
        }

        // Add mesh triangles.
        lock (newTriangles) {
            newTriangles.Add((squareCount * 4));
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 3);
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 2);
            newTriangles.Add((squareCount * 4) + 3);
        }
        squareCount++;
    }

    // Called to make a face on the mesh on the front of a block :: FORWARD.
    public void FaceForward(object state) {
        object[] stateAr = state as object[];
        
        float size = (float)Convert.ToDouble(stateAr[0]);
        Vector3 position = (Vector3)stateAr[1];
        Vector2 texture = (Vector2)stateAr[2];
        
        // Add mesh vertices.
        lock (newVertices) {
            newVertices.Add(new Vector3(position.x + (size / 2), position.y - (size / 2), position.z + (size / 2)));
            newVertices.Add(new Vector3(position.x + (size / 2), position.y + (size / 2), position.z + (size / 2)));
            newVertices.Add(new Vector3(position.x - (size / 2), position.y + (size / 2), position.z + (size / 2)));
            newVertices.Add(new Vector3(position.x - (size / 2), position.y - (size / 2), position.z + (size / 2)));
        }

        // Add mesh triangles.
        lock (newTriangles) {
            newTriangles.Add((squareCount * 4));
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 3);
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 2);
            newTriangles.Add((squareCount * 4) + 3);
        }

        squareCount++;
    }

    // Called to make a face on the mesh on the back of a block :: BACK.
    public void FaceBack(object state) {
        object[] stateAr = state as object[];
        
        float size = (float)Convert.ToDouble(stateAr[0]);
        Vector3 position = (Vector3)stateAr[1];
        Vector2 texture = (Vector2)stateAr[2];
        
        // Add mesh vertices.
        lock (newVertices) {
            newVertices.Add(new Vector3(position.x + (size / 2), position.y + (size / 2), position.z - (size / 2)));
            newVertices.Add(new Vector3(position.x + (size / 2), position.y - (size / 2), position.z - (size / 2)));
            newVertices.Add(new Vector3(position.x - (size / 2), position.y - (size / 2), position.z - (size / 2)));
            newVertices.Add(new Vector3(position.x - (size / 2), position.y + (size / 2), position.z - (size / 2)));
        }

        // Add mesh triangles.
        lock (newTriangles) {
            newTriangles.Add((squareCount * 4));
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 3);
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 2);
            newTriangles.Add((squareCount * 4) + 3);
        }

        squareCount++;        
    }

    // Called to make a face on the mesh on the top of a block :: RIGHT.
    public void FaceRight(object state) {
        object[] stateAr = state as object[];
        
        float size = (float)Convert.ToDouble(stateAr[0]);
        Vector3 position = (Vector3)stateAr[1];
        Vector2 texture = (Vector2)stateAr[2];
        
        // Add mesh vertices.
        lock (newVertices) {
            newVertices.Add(new Vector3(position.x + (size / 2), position.y + (size / 2), position.z - (size / 2)));
            newVertices.Add(new Vector3(position.x + (size / 2), position.y + (size / 2), position.z + (size / 2)));
            newVertices.Add(new Vector3(position.x + (size / 2), position.y - (size / 2), position.z + (size / 2)));
            newVertices.Add(new Vector3(position.x + (size / 2), position.y - (size / 2), position.z - (size / 2)));
        }

        // Add mesh triangles.
        lock (newTriangles) {
            newTriangles.Add((squareCount * 4));
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 3);
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 2);
            newTriangles.Add((squareCount * 4) + 3);
        }

        squareCount++;
    }

    // Called to make a face on the mesh on the top of a block :: LEFT.
    public void FaceLeft(object state) {
        object[] stateAr = state as object[];
        
        float size = (float)Convert.ToDouble(stateAr[0]);
        Vector3 position = (Vector3)stateAr[1];
        Vector2 texture = (Vector2)stateAr[2];
        
        // Add mesh vertices.
        lock (newVertices) {
            newVertices.Add(new Vector3(position.x - (size / 2), position.y + (size / 2), position.z + (size / 2)));
            newVertices.Add(new Vector3(position.x - (size / 2), position.y + (size / 2), position.z - (size / 2)));
            newVertices.Add(new Vector3(position.x - (size / 2), position.y - (size / 2), position.z - (size / 2)));
            newVertices.Add(new Vector3(position.x - (size / 2), position.y - (size / 2), position.z + (size / 2)));
        }

        // Add mesh triangles.
        lock (newTriangles) {
            newTriangles.Add((squareCount * 4));
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 3);
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 2);
            newTriangles.Add((squareCount * 4) + 3);
        }

        squareCount++;
    }
    #endregion
}