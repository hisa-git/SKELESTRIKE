
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    public float interactDistance = 1f;
    public LayerMask interactLayer;
    private Interactable currentInteractable;

    public InputActionReference interactionAction;

    void OnEnable()
    {
        interactionAction.action.Enable();
        interactionAction.action.performed += Interact;
    }
    void OnDisable()
    {
        interactionAction.action.performed -= Interact;
        interactionAction.action.Disable();
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
        currentInteractable?.Interact();
        //Debug.Log("Player tried to interact with" + currentInteractable?.name);
    }
}
