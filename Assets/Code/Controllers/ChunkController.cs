using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using IO_Tools;
using DevconTools;

public class ChunkController : MonoBehaviour {
    public List<Structs.neighbor> neighbors = new List<Structs.neighbor>();
    public Dictionary<Vector3, int> blockData = new Dictionary<Vector3, int>();
    private Vector3[] posiblePosition;
    private int chunkLength;
    public bool preLoad=false;
    public GameObject master, bounds, chunkMesh;
    public int totalCreated;

    // Use this for initialization
    void Start() {
        master = GameObject.FindGameObjectWithTag("GameController");
        chunkLength = master.GetComponent<MasterController>().chunkLength;
        posiblePosition = master.GetComponent<MasterController>().posiblePosition;

        neighbors.Add(new Structs.neighbor(Enums.direction.right,
                                transform.position + new Vector3(chunkLength, 0, 0),
                                false));
        neighbors.Add(new Structs.neighbor(Enums.direction.left,
                                transform.position + new Vector3(-chunkLength, 0, 0),
                                false));
        neighbors.Add(new Structs.neighbor(Enums.direction.up,
                                transform.position + new Vector3(0, chunkLength, 0),
                                false));
        neighbors.Add(new Structs.neighbor(Enums.direction.down,
                                transform.position + new Vector3(0, -chunkLength, 0),
                                false));
        neighbors.Add(new Structs.neighbor(Enums.direction.forward,
                                transform.position + new Vector3(0, 0, chunkLength),
                                false));
        neighbors.Add(new Structs.neighbor(Enums.direction.back,
                                transform.position + new Vector3(0, 0, -chunkLength),
                                false));

        checkNeighbors();
        StartCoroutine(genTerrain());
    }

    // Looks to see state of other chunks.
    void checkNeighbors() {

        for (int i = 0; i < neighbors.Count; i++) {
            if (master.GetComponent<MasterController>().chunkData.ContainsKey(neighbors.ToArray()[i].position)) {
                neighbors.ToArray()[i].exists = true;
            }
        }
    }

    //Creates all of the blocks in the chunk.
    IEnumerator genTerrain() {
        int count = 0, frameCount = 0;
        for (int i = 0; i < posiblePosition.Length; i++) {

            //int blockType = Mathf.RoundToInt(pnng.smoothNoise(i, 2, 1, 1));
            int blockType = 0;
            if (posiblePosition[i].y <= Mathf.RoundToInt((pnng.smoothNoise(i,1,1,6)))) { blockType = 1; }
            //Debug.Log(blockType);
            Vector3 thisPosition = posiblePosition[i];

            if (!blockData.ContainsKey(thisPosition)) {
                if (transform.TransformPoint(thisPosition).y <= 10 && transform.TransformPoint(thisPosition).y >=-4) {
                    blockData.Add(thisPosition, blockType);
                    count++;
                }
            }

            //Division of Labor
            if (frameCount >= (1.0/Time.deltaTime)/2 && preLoad == false) {
                frameCount = 0;
                //Debug.Log("Yielding frame.");
                yield return null;
            } else { frameCount++; }
        }
        //chunkMesh.GetComponent<TerrainController>().PreGen(blockData, bounds);
    }
    
    // Adds this chunks data to the xml save file.
    void addToSave() {
        IO_Tools.Save.chunk(transform.position.x, transform.position.y, transform.position.z, gameObject.name);
        Debug.Log("saved chunk.");
    }
}