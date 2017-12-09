﻿using UnityEngine;

public class CameraFollow : MonoBehaviour {

    //-----------------------------------------------
    //Kamera Target
    //-----------------------------------------------
    [Header("Camera Target")]
    [SerializeField]
    private Transform m_Target;
    //-----------------------------------------------
    //Sonstiges
    //-----------------------------------------------
    private Rigidbody m_TargetRB;
    private Camera m_Camera;
    //-----------------------------------------------
    //Einstellungen
    //-----------------------------------------------
    [Header("Movement-Settings")]
    [SerializeField]
    private float m_TranslateSmoothSpeed;
    [SerializeField]
    private float m_Offset;
    [Header("FOV-Settings")]
    [SerializeField]
    private float m_FOVSmoothSpeed;
    [SerializeField]
    private float m_MaxSpeed;
    [SerializeField]
    private float m_MinFOV;
    [SerializeField]
    private float m_MaxFOV;

    private void Start()
    {
        //-----------------------------------------------
        //Hole Components
        //-----------------------------------------------
        m_Camera = gameObject.GetComponent<Camera>();
        m_TargetRB = m_Target.GetComponent<Rigidbody>();
        //-----------------------------------------------
        //Debug-Vorsorge
        //-----------------------------------------------
        if (m_TargetRB == null) Debug.LogError("Target has no Ridigbody");
    }

    //-----------------------------------------------
    //Bewegt die Kamera jedes Fixed Update
    //-----------------------------------------------
    private void FixedUpdate()
    {
        //-----------------------------------------------
        //Bestimme Wert zw. 0-1 basierend auf Velocity
        //-----------------------------------------------
        float l_LerpVal = Mathf.Min(m_TargetRB.velocity.magnitude / m_MaxSpeed, 1);
        //-----------------------------------------------
        //Passe Kamera FOV an
        //-----------------------------------------------
        m_Camera.fieldOfView= Mathf.Lerp(m_Camera.fieldOfView, Mathf.Lerp(m_MinFOV, m_MaxFOV, l_LerpVal), 
                                         Time.deltaTime * m_FOVSmoothSpeed);
        //-----------------------------------------------
        //Interpolation Wert
        //-----------------------------------------------
        float interpolation = m_TranslateSmoothSpeed * Time.deltaTime;
        //-----------------------------------------------
        //Update Position
        //-----------------------------------------------
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, m_Target.position.x + Mathf.Sign(m_TargetRB.velocity.x) * m_Offset, interpolation),
                                         transform.position.y,
                                         Mathf.Lerp(transform.position.z, m_Target.position.z + Mathf.Sign(m_TargetRB.velocity.z) * m_Offset, interpolation));
    }
}