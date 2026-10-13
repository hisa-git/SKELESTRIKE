using System;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField]
    private float hp = 10;
    [SerializeField]
    float minDmg = 5f;
    [SerializeField]
    float hardness = 1f;
    [SerializeField]
    GameObject[] prefabsOnCollapse;
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
        if (prefabsOnCollapse.Length > 0)
        {
            GameObject prefab = prefabsOnCollapse[UnityEngine.Random.Range(0, prefabsOnCollapse.Length)];
            if (prefab != null)
            {
                Instantiate(prefabsOnCollapse[UnityEngine.Random.Range(0, prefabsOnCollapse.Length)], transform.position, transform.rotation);
            }
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<IgnoreForColDmg>() != null) return;

        //Vector3 colVel = collision.linearVelocity;
        //float colSpeed = colVel.magnitude;

        float mass = 1;
        Vector3 vel = new Vector3();
        if (GetComponent<Rigidbody>() != null)
        {
            mass = GetComponent<Rigidbody>().mass;
            vel = GetComponent<Rigidbody>().linearVelocity;
        }
        float otherMass = 10000;
        Vector3 otherVel = new Vector3();
        float otherHardness = 1f;
        if (collision.rigidbody != null)
        {
            otherMass = collision.rigidbody.mass;
            otherVel = collision.rigidbody.linearVelocity;
            if (collision.gameObject.GetComponent<Destructable>() != null)
            {
                otherHardness = collision.gameObject.GetComponent<Destructable>().hardness;
            }
        }

        //float kinE = mass * (vel.magnitude * vel.magnitude) * 0.5f;
        //float otherKinE = otherMass * (otherVel.magnitude * otherVel.magnitude) * 0.5f;
        //float colEnergy = kinE + otherKinE;
        
        /* OLD VARIANT
        float dmg = (((vel - otherVel).magnitude * (vel - otherVel).magnitude) * 0.5f + 
                    ((otherVel.magnitude / (vel.magnitude + otherVel.magnitude)) * (vel - otherVel).magnitude * (vel - otherVel).magnitude * otherMass * 0.5f))
                     * otherHardness * hardness; // this shit just works dont touch
        */

        float dmg = (((otherVel.magnitude / (vel.magnitude + otherVel.magnitude)) * ((vel - otherVel).magnitude * (vel - otherVel).magnitude) * (otherMass/(otherMass+mass)) * otherMass) +
                     ((vel.magnitude / (vel.magnitude + otherVel.magnitude)) * ((vel - otherVel).magnitude * (vel - otherVel).magnitude) * (otherMass/(otherMass+mass)) * mass))
                     * 0.5f * otherHardness * hardness; // this shit just works dont touch

        if (dmg > minDmg)
        {
            Damage(dmg);
            Debug.Log("DMGed: " + dmg);
        } 
        else
        {
            Debug.Log(dmg);
        }
    }
}

