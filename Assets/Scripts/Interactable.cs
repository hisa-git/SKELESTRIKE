using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    public UnityEvent action;
    public InputActionReference inputType;

    public void Interact(InputActionReference interactedInputType)
    {
        if (interactedInputType == inputType) action?.Invoke();
    }
}
