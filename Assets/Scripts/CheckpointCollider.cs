using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointCollider : MonoBehaviour {

    private int index;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetIndex(int idx)
    {
        index = idx;
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.GetComponentInParent<VehicleAutoMove>().OnTrigger(other, index);
    }
}
