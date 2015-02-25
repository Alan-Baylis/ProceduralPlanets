using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class MasterController : MonoBehaviour {

    // Basic Objects
    public Transform chunk, block;
    public GameObject firstChunk;
    public Vector3[] posiblePosition;
    public int totalChunks, chunkSize, chunkLength;
    public Dictionary<Vector3, Structs.chunkData> chunkData = new Dictionary<Vector3, Structs.chunkData>();
    public Dictionary<string, Structs.blockTypes> blockDictionary = new Dictionary<string, Structs.blockTypes>();


    private void Start() {
        chunkSize = chunkLength * chunkLength * chunkLength;
        FindPossitions();
        foreach (GameObject gam in GameObject.FindGameObjectsWithTag("Chunk")) {
            if (!chunkData.ContainsKey(gam.transform.position)) { chunkData.Add(gam.transform.position, new Structs.chunkData(gam, true)); }
        }
    }

    private void FindPossitions() {
        posiblePosition = UnityTools.PointsInsideVolume(new Vector3(chunkLength, chunkLength, chunkLength), 2, new Vector3(1, 1, 1)).ToArray();
    }

    public void DeactivateChunks() {
        foreach (Vector3 key in chunkData.Keys) {
            if (chunkData[key].active == false) {
                chunkData[key].chunk.SetActive(false);
                //Debug.Log("Set chunk active: false.");
            } else { /*Debug.Log("Set chunk active: true.");*/ }
        }
    }

    public void CreateChunk(Vector3 position) {
        Transform newChunk = Instantiate(chunk, position, Quaternion.identity) as Transform;
        chunkData.Add(position, new Structs.chunkData(newChunk.gameObject, true));
        newChunk.name = "Chunk-" + totalChunks.ToString();
        totalChunks++;
    }
}