using UnityEngine;

public class Holdable : MonoBehaviour
{
    public GameObject holdPoint;
    public float holdForce = 10f;
    public float holdMass = 0.1f;
    public bool isHeld = false;
    public float tolerance = 0f;
    public float releaseDistance = 2f;
    public bool lockRotation = false;
    public Vector3 rotationMultiplier;
    public Vector3 rotationOffset;

    private bool useGravity;
    private float defMass;


    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        useGravity = rb.useGravity;
        defMass = rb.mass;

        if (holdPoint == null)
        {
            holdPoint = GameObject.Find("PlayerHoldPoint");
        }
    }

    void Update()
    {
        if (isHeld && (holdPoint.transform.position - transform.position).magnitude > releaseDistance) Release();
        
        if (isHeld)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            if ((holdPoint.transform.position - transform.position).magnitude > tolerance)
            {
                Debug.Log(holdForce); // DEBUG
                rb.AddForce((holdPoint.transform.position - transform.position) * holdForce, ForceMode.Force);
            }
            rb.MoveRotation(Quaternion.Euler(new Vector3(holdPoint.transform.rotation.eulerAngles.x * rotationMultiplier.x + rotationOffset.x,
                                                         holdPoint.transform.rotation.eulerAngles.y * rotationMultiplier.y + rotationOffset.y,
                                                         holdPoint.transform.rotation.eulerAngles.z * rotationMultiplier.z + rotationOffset.z)));
        }
    }
    private void Hold()
    {
        isHeld = true;
        rb.useGravity = false;
        rb.mass = holdMass;
        if (lockRotation)
        {
            rb.freezeRotation = true;
        }
    }
    private void Release()
    {
        isHeld = false;
        rb.useGravity = useGravity;
        rb.mass = defMass;
        if (lockRotation)
        {
            rb.freezeRotation = false;
        }
    }
    public void Interact()
    {
        if (isHeld) Release();
        else Hold();
    }
}
