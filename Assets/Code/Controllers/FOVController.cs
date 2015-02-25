using UnityEngine;
using System.Collections;

public class FOVController : MonoBehaviour {

    private GameObject master;

	// Use this for initialization
	void Start () {
        master = UnityTools.GetMasterController();
	}

    private void CheckPositions() {

    }
}
