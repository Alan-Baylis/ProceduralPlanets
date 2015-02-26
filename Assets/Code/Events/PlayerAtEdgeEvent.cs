/* Purpose: This class is used for the PlayerAtEdgeEvent.
 * 
 * Special Notes: N/A.
 * 
 * Author: Devyn Cyphers; Devcon.
 */

using System;
using UnityEngine;
using System.Collections;

// Delegates to use.
public delegate void PlayerAtEdge(object source, MyEventArgs e);

public class PlayerAtEdgeEvent {
    
    // Variables to use.
    public event PlayerAtEdge PastTrigger;
    private Vector3 i;
    private bool atEdge = false;
    private float maxDist = 32;

    // Variable to test the event.
    public Vector3 myValue {
        get { return i; }
        set { 
            if ( value.x < maxDist && value.x > -maxDist && value.y < maxDist && value.y > -maxDist && value.z < maxDist && value.z > -maxDist) { 
                i = value;
                atEdge = false;

            } else if(PastTrigger != null && atEdge != true) {
                atEdge = true;
                PastTrigger(this, new MyEventArgs("Player is at edge zone of chunk."));
                
            } 
        }
    }
}