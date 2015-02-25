using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System;

public class TerrainController : MonoBehaviour {

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

    public void PreGen(Dictionary<Vector3, int> setBlockData, GameObject setBounds) {
        //Debug.Log("TerCont Started. blockData lenght: " + blockData.Count);
        mesh = gameObject.GetComponent<MeshFilter>().mesh;
        col = gameObject.GetComponent<MeshCollider>();
        ren = gameObject.GetComponent<MeshRenderer>();
        master = UnityTools.GetMasterController();
        taskCont = master.GetComponent<TaskController>();
        bounds = setBounds;
        blockData = setBlockData;
        Stopwatch timer = new Stopwatch();

        ThreadWork();
        //StartCoroutine(ThreadWork());

        UnityEngine.Debug.Log(timer.Elapsed);
    }

    public void UpdateMesh() {

        mesh.Clear();

        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        //mesh.uv = newUV.ToArray();

        mesh.Optimize();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        col.sharedMesh = null;
        col.sharedMesh = mesh;

        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshCollider>().sharedMesh = col.sharedMesh;

        Material mat = (Material)Resources.Load("Grass", typeof(Material));
        ren.material = mat;

        newVertices.Clear();
        newUV.Clear();
        newTriangles.Clear();
    }

    public void ThreadWork() {
        int ticks = Environment.TickCount;

        foreach (KeyValuePair<Vector3, int> data in blockData) {
            ThreadPool.QueueUserWorkItem(delegate {
                CheckForAir(data);
            });

            if (Environment.TickCount - ticks > 2000) {
                ticks = Environment.TickCount;
                //yield return null;
            }
        }
        UpdateMesh();
        bounds.SetActive(false);
    }

    private void CheckForAir(KeyValuePair<Vector3, int> data) {

        if (data.Value != 0) {
            int val;

            //Up
            Vector3 alteredKey = data.Key + new Vector3(0, 1, 0);
            if (blockData.TryGetValue(alteredKey, out val)) {

                if (val == 0) {
                    FaceUp(new object[] { 1, data.Key, new Vector2(0, 0) });
                }
            }
            //Down
            alteredKey = data.Key + new Vector3(0, -1, 0);
            if (blockData.TryGetValue(alteredKey, out val)) {
                if (val == 0) {
                    FaceDown(new object[] { 1, data.Key, new Vector2(0, 0) });
                }
            }
            //Forward
            alteredKey = data.Key + new Vector3(0, 0, 1);
            if (blockData.TryGetValue(alteredKey, out val)) {
                if (val == 0) {
                    FaceForward(new object[] { 1, data.Key, new Vector2(0, 0) });
                }
            }
            //Back
            alteredKey = data.Key + new Vector3(0, 0, -1);
            if (blockData.TryGetValue(alteredKey, out val)) {
                if (val == 0) {
                    FaceBack(new object[] { 1, data.Key, new Vector2(0, 0) });
                }
            }
            //Right
            alteredKey = data.Key + new Vector3(1, 0, 0);
            if (blockData.TryGetValue(alteredKey, out val)) {
                if (val == 0) {
                    FaceRight(new object[] { 1, data.Key, new Vector2(0, 0) });
                }
            }
            //Left
            alteredKey = data.Key + new Vector3(-1, 0, 0);
            if (blockData.TryGetValue(alteredKey, out val)) {
                if (val == 0) {
                    FaceLeft(new object[] { 1, data.Key, new Vector2(0, 0) });
                }
            }
        }
    }

    public void FaceUp(object state) {
        //Debug.Log("FaceUp called.");
        object[] stateAr = state as object[];
        //Debug.Log("stateArray size: " + stateAr.Length);
        float size = (float)Convert.ToDouble(stateAr[0]);
        Vector3 position = (Vector3)stateAr[1];
        Vector2 texture = (Vector2)stateAr[2];

        //Up
        lock (newVertices) {
            newVertices.Add(new Vector3(position.x + (size / 2), position.y + (size / 2), position.z + (size / 2)));
            newVertices.Add(new Vector3(position.x + (size / 2), position.y + (size / 2), position.z - (size / 2)));
            newVertices.Add(new Vector3(position.x - (size / 2), position.y + (size / 2), position.z - (size / 2)));
            newVertices.Add(new Vector3(position.x - (size / 2), position.y + (size / 2), position.z + (size / 2)));
        }

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

    public void FaceDown(object state) {
        object[] stateAr = state as object[];
        float size = (float)Convert.ToDouble(stateAr[0]);
        Vector3 position = (Vector3)stateAr[1];
        Vector2 texture = (Vector2)stateAr[2];
        //Down
        lock (newVertices) {
            newVertices.Add(new Vector3(position.x + (size / 2), position.y - (size / 2), position.z - (size / 2)));
            newVertices.Add(new Vector3(position.x + (size / 2), position.y - (size / 2), position.z + (size / 2)));
            newVertices.Add(new Vector3(position.x - (size / 2), position.y - (size / 2), position.z + (size / 2)));
            newVertices.Add(new Vector3(position.x - (size / 2), position.y - (size / 2), position.z - (size / 2)));
        }

        lock (newTriangles) {
            newTriangles.Add((squareCount * 4));
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 3);
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 2);
            newTriangles.Add((squareCount * 4) + 3);
        }
        squareCount++;

