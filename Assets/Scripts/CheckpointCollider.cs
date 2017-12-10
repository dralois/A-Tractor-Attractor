using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointCollider : MonoBehaviour {

    private int index;
    private VehicleAutoMove parent;

    // Use this for initialization
    void Start () {
        parent = gameObject.GetComponentInParent<VehicleAutoMove>();

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

        if (other.tag == "Traktor")
        {
            var vehicle = other.GetComponent<VehicleScript>();
            if (vehicle.playerIndex == parent.PlayerIndex)
            {
                if (Vector3.Dot(this.transform.right, this.transform.position - other.transform.position) > 0)
                {
                    parent.nextCheckpoint();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Traktor")
        {
            var vehicle = other.GetComponent<VehicleScript>();
            if (vehicle.playerIndex == parent.PlayerIndex)
            {
                if (Vector3.Dot(this.transform.right, this.transform.position - other.transform.position) > 0)
                {
                    parent.prevCheckpoint();
                }
            }
        }
    }
}
