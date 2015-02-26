/* Purpose: This class is used for the FacingChunkEvent.
 * 
 * Special Notes: N/A.
 * 
 * Author: Devyn Cyphers; Devcon.
 */

using UnityEngine;
using System.Collections;

// Delegates to use.
public delegate void FacingChunk(object source, MyEventArgs e);

public class FacingChunkEvent {

    // variables to use.
    public event FacingChunk ChunkChanged;
    private Vector3 facingChunkAngle, lastChunkAngle;
    private bool sameChunk = true;

    // Variable to test the event.
    public Vector3 myValue {
        get { return facingChunkAngle; }
        set {
            facingChunkAngle = UnityTools.ClosestAngle(value, 
                                new Vector3[6]{Vector3.up,Vector3.down, Vector3.forward,
                                    Vector3.back, Vector3.left, Vector3.right});
            if (lastChunkAngle == facingChunkAngle) {
                sameChunk = true;
            } else if (myValue != null && sameChunk == true) {
                sameChunk = false;
                lastChunkAngle = facingChunkAngle;
                ChunkChanged(this, new MyEventArgs("The Player is facing a different chunk."));
            }

        }
    }
}
