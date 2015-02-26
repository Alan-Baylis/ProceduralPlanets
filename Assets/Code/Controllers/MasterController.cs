/* Purpose: This class is attatched to the master GameObject and is a general controller.
 * 
 * Special Notes: shared variables are stored here.
 * 
 * Author: Devyn Cyphers; Devcon.
 */
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class MasterController : MonoBehaviour {

    // Variables to use.
    public Transform chunk, block;
    public GameObject firstChunk;
    public Vector3[] posiblePosition;
    public int totalChunks, chunkSize, chunkLength;
    public Dictionary<Vector3, Structs.chunkData> chunkData = new Dictionary<Vector3, Structs.chunkData>();
    public Dictionary<string, Structs.blockTypes> blockDictionary = new Dictionary<string, Structs.blockTypes>();

    // Initialization.
    private void Start() {
        chunkSize = chunkLength * chunkLength * chunkLength;
        FindPossitions();
        foreach (GameObject gam in GameObject.FindGameObjectsWithTag("Chunk")) {
            if (!chunkData.ContainsKey(gam.transform.position)) { chunkData.Add(gam.transform.position, new Structs.chunkData(gam, true)); }
        }
    }

    // This method makes posiblePositions(Vector3[]) of all of the blocks within a chunk.
    private void FindPossitions() {
        posiblePosition = UnityTools.PointsInsideCube(new Vector3(chunkLength, chunkLength, chunkLength), 2, new Vector3(1, 1, 1)).ToArray();
    }

    // This method tries to deactivate chunks based off of chunkData(Dictionary<Vector3, Structs.chunkData>) :: UNDER CONSTRUCTION.
    public void DeactivateChunks() {
        foreach (Vector3 key in chunkData.Keys) {
            if (chunkData[key].active == false) {
                chunkData[key].chunk.SetActive(false);
            }
        }
    }

    // This method creates a chunk at the position(vector3) and adds it to the chunkData dictionary.
    public void CreateChunk(Vector3 position) {
        Transform newChunk = Instantiate(chunk, position, Quaternion.identity) as Transform;
        chunkData.Add(position, new Structs.chunkData(newChunk.gameObject, true));
        newChunk.name = "Chunk-" + totalChunks.ToString();
        totalChunks++;
    }
}