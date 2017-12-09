using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleScript : MonoBehaviour {

    //public variables
    public float beamStrength = 1.0f;
    public float sidewaysDamping = 0.5f;
    public float enemyBeamScaling = 1.0f;
    public float maxAngularVelocity = 1.0f;
    public UfoController[] ufos;
    public Transform[] corners;

    public float maxHeight = 2.0f;

    public int playerIndex;
    public Screen screen;

    private Vector3 resetPoint;

    private Vector3 previousVelocity;
    
    //private variables
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = this.gameObject.GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxAngularVelocity;

        resetPoint = rb.position;
        previousVelocity = rb.velocity;
    }

    void Update()
    {
        //Rotiert das objekt in richtung der aktuellen geschwindigkeit 
        if (rb.velocity.magnitude > 0.5f) 
        {
            float rotationSpeed = 2 * Time.deltaTime * rb.velocity.magnitude;
            if (rotationSpeed > 200 * Time.deltaTime)
                rotationSpeed = 200 * Time.deltaTime;

            rb.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(rb.velocity), rotationSpeed);
        }
        clampRotation();
    }
	
    void clampRotation()
    {
        Vector3 euler = this.transform.localEulerAngles;
        euler.z = Mathf.Clamp(euler.z, -20, 20);
        euler.x = Mathf.Clamp(euler.x, -10, 10);
        this.transform.localEulerAngles = euler;
    }

	// Update is called once per frame
	void FixedUpdate () {
        //calculate forces on corners and apply it to the rigidbody
        for(int i = 0; i < 4; i++)
        {
            Vector3 force = new Vector3(0,0,0);
            bool applyForce = false;
            foreach(UfoController ufo in ufos)
            {
                if (ufo.getCurrentScreen() != screen)
                    continue;

                if (!ufo.isPulling())
                    continue;

                //Debug.Log("inside");
                applyForce = true;

                Vector3 ufoAdaptedHeight = new Vector3(ufo.getWorldPosition().x, corners[i].position.y, ufo.getWorldPosition().z);
                Vector3 cornerToUfo = ufoAdaptedHeight - corners[i].position;
                Vector3 ufoForce;

                ufoForce = cornerToUfo.normalized * beamStrength;
                Debug.Log(cornerToUfo.normalized);

                force += ufoForce;
            }

            //apply force on rigidbody
            if(applyForce)
                rb.AddForceAtPosition(force, corners[i].position);
        }

        //damp sideways velocity
        var locVel = transform.InverseTransformDirection(rb.velocity);
        locVel.x *= sidewaysDamping;
        rb.velocity = transform.TransformDirection(locVel);

        //clamp y coordinate
        if (rb.position.y > maxHeight)
        {
            rb.transform.Translate(new Vector3(0, maxHeight - rb.position.y, 0));
            if(rb.velocity.y  > 0)
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
    }

    public void setResetPoint(Vector3 pos)
    {
        resetPoint = pos;
    }

    public void onDeath()
    {
        rb.position = resetPoint;
    }
}
