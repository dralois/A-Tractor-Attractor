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

    public int playerIndex;
    public Screen screen;

    //private variables
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = this.gameObject.GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxAngularVelocity;
    }

    void Update()
    {
        rb.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(rb.velocity), 200*Time.deltaTime);
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
              
                Vector3 ufoAdaptedHeight = new Vector3(ufo.getWorldPosition().x, corners[i].position.y, ufo.getWorldPosition().z);
                Vector3 cornerToUfo = ufoAdaptedHeight - corners[i].position;
                Vector3 ufoForce = cornerToUfo.normalized * ((beamStrength / (cornerToUfo.magnitude + 3)) * 3 + 1);
                if (ufo.getPlayerIndex() != playerIndex)
                    ufoForce *= enemyBeamScaling;

                force += ufoForce;
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