        //int ticks = Environment.TickCount;
        //while (Environment.TickCount - ticks < 2000) ;

    }
    public void FaceForward(object state) {
        object[] stateAr = state as object[];
        float size = (float)Convert.ToDouble(stateAr[0]);
        Vector3 position = (Vector3)stateAr[1];
        Vector2 texture = (Vector2)stateAr[2];
        //Forward
        lock (newVertices) {
            newVertices.Add(new Vector3(position.x + (size / 2), position.y - (size / 2), position.z + (size / 2)));
            newVertices.Add(new Vector3(position.x + (size / 2), position.y + (size / 2), position.z + (size / 2)));
            newVertices.Add(new Vector3(position.x - (size / 2), position.y + (size / 2), position.z + (size / 2)));
            newVertices.Add(new Vector3(position.x - (size / 2), position.y - (size / 2), position.z + (size / 2)));
        }

        lock (newTriangles) {
            newTriangles.Add((squareCount * 4));
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 3);
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 2);
            newTriangles.Add((squareCount * 4) + 3);
        }

        squareCount++;

        //int ticks = Environment.TickCount;
        //while (Environment.TickCount - ticks < 2000) ;
    }

    public void FaceBack(object state) {
        object[] stateAr = state as object[];
        float size = (float)Convert.ToDouble(stateAr[0]);
        Vector3 position = (Vector3)stateAr[1];
        Vector2 texture = (Vector2)stateAr[2];
        //Back
        lock (newVertices) {
            newVertices.Add(new Vector3(position.x + (size / 2), position.y + (size / 2), position.z - (size / 2)));
            newVertices.Add(new Vector3(position.x + (size / 2), position.y - (size / 2), position.z - (size / 2)));
            newVertices.Add(new Vector3(position.x - (size / 2), position.y - (size / 2), position.z - (size / 2)));
            newVertices.Add(new Vector3(position.x - (size / 2), position.y + (size / 2), position.z - (size / 2)));
        }

        lock (newTriangles) {
            newTriangles.Add((squareCount * 4));
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 3);
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 2);
            newTriangles.Add((squareCount * 4) + 3);
        }

        squareCount++;

        //int ticks = Environment.TickCount;
        //while (Environment.TickCount - ticks < 2000) ;
    }
    public void FaceRight(object state) {
        object[] stateAr = state as object[];
        float size = (float)Convert.ToDouble(stateAr[0]);
        Vector3 position = (Vector3)stateAr[1];
        Vector2 texture = (Vector2)stateAr[2];
        //Right
        lock (newVertices) {
            newVertices.Add(new Vector3(position.x + (size / 2), position.y + (size / 2), position.z - (size / 2)));
            newVertices.Add(new Vector3(position.x + (size / 2), position.y + (size / 2), position.z + (size / 2)));
            newVertices.Add(new Vector3(position.x + (size / 2), position.y - (size / 2), position.z + (size / 2)));
            newVertices.Add(new Vector3(position.x + (size / 2), position.y - (size / 2), position.z - (size / 2)));
        }

        lock (newTriangles) {
            newTriangles.Add((squareCount * 4));
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 3);
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 2);
            newTriangles.Add((squareCount * 4) + 3);
        }

        squareCount++;

        //int ticks = Environment.TickCount;
        //while (Environment.TickCount - ticks < 2000) ;
    }

    public void FaceLeft(object state) {
        object[] stateAr = state as object[];
        float size = (float)Convert.ToDouble(stateAr[0]);
        Vector3 position = (Vector3)stateAr[1];
        Vector2 texture = (Vector2)stateAr[2];
        //Left
        lock (newVertices) {
            newVertices.Add(new Vector3(position.x - (size / 2), position.y + (size / 2), position.z + (size / 2)));
            newVertices.Add(new Vector3(position.x - (size / 2), position.y + (size / 2), position.z - (size / 2)));
            newVertices.Add(new Vector3(position.x - (size / 2), position.y - (size / 2), position.z - (size / 2)));
            newVertices.Add(new Vector3(position.x - (size / 2), position.y - (size / 2), position.z + (size / 2)));
        }

        lock (newTriangles) {
            newTriangles.Add((squareCount * 4));
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 3);
            newTriangles.Add((squareCount * 4) + 1);
            newTriangles.Add((squareCount * 4) + 2);
            newTriangles.Add((squareCount * 4) + 3);
        }

        squareCount++;

        //int ticks = Environment.TickCount;
        //while (Environment.TickCount - ticks < 2000) ;
    }
}