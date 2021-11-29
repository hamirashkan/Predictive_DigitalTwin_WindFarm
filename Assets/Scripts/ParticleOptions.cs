using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOptions : MonoBehaviour
{
    private ParticleCollisionEvent[] CollisionEvents;
    public ParticleSystem ps;
    List<ParticleCollisionEvent> collisionEvents;
    void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    //[System.Obsolete]
    //void OnParticleCollision(GameObject other)
    //{

    //    ParticlePhysicsExtensions.GetCollisionEvents(ps, other, collisionEvents);
    //    int safeLength = ps.safeCollisionEventSize;
    //    var percent = ((float)(safeLength) / (float)(ps.particleCount) * 100.0f);
 
    //    if (collisionEvents[0].colliderComponent.name == "Capsule")
    //    {
    //        Debug.Log("Velocity" + collisionEvents[100].velocity.magnitude);
    //    }
    //    Debug.Log("name: " + collisionEvents[0].colliderComponent.name + "----->  Velocity: " + collisionEvents[0].velocity.magnitude + "%" + " ----->   particle number: " + ps.particleCount);

    //}

}