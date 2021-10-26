using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleCollison : MonoBehaviour
{
    [SerializeField] GameObject effectsOnCollision;
    
    ParticleSystem ps;

    [SerializeField] float offset = 0;
    [SerializeField] Vector3 rotationOffset = new Vector3(0, 0, 0);
    [SerializeField] bool useFirePointRotation;
    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = ps.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            var effect = Instantiate(effectsOnCollision, collisionEvents[i].intersection + collisionEvents[i].normal * offset, new Quaternion());

            if (useFirePointRotation)
            {
                effect.transform.LookAt(transform.position);
            }
            else
            {
                effect.transform.LookAt(collisionEvents[i].intersection + collisionEvents[i].normal);
                effect.transform.rotation *= Quaternion.Euler(rotationOffset);
            }
        }
    }
}
