using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointScript : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Traktor")
        {
            col.gameObject.GetComponent<VehicleScript>().setResetPoint(this.transform.position);
        }
    }
}
