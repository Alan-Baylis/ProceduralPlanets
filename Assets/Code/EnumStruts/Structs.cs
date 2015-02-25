using System;
using UnityEngine;
using System.Collections;

public class Structs {

    public struct neighbor {
        public Enums.direction direction;
        public Vector3 position;
        public bool exists;

        public neighbor(Enums.direction dir1, Vector3 pos1, bool exi1) {
            direction = dir1;
            position = pos1;
            exists = exi1;
        }
    }

    public struct chunkData {
        public GameObject chunk;
        public bool active;


        public chunkData(GameObject chunk1, bool active1) {
            chunk = chunk1;
            active = active1;
        }

        public void setActive(bool value) {
            active = value;
        }
    }

    public struct blockTypes {
        public int type;

        public blockTypes(int type1) {
            type = type1;
        }
    }

}
