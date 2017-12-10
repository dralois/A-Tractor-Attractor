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

    public void nextCheckpoint()
    {
        checkpointIndex = (checkpointIndex + 1) % Checkpoints.Length;
        Debug.Log(checkpointIndex+" inc");
    }

    public void prevCheckpoint()
    {
        checkpointIndex = checkpointIndex == 0 ? (Checkpoints.Length - 1) : (checkpointIndex - 1);

        Debug.Log(checkpointIndex + " dec");
    }
    

}
