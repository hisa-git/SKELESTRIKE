using System;
using UnityEngine;

public class PhysInteractions : MonoBehaviour
{
    [SerializeField]
    private float hp = 10;
    [SerializeField]
    float minDmg = 5f;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Damage(float dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            Collapse();
        }
    }

    public void Collapse()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 vel = new Vector3();
        if (rb != null)
        {
            vel = rb.linearVelocity;
        }

        Vector3 otherVel = new Vector3();
        float otherMass = 1f;
        if (collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            otherVel = collision.gameObject.GetComponent<Rigidbody>().linearVelocity;
            otherMass = collision.gameObject.GetComponent<Rigidbody>().mass;
        }
        
        float dmgSpeed = (vel - otherVel).magnitude;
        float dmg = 0.5f * otherMass * dmgSpeed*dmgSpeed;
        Damage(Math.Min(dmg, minDmg));
    }
}
