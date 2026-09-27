
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    public float interactDistance = 1f;
    public LayerMask interactLayer;
    private Interactable currentInteractable;

    public InputActionReference[] interactionActions;

    void OnEnable()
    {
        foreach (InputActionReference interactionAction in interactionActions)
        {
            interactionAction.action.Enable();
            interactionAction.action.performed += Interact;
        }
    }
    void OnDisable()
    {
        foreach (InputActionReference interactionAction in interactionActions)
        {
            interactionAction.action.performed -= Interact;
            interactionAction.action.Disable();
        }
    }

    void Update()
    {
        CheckInteractable();
    }

    void CheckInteractable()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
        {
            Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();

            currentInteractable = interactable;
        }
        else currentInteractable = null;
    }

    void Interact(InputAction.CallbackContext context)
    {
        currentInteractable?.Interact(GetAllActiveActions()[0]);
        //Debug.Log("Player tried to interact with" + currentInteractable?.name);
    }

    InputActionReference[] GetAllActiveActions()
    {
        List<InputActionReference> activeActions = new List<InputActionReference>();
        foreach (InputActionReference interactionAction in interactionActions)
        {
            if (interactionAction.action.phase == InputActionPhase.Performed)
            {
                activeActions.Add(interactionAction);
            }
        }
        return activeActions.ToArray();
    }
}
