using System;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField]
    private float hp = 10;
    [SerializeField]
    float minDmg = 5f;
    [SerializeField]
    float colDmgMultiplier = 1f;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    public void Damage(float dmg)
    {
        Debug.Log(gameObject + " damaged " + dmg); // DEBUG
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
        //Vector3 colVel = collision.linearVelocity;
        //float colSpeed = colVel.magnitude;

        float mass = 1;
        Vector3 vel = new Vector3();
        if (GetComponent<Rigidbody>() != null)
        {
            mass = GetComponent<Rigidbody>().mass;
            vel = GetComponent<Rigidbody>().linearVelocity;
        }
        float otherMass = 1;
        Vector3 otherVel = new Vector3();
        if (collision.rigidbody != null)
        {
            otherMass = collision.rigidbody.mass;
            otherVel = collision.rigidbody.linearVelocity;
        }

        float colSpeed = (vel - otherVel).magnitude;
        float effectiveMass = (mass * otherMass) / (mass + otherMass);
        float colEnergy = 0.5f * effectiveMass * (colSpeed * colSpeed);
        

        float dmg = colEnergy * colDmgMultiplier;
        if (dmg > minDmg) Damage(dmg);
    }
}

