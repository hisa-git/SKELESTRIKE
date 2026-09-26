using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public int _platformsCollisionCounter;
    private int platformsCollisionCounter {get => _platformsCollisionCounter;
                                           set
                                           {
                                                _platformsCollisionCounter = value;
                                                if (_platformsCollisionCounter == 0) isOnPlatform = false;
                                                else isOnPlatform = true;
                                           } }
    public bool isOnPlatform {get; set;}


    // collision enter
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<PlayerMovement>() != null) return;

        //if (collider.GetComponent<Platform>() != null)
        //{
            platformsCollisionCounter += 1;
        //}
    }
    // collision exit
    private void OnTriggerExit(Collider collider)
    {
        if (collider.GetComponent<PlayerMovement>() != null) return;

        //if (collider.GetComponent<Platform>() != null)
        //{
            platformsCollisionCounter -= 1;;
        //}
    }
}
