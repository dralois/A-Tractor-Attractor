using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleScript : MonoBehaviour {

    //public variables
    public float beamStrength = 1.0f;
    public float sidewaysDamping = 0.5f;
    public float enemyBeamScaling = 1.0f;
    public float maxAngularVelocity = 1.0f;
    public Cursor[] ufos;
    public Transform[] corners;

    public Screen screen;

    //private variables
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = this.gameObject.GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxAngularVelocity;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //calculate forces on corners and apply it to the rigidbody
        for(int i = 0; i < 4; i++)
        {
            Vector3 force = new Vector3();
            foreach(Cursor ufo in ufos)
            {
                if (ufo.getCurrentScreen() != screen)
                    continue;

                Debug.Log(ufo.getWorldPosition());
                Vector3 ufoAdaptedHeight = new Vector3(ufo.getWorldPosition().x, corners[i].position.y, ufo.getWorldPosition().z);
                Vector3 cornerToUfo = ufoAdaptedHeight - corners[i].position;
                force += cornerToUfo.normalized * beamStrength;//(beamStrength / cornerToUfo.magnitude);
            }
            //apply force on rigidbody

            rb.AddForceAtPosition(force, corners[i].position);
        }

        //damp sideways velocity
        var locVel = transform.InverseTransformDirection(rb.velocity);
        locVel.x *= sidewaysDamping;
        rb.velocity = transform.TransformDirection(locVel);
    }
}
