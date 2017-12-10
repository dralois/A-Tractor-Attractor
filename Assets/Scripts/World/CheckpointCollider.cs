using UnityEngine;

public class CheckpointCollider : MonoBehaviour {

    private int index;
    private VehicleAutoMove parent;
    
    void Start () {
        parent = gameObject.GetComponentInParent<VehicleAutoMove>();
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
	
    public void SetIndex(int idx)
    {
        index = idx;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "TraktorBoundingBox")
        {
            var vehicle = other.GetComponentInParent<VehicleScript>();
            if (vehicle.playerIndex == parent.PlayerIndex)
            {
                if (vehicle.Dead)
                    return;

                if (Vector3.Dot(this.transform.right, this.transform.position - other.transform.parent.position) > 0)
                {
                    parent.nextCheckpoint();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "TraktorBoundingBox")
        {
            var vehicle = other.GetComponentInParent<VehicleScript>();
            if (vehicle.playerIndex == parent.PlayerIndex)
            {
                if (Vector3.Dot(this.transform.right, this.transform.position - other.transform.parent.position) > 0)
                {
                    parent.prevCheckpoint();
                }
            }
        }
    }
}