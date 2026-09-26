using System.Collections.Generic;
using UnityEngine;

public class BuildingPart : MonoBehaviour
{
    //public float loadTime = 10f;
    Rigidbody rb;
    FixedJoint[] joints;
    [SerializeField]
    bool isStatic = true;
    public int groundDistance = 0;
    bool groundConnected = false;
    public List<GameObject> connectedObjs = new List<GameObject>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //joints = GetJoints();
        rb.isKinematic = isStatic;
        gameObject.layer = 6;
        rb.excludeLayers = LayerMask.GetMask("Static Environment");

        Collider[] hitColliders = Physics.OverlapBox(transform.position, (transform.localScale / 2) + new Vector3(0.2f, 0.2f, 0.2f), transform.rotation, LayerMask.GetMask("Static Environment"));
        foreach (Collider col in hitColliders)
        {
            if (col.gameObject != gameObject && col.GetComponent<BuildingPart>() != null)
            {
                connectedObjs.Add(col.gameObject);
            }
        }
    }

    void Update()
    {
        /*
        if (RemoveEmptyJoints())
        {
            joints = GetJoints();
        } */

        //loadTime -= Time.deltaTime;
        

        if (isStatic)
        {
            //if (loadTime <= 0)
            //{

            List<int> distances = new List<int>();
            distances.Add(101);
            List<GameObject> objs2Del = new List<GameObject>();
            groundConnected = false;
            
            
                foreach (GameObject obj in connectedObjs)
                {
                    if (obj == null)
                    {
                        objs2Del.Add(obj);
                    }
                    else
                    {
                        if (obj.GetComponent<BuildingPart>() != null)
                        {
                            distances.Add(obj.GetComponent<BuildingPart>().groundDistance);
                        }
                        else
                        {
                            groundConnected = true;
                        }
                    }
                }
                foreach (GameObject obj in objs2Del)
                {
                    connectedObjs.Remove(obj);
                }

                if (groundConnected)
                {
                    groundDistance = 0;
                }
                else
                {
                    groundDistance = Mathf.Min(distances.ToArray()) + 1;
                }
            //}

            if (groundDistance > 100)
            {
                isStatic = false;
                rb.isKinematic = false;
                gameObject.layer = 0;
                rb.excludeLayers = LayerMask.GetMask("Nothing");
                foreach (GameObject obj in connectedObjs)
                {
                    if (obj.GetComponent<Rigidbody>() != null)
                    {
                        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
                        joint.connectedBody = obj.GetComponent<Rigidbody>();
                    }
                }
            }
        }

        else
        {
            joints = GetJoints();
            RemoveEmptyJoints();
        }
    }

    FixedJoint[] GetJoints()
    {
        return GetComponents<FixedJoint>();
    }

    bool RemoveEmptyJoints()
    {
        bool result = false;
        foreach (FixedJoint jnt in joints)
        {
            if (jnt.connectedBody == null)
            {
                Destroy(jnt);
                result = true;
            }
        }
        return result;
    }

    /*
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger" + other.gameObject);
        if (loadTime > 0 && isStatic && other.GetComponent<BuildingPart>() != null && other.GetComponent<BuildingPart>().isStatic)
        {
            connectedObjs.Add(other.gameObject);
        }
    }
    */
}
