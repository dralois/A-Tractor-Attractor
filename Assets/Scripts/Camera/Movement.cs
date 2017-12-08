using UnityEngine;

public class Movement : MonoBehaviour {

    //-----------------------------------------------
    //Rigidbody des Objekts und Geschwindigkeit
    //-----------------------------------------------
    private Rigidbody m_Rigidbody;
    public float m_Speed;

    private void Start()
    {
        //-----------------------------------------------
        //Hole Rigidbody
        //-----------------------------------------------
        m_Rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    //-----------------------------------------------
    //Bewegt den Würfel
    //-----------------------------------------------
    private void Update()
    {
        //-----------------------------------------------
        //Hole Horizontal/Vertikal in int
        //-----------------------------------------------
        float l_Horizontal = (Input.GetAxisRaw("Horizontal") == 0 ? 0 : Input.GetAxisRaw("Horizontal") > 0 ? 1 : -1) * m_Speed;
        float l_Vertical = (Input.GetAxisRaw("Vertical") == 0 ? 0 : Input.GetAxisRaw("Vertical") > 0 ? 1 : -1) * m_Speed;
        //-----------------------------------------------
        //Falls Bewegung notwendig
        //-----------------------------------------------
        if (Mathf.Abs(l_Horizontal) > 0 || Mathf.Abs(l_Vertical) > 0)
        {
            //-----------------------------------------------
            //Wende Kraft an
            //-----------------------------------------------
            m_Rigidbody.AddForce(l_Horizontal * Time.deltaTime * m_Speed,
                                                          0, l_Vertical * Time.deltaTime * m_Speed,
                                                          ForceMode.VelocityChange);
        }
        else
        {
            //-----------------------------------------------
            //Oder bremse
            //-----------------------------------------------
            m_Rigidbody.AddForce(m_Rigidbody.velocity.x * -Time.deltaTime,
                                 0, m_Rigidbody.velocity.z * -Time.deltaTime,
                                 ForceMode.VelocityChange);
        }
    }
}