using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ParticleTrigger : MonoBehaviour
{
    public ParticleSystem ps;
    public float G = 9.8f;
    [System.Obsolete]
    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
            
        }
        Debug.Log("contact:   " + collision.collider.name + "  Force:   " + collision.impactForceSum.magnitude + "   impulse:   " + collision.impulse.magnitude);

    }
    void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(-transform.up * G, ForceMode.Acceleration);
    }
}