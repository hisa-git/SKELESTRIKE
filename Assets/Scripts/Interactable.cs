using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent action;

    public void Interact()
    {
        action?.Invoke();
    }
}
