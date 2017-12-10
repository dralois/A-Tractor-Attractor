using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleScript : MonoBehaviour
{

    //public variables
    public float beamStrength = 1.0f;
    public float sidewaysDamping = 0.5f;
    public float enemyBeamScaling = 1.0f;
    public float maxAngularVelocity = 1.0f;
    public float respawnTime = 2.0f;
    public Material[] mats;
    public UfoController[] ufos;
    public Transform[] corners;
    public float autoSpeed;

    public float maxHeight = 2.0f;

    public int playerIndex;
    public Screen screen;

    private Vector3 resetPoint;

    private Vector3 previousVelocity;

    bool inMud;

    //private variables
    private Rigidbody rb;
    public bool Dead { get; private set; }

    public VehicleAutoMove vehicleAutoMove;

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxAngularVelocity;

        resetPoint = rb.position;

        inMud = false;
    }    
	
    void clampRotation()
    {
        Vector3 euler = this.rb.rotation.eulerAngles;
        euler.z = 0;
        euler.x = 0;
        rb.rotation = Quaternion.Euler(euler);
    }
    
    void FixedUpdate()
    {
        if (CountDown.CountdownActive || Dead)
            { return; }

        bool ufoIsPulling = false;

        //calculate forces on corners and apply it to the rigidbody
        for (int i = 0; i < 4; i++)
        {
            Vector3 force = new Vector3(0, 0, 0);
            bool applyForce = false;
            foreach (UfoController ufo in ufos)
            {
                if (ufo.getCurrentScreen() != screen)
                    continue;

                if (!ufo.isPulling())
                    continue;

                ufoIsPulling = true;

                //Debug.Log("inside");
                applyForce = true;

                Vector3 ufoAdaptedHeight = new Vector3(ufo.getWorldPosition().x, corners[i].position.y, ufo.getWorldPosition().z);
                Vector3 cornerToUfo = ufoAdaptedHeight - corners[i].position;
                Vector3 ufoForce;

                //cornerToUfo.Normalize();
                ufoForce = cornerToUfo.normalized * beamStrength;
                //Debug.Log(cornerToUfo);

                force += ufoForce;
            }

            //apply force on rigidbody
            if (applyForce)
                rb.AddForceAtPosition(force, corners[i].position, ForceMode.Force);
        }

        var target = vehicleAutoMove.GetCurrentCheckpoint();

        if (ufoIsPulling == false && target != null && rb.velocity.magnitude < autoSpeed * 1.5f)
        {
            if(!inMud)
                rb.MovePosition(rb.position + (new Vector3(target.position.x, rb.position.y, target.position.z) - rb.position).normalized * Time.fixedDeltaTime * autoSpeed);
            else
                rb.MovePosition(rb.position + (new Vector3(target.position.x, rb.position.y, target.position.z) - rb.position).normalized * Time.fixedDeltaTime * autoSpeed * 0.5f);
        }

        //damp sideways velocity
        var locVel = transform.InverseTransformDirection(rb.velocity);
        locVel.x *= sidewaysDamping;
        rb.velocity = transform.TransformDirection(locVel);

        //clamp y coordinate
        if (rb.position.y > maxHeight)
        {
            rb.transform.Translate(new Vector3(0, maxHeight - rb.position.y, 0));
            if (rb.velocity.y > 0)
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }


                //Rotiert das objekt in richtung der aktuellen geschwindigkeit 
        clampRotation();
        if (ufoIsPulling == false)
        {
            if (target != null && rb.velocity.magnitude < autoSpeed * 1.2f)
            {
                float rotationSpeed = Time.deltaTime * 100;
                rb.rotation = Quaternion.RotateTowards(rb.rotation, Quaternion.LookRotation((new Vector3(target.position.x, rb.position.y, target.position.z) - rb.position).normalized, Vector3.up), rotationSpeed);
            }
        }

        else if (rb.velocity.magnitude > 0.5f)
        {
            float rotationSpeed = 2 * Time.deltaTime * rb.velocity.magnitude;
            if (rotationSpeed > 200 * Time.deltaTime)
                rotationSpeed = 200 * Time.deltaTime;
            rb.rotation = Quaternion.RotateTowards(rb.rotation, Quaternion.LookRotation(rb.velocity, Vector3.up), rotationSpeed);
        }

    }

    public void setResetPoint(Vector3 pos)
    {
        resetPoint = pos;
    }

    public void onDeath()
    {
        rb.position = resetPoint;
        rb.velocity = Vector3.zero;
        Dead = true;
        StartCoroutine(respawnTimer());
    }

    private IEnumerator respawnTimer()
    {
        List<float> metallInformatin = new List<float>();
        foreach (Material m in mats)
        {
            metallInformatin.Add(m.GetFloat("_Metallic"));
            m.color = new Color(m.color.r, m.color.g, m.color.b, 0.5f);
            m.SetFloat("_Metallic", 0);
        }

        yield return new WaitForSeconds(respawnTime);
        Dead = false;

        int i = 0;
        foreach (Material m in mats)
        {
            m.color = new Color(m.color.r, m.color.g, m.color.b, 1.0f);
            m.SetFloat("_Metallic", metallInformatin[i]);
            i++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {        
        if(other.gameObject.tag == "Mud")
        {
            rb.mass = 3;
            rb.velocity = rb.velocity.normalized * 10.0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Mud")
        {
            rb.mass = 1;
            maxAngularVelocity = 1;
        }
    }
}