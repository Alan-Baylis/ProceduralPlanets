using System;
using UnityEngine;
using System.Collections;

public delegate void PlayerAtEdge(object source, MyEventArgs e);

public class PlayerAtEdgeEvent {
    public event PlayerAtEdge PastTrigger;
    private Vector3 i;
    private bool atEdge = false;
    private float maxDist = 32;
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