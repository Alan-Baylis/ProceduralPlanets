using System;
using UnityEngine;
using System.Collections;

public delegate void PlayerChangingChunk(object source, MyEventArgs e);

public class PlayerChangingChunkEvent {
    public event PlayerChangingChunk chunkChanged;
    private Vector3 i;
    private float maxDist = 32;
    public Vector3 myValue {
        get { return i; }
        set {
            if (value.x < maxDist && value.x > -maxDist && value.y < maxDist && value.y > -maxDist && value.z < maxDist && value.z > -maxDist) {
                i = value;

            } else if (chunkChanged != null) {
                chunkChanged(this, new MyEventArgs("Player has changed chunks."));
            }
        }
    }
}