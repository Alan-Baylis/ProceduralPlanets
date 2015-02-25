using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

    public GameObject currentChunk, lastChunk, pointingChunk, master;
    PlayerAtEdgeEvent playerAtEdge = new PlayerAtEdgeEvent();
    PlayerChangingChunkEvent playerChangingChunk = new PlayerChangingChunkEvent();
    FacingChunkEvent facingChunk = new FacingChunkEvent();
    List<Vector3> localChunkPositions = new List<Vector3>();
    public int chunkLength, chunkSize, loadDistance;
    public float fps=0;

    // Use this for initialization
    void Start() {
        chunkLength = master.GetComponent<MasterController>().chunkLength;
        chunkSize = master.GetComponent<MasterController>().chunkSize;

        currentChunk = master.GetComponent<MasterController>().firstChunk;
        playerAtEdge.PastTrigger += new PlayerAtEdge(PlayerAtEdge);
        playerChangingChunk.chunkChanged += new PlayerChangingChunk(PlayerChangingChunk);
        facingChunk.ChunkChanged += new FacingChunk(FacingChunkChanged);
        StartCoroutine(TryToGrowChunks());
        StartCoroutine(CheckFPS());
    }

    // Update is called once per frame
    void Update() {
        playerAtEdge.myValue = transform.position - currentChunk.transform.position;
        playerChangingChunk.myValue = transform.position - currentChunk.transform.position;
        facingChunk.myValue = transform.forward;
        fps = 1.0f / Time.deltaTime;
        //TryToGrowPriorityChunks();
    }

    IEnumerator ChangeLoadingDistance() {
        int chunkCount = Mathf.FloorToInt((loadDistance * 2) / chunkLength);
        Debug.Log("ChunkCount :" + chunkCount);
        localChunkPositions = UnityTools.pointsInsideEllipse(new Vector3(chunkCount, chunkCount, chunkCount), 1, new Vector3(chunkLength, chunkLength, chunkLength));
        yield return null;
    }

    IEnumerator CheckFPS() {
        while (true) {
            if (1.0 / Time.deltaTime > 60 && (1.0 / Time.deltaTime) < 90 && loadDistance < 32) {
                loadDistance += 2;
                yield return StartCoroutine(ChangeLoadingDistance());
            } else if (loadDistance > 8) { loadDistance -= 2; }
            yield return new WaitForSeconds(5.0f);
        }
    }

    void TryToAddChunk(Vector3 position) {
        if (!master.GetComponent<MasterController>().chunkData.ContainsKey(position)) {
            master.GetComponent<MasterController>().CreateChunk(position);
        }
    }

    void TryToActivateChunk(Vector3 position) {
        if (master.GetComponent<MasterController>().chunkData.ContainsKey(position)) {
            master.GetComponent<MasterController>().chunkData[position].setActive(true);
        }
    }

    void TryToDeactivateChunk(Vector3 position) {
        if (master.GetComponent<MasterController>().chunkData.ContainsKey(position)) {

            master.GetComponent<MasterController>().chunkData[position].setActive(false);
            //Debug.Log("TryToDeactivateChunk Passed.");
        } else { /*Debug.Log("TryToDeactiavteChunk Failed.");*/ }
    }

    //nulled.
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

    //nulled.
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

    //nulled.
    void CheckLoadedChunks(List<Vector3> localChunkPositions) {
        List<Vector3> editedChunkPositions = new List<Vector3>(master.GetComponent<MasterController>().chunkData.Keys);
        foreach (Vector3 vec3 in localChunkPositions) {
            if (editedChunkPositions.Contains(vec3)) {
                editedChunkPositions.Remove(vec3);
            }
        }

        foreach (Vector3 key in editedChunkPositions) {
            if (master.GetComponent<MasterController>().chunkData[key].active == true) {
                //Debug.Log("Trying To Deactivate Chunk.");
                TryToDeactivateChunk(key);
            }
        }

        master.GetComponent<MasterController>().DeactivateChunks();
    }

    void PlayerChangingChunk(object source, MyEventArgs e) {
        Vector3 testPosition = UnityTools.Ceiling(transform.position, chunkLength/2);
        //tryToAddChunk(testPosition);
        lastChunk = currentChunk;
        currentChunk = UnityTools.ClosestWithTag(this.gameObject, "Chunk");
        currentChunk.layer = 9;
        lastChunk.layer = 8;
        //Debug.Log(e.GetInfo());
    }

    void PlayerAtEdge(object source, MyEventArgs e) {
        //Debug.Log(e.GetInfo());
        Vector3 testPosition = UnityTools.Ceiling(transform.position + (transform.forward * ((chunkLength/2)-1)), chunkLength);
        //tryToAddChunk(testPosition);

    }

    void FacingChunkChanged(object source, MyEventArgs e) {
        //Debug.Log(e.GetInfo());

        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        RaycastHit hit;
        Ray ray = new Ray(transform.position, fwd);

        if (Physics.Raycast(ray, out hit, chunkLength/2, 8)) {
            pointingChunk = hit.transform.parent.gameObject;
        }
        Vector3 testPosition = UnityTools.Ceiling(transform.position + (transform.forward * ((chunkLength/2)-1)), chunkLength);
        //tryToAddChunk(testPosition);

    }
}