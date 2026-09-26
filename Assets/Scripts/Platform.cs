using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool isCollidingPlayer = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlatformTrigger>() != null)
        {
           isCollidingPlayer = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlatformTrigger>() != null)
        {
           isCollidingPlayer = false;
        }
    }
}