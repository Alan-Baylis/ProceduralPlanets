/* Purpose: This class is used for the PlayerChangingChunk.
 * 
 * Special Notes: N/A.
 * 
 * Author: Devyn Cyphers; Devcon.
 */

using System;
using UnityEngine;
using System.Collections;

// delegates to use.
public delegate void PlayerChangingChunk(object source, MyEventArgs e);

public class PlayerChangingChunkEvent {
    
    // Variables to use.
    public event PlayerChangingChunk chunkChanged;
    private Vector3 i;
    private float maxDist = 32;

    // variable to test the event.
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