/* Purpose: This class is attatched to each chunk and is a general controller.
 * Notable features:
 *      Finds the neighboring chunks.
 *      Creates the terrain that is inside this chunk, passes it to terrainController.
 * 
 * Special Notes: N/A.
 * 
 * Author: Devyn Cyphers; Devcon.
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using IO_Tools;
using DevconTools;

public class ChunkController : MonoBehaviour {

    // Variables to use.
    public List<Structs.neighbor> neighbors = new List<Structs.neighbor>();
    public Dictionary<Vector3, int> blockData = new Dictionary<Vector3, int>();
    public GameObject master, bounds, chunkMesh;
    
    private Vector3[] posiblePosition;

    public bool preLoad=false;
    public int totalCreated;

    private int chunkLength;

    // Initialization.
    void Start() {

        // Gets variables from master.
        master = GameObject.FindGameObjectWithTag("GameController");
        chunkLength = master.GetComponent<MasterController>().chunkLength;
        posiblePosition = master.GetComponent<MasterController>().posiblePosition;

        // Sets up the neighbors List<Structs.neighbor>.
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

        // Call methods.
        checkNeighbors();

        // Start coroutines.
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

    // This coroutine Creates all of the blocks in the chunk.
    IEnumerator genTerrain() {
        int count = 0, frameCount = 0;
        for (int i = 0; i < posiblePosition.Length; i++) {

            int blockType = 0;
            Vector3 thisPosition = posiblePosition[i];
            thisPosition = transform.position+posiblePosition[i];
            float noisePos = thisPosition.x * thisPosition.x + thisPosition.y * thisPosition.y + thisPosition.z * thisPosition.z;

            if (thisPosition.y <= Mathf.RoundToInt((pnng.smoothNoise(noisePos,3,1,2)))) { blockType = 1; }

            thisPosition = posiblePosition[i];

            if (!blockData.ContainsKey(thisPosition)) {
                if (transform.TransformPoint(thisPosition).y <= 10 && transform.TransformPoint(thisPosition).y >=-4) {
                    blockData.Add(thisPosition, blockType);
                    count++;
                }
            }

            //Division of Labor
            if (frameCount >= (1.0/Time.deltaTime)/2 && preLoad == false) {
                frameCount = 0;

                yield return null;
            } else { frameCount++; }
        }
        chunkMesh.GetComponent<TerrainController>().PreGen(blockData, bounds);
    }
    
    // This method Adds this chunks data to the xml save file :: UNDER CONSTRUCTION.
    void addToSave() {
        IO_Tools.Save.chunk(transform.position.x, transform.position.y, transform.position.z, gameObject.name);
        Debug.Log("saved chunk.");
    }
}