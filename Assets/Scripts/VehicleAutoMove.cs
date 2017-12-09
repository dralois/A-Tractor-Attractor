using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAutoMove : MonoBehaviour {

    public GameObject[] Checkpoints;
    public int PlayerIndex;

    private int checkpointIndex;

	// Use this for initialization
	void Awake () {
        checkpointIndex = 0;
        for (int i = 0; i < Checkpoints.Length; i++)
        {
            var collider = Checkpoints[i].GetComponentInChildren<CheckpointCollider>();
            collider.SetIndex(i);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Transform GetCurrentCheckpoint()
    {
        return Checkpoints[checkpointIndex].transform;
    }

    public void OnTrigger(Collider other, int idx)
    {
        if(other.tag == "Traktor")
        {
            var vehicle = other.GetComponent<VehicleScript>();
            if(vehicle.playerIndex == PlayerIndex)
            {
                if(idx == checkpointIndex - 1) // letzter checkpoint 
                {
                    checkpointIndex --;
                }
                else if(idx >= checkpointIndex && checkpointIndex < Checkpoints.Length)
                {
                    checkpointIndex++;
                }
                if(checkpointIndex == Checkpoints.Length)
                {
                    // TODO Finished!
                    checkpointIndex = 0;
                }
            }
        }
    }

}
