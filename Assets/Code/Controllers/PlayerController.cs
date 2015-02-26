/*purpose: This class is attatched to the player and is a general controller.
 * Notable features: 
 *      Controls the loading distance and placement of chunks.
 *      Deactivates far away chunks.
 *      Several player/chunk events.
 *
 * Special Notes: N/A.
 *
 * Author: Devyn Cyphers; Devcon.
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

    // Variable to use.
    public GameObject currentChunk, lastChunk, pointingChunk, master;
    private PlayerAtEdgeEvent playerAtEdge = new PlayerAtEdgeEvent();
    private PlayerChangingChunkEvent playerChangingChunk = new PlayerChangingChunkEvent();
    private FacingChunkEvent facingChunk = new FacingChunkEvent();
    private List<Vector3> localChunkPositions = new List<Vector3>();
    public int chunkLength, chunkSize, loadDistance;

    // Initialization.
    void Start() {
        
        // Get data from master.
        chunkLength = master.GetComponent<MasterController>().chunkLength;
        chunkSize = master.GetComponent<MasterController>().chunkSize;
        currentChunk = master.GetComponent<MasterController>().firstChunk;
        
        // Event delegates.
        playerAtEdge.PastTrigger += new PlayerAtEdge(PlayerAtEdge);
        playerChangingChunk.chunkChanged += new PlayerChangingChunk(PlayerChangingChunk);
        facingChunk.ChunkChanged += new FacingChunk(FacingChunkChanged);
        
        // Start Coroutines.
        StartCoroutine(ChangeLoadingDistance());
        StartCoroutine(TryToGrowChunks());
        StartCoroutine(CheckFPS());
    }

    // Update is called once per frame
    void Update() {

        // Event data.
        playerAtEdge.myValue = transform.position - currentChunk.transform.position;
        playerChangingChunk.myValue = transform.position - currentChunk.transform.position;
        facingChunk.myValue = transform.forward;
        
    }

    // A Coroutine that refreshes the localChunkPositions.
    IEnumerator ChangeLoadingDistance() {
        localChunkPositions = UnityTools.PointsInsideEllipse(new Vector3(loadDistance, loadDistance, 1), new Vector3(1, 1, 1), new Vector3(chunkLength, chunkLength, chunkLength));
        yield return null;

    }

    // A Coroutine that expands the loadingDistance based on the FPS.
    IEnumerator CheckFPS() {

        while (true) {
            if (1.0 / Time.deltaTime > 60 && (1.0 / Time.deltaTime) < 90 && loadDistance < 32) {
                loadDistance += 1;
                yield return StartCoroutine(ChangeLoadingDistance());

            } else if (loadDistance > 8) { loadDistance -= 1; }
            yield return new WaitForSeconds(5.0f);
        
        }
    }

    // A method that tries to add a chunk at the position(Vector3).
    void TryToAddChunk(Vector3 position) {
        if (!master.GetComponent<MasterController>().chunkData.ContainsKey(position)) {
            master.GetComponent<MasterController>().CreateChunk(position);
        }
    }

    // A meathod that tries to set the chunk at the position(Vector3) to be active.
    void TryToActivateChunk(Vector3 position) {
        if (master.GetComponent<MasterController>().chunkData.ContainsKey(position)) {
            master.GetComponent<MasterController>().chunkData[position].setActive(true);
        }
    }

    // A meathod that tries to set the chunk at the position(Vector3) to be Deactive.
    void TryToDeactivateChunk(Vector3 position) {
        if (master.GetComponent<MasterController>().chunkData.ContainsKey(position)) {
            master.GetComponent<MasterController>().chunkData[position].setActive(false);
        }
    }

    // OBSOLETE: Tried to create the neighboring chunks. 
    void TryToGrowPriorityChunks() {
        List<Structs.neighbor> neighbors = currentChunk.GetComponent<ChunkController>().neighbors;
        
        if (!neighbors.TrueForAll(x => x.exists == true)) {
            foreach (Structs.neighbor nei in neighbors) {
                if (nei.exists == false) {
                    TryToAddChunk(neighbors.Find(x => x.Equals(nei)).position);
                }
            }
        }
    }

    // A coroutine that tries to grow the amount of active chunks. 
    IEnumerator TryToGrowChunks() {
        while (true) {
            List<Vector3> chunkCheckPositions = new List<Vector3>();
            foreach (Vector3 vec3 in localChunkPositions) {
                Vector3 key = vec3 + transform.position;
                key = UnityTools.Ceiling(key, chunkLength);
                if (!master.GetComponent<MasterController>().chunkData.ContainsKey(key)) { chunkCheckPositions.Add(key); }
            }

            chunkCheckPositions.ForEach(x => TryToAddChunk(x));
            CheckLoadedChunks(chunkCheckPositions);
            yield return new WaitForSeconds(2f);
        }
    }

    // A method that takes the localChunkPosition(List<Vector3>) and keep only the surrounding chunks active.
    void CheckLoadedChunks(List<Vector3> localChunkPositions) {
        List<Vector3> editedChunkPositions = new List<Vector3>(master.GetComponent<MasterController>().chunkData.Keys);
        foreach (Vector3 vec3 in localChunkPositions) {
            if (editedChunkPositions.Contains(vec3)) {
                editedChunkPositions.Remove(vec3);
            }
        }

        foreach (Vector3 key in editedChunkPositions) {
            if (master.GetComponent<MasterController>().chunkData[key].active == true) {
                TryToDeactivateChunk(key);
            }
        }

        master.GetComponent<MasterController>().DeactivateChunks();
    }

    // PlayerChangingChunk Event method.
    void PlayerChangingChunk(object source, MyEventArgs e) {
        Vector3 testPosition = UnityTools.Ceiling(transform.position, chunkLength/2);
        lastChunk = currentChunk;
        currentChunk = UnityTools.ClosestWithTag(this.gameObject, "Chunk");
        currentChunk.layer = 9;
        lastChunk.layer = 8;
    }

    // PlayerAtEdge Event method.
    void PlayerAtEdge(object source, MyEventArgs e) {
        Vector3 testPosition = UnityTools.Ceiling(transform.position + (transform.forward * ((chunkLength/2)-1)), chunkLength);
    }

    // FacingChunkChanged Event method.
    void FacingChunkChanged(object source, MyEventArgs e) {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        RaycastHit hit;
        Ray ray = new Ray(transform.position, fwd);

        if (Physics.Raycast(ray, out hit, chunkLength/2, 8)) {
            pointingChunk = hit.transform.parent.gameObject;
        }
        Vector3 testPosition = UnityTools.Ceiling(transform.position + (transform.forward * ((chunkLength/2)-1)), chunkLength);
    }
}