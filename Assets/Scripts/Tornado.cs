using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour {

    public float speed;
	
	// Update is called once per frame
	void Update () {
        this.transform.rotation *= Quaternion.AngleAxis(speed * Time.deltaTime, Vector3.forward);
	}
}
