using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float braking = 10f;
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private bool tankRotation;

    [Header("Jump")]
    [SerializeField] private bool canJump = true;
    [SerializeField] private float jumpForce = 7f;

    [Header("Tilt")]
    [SerializeField] private bool ejectPlayerWhenTilted = true;
    [SerializeField] private float maxTiltAngle = 70f;

    [Header("Interaction")]
    [SerializeField] private float interactionRadius = 3f;
    [SerializeField] private string playerTag = "Player";

    [Header("Player")]
    [SerializeField] private Vector3 seatOffset = new(0f, 1f, 0f);
    [SerializeField] private Vector3 cameraOffset = new(0f, 1.5f, 0.5f);
    [SerializeField] private float cameraDownAngle = 8f;
    [SerializeField] private Vector3 exitOffset = new(1.5f, 0f, 0f);

    [Header("Player Components")]
    [SerializeField] private MonoBehaviour playerMovement;
    [SerializeField] private MonoBehaviour playerCameraLook;

    private GameObject player;
    private Camera playerCamera;
    private CharacterController characterController;
    private Collider[] playerColliders;

    private Transform originalCameraParent;
    private Vector3 originalCameraLocalPosition;
    private Quaternion originalCameraLocalRotation;

    private float currentSpeed;
    private bool occupied;

    public bool IsOccupied => occupied;
    public float CurrentSpeed => currentSpeed;

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (occupied)
        {
            HandleVehicle();

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                ExitVehicle();
                return;
            }

            if (canJump && Keyboard.current.spaceKey.wasPressedThisFrame)
                Jump();

            CheckTilt();

            return;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
            TryEnterVehicle();
    }

    private void TryEnterVehicle()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            interactionRadius
        );

        foreach (Collider collider in colliders)
        {
            GameObject candidate = collider.transform.root.gameObject;

            if (!candidate.CompareTag(playerTag))
                continue;

            Camera camera = candidate.GetComponentInChildren<Camera>();

            if (camera == null)
                continue;

            EnterVehicle(candidate, camera);
            return;
        }
    }

    private void EnterVehicle(GameObject playerObject, Camera camera)
    {
        if (occupied)
            return;

        player = playerObject;
        playerCamera = camera;

        characterController = player.GetComponent<CharacterController>();
        playerColliders = player.GetComponentsInChildren<Collider>();

        SaveCameraTransform();
        DisablePlayer();

        player.transform.SetParent(transform);
        player.transform.localPosition = seatOffset;
        player.transform.localRotation = Quaternion.identity;

        playerCamera.transform.SetParent(player.transform);
        playerCamera.transform.localPosition = cameraOffset;
        playerCamera.transform.localRotation =
            Quaternion.Euler(cameraDownAngle, 0f, 0f);

        occupied = true;
    }

    private void ExitVehicle()
    {
        if (!occupied || player == null)
            return;

        currentSpeed = 0f;

        Vector3 exitPosition = transform.TransformPoint(exitOffset);

        Quaternion exitRotation = Quaternion.Euler(
            0f,
            transform.eulerAngles.y,
            0f
        );

        player.transform.SetParent(null);

        player.transform.SetPositionAndRotation(
            exitPosition,
            exitRotation
        );

        RestoreCameraTransform();
        EnablePlayer();

        player = null;
        playerCamera = null;
        characterController = null;
        playerColliders = null;

        occupied = false;
    }

    private void Jump()
    {
        transform.position += Vector3.up * jumpForce * Time.deltaTime;
    }

    private void CheckTilt()
    {
        if (!ejectPlayerWhenTilted || !occupied)
            return;

        float tiltAngle = Vector3.Angle(
            transform.up,
            Vector3.up
        );

        if (tiltAngle >= maxTiltAngle)
            EjectPlayer();
    }

    private void EjectPlayer()
    {
        if (!occupied || player == null)
            return;

        Vector3 ejectPosition = transform.position + Vector3.up * 2f;

        Quaternion ejectRotation = Quaternion.Euler(
            0f,
            transform.eulerAngles.y,
            0f
        );

        player.transform.SetParent(null);

        player.transform.SetPositionAndRotation(
            ejectPosition,
            ejectRotation
        );

        RestoreCameraTransform();
        EnablePlayer();

        player = null;
        playerCamera = null;
        characterController = null;
        playerColliders = null;

        occupied = false;
        currentSpeed = 0f;
    }

    private void HandleVehicle()
    {
        float forwardInput = 0f;
        float turnInput = 0f;

        if (Keyboard.current.wKey.isPressed)
            forwardInput += 1f;

        if (Keyboard.current.sKey.isPressed)
            forwardInput -= 1f;

        if (Keyboard.current.dKey.isPressed)
            turnInput += 1f;

        if (Keyboard.current.aKey.isPressed)
            turnInput -= 1f;

        HandleAcceleration(forwardInput);
        MoveVehicle();
        RotateVehicle(turnInput);

        if (player != null)
        {
            player.transform.localPosition = seatOffset;
            player.transform.localRotation = Quaternion.identity;
        }

        if (playerCamera != null)
        {
            playerCamera.transform.localPosition = cameraOffset;
            playerCamera.transform.localRotation =
                Quaternion.Euler(cameraDownAngle, 0f, 0f);
        }
    }

    private void HandleAcceleration(float input)
    {
        if (Mathf.Abs(input) > 0.01f)
        {
            float targetSpeed = input * maxSpeed;

            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                targetSpeed,
                acceleration * Time.deltaTime
            );
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                0f,
                braking * Time.deltaTime
            );
        }
    }

    private void MoveVehicle()
    {
        transform.position +=
            transform.forward *
            currentSpeed *
            Time.deltaTime;
    }

    private void RotateVehicle(float input)
    {
        if (Mathf.Abs(input) < 0.01f)
            return;

        float multiplier = tankRotation
            ? 1f
            : Mathf.Abs(currentSpeed) / maxSpeed;

        float direction = currentSpeed >= 0f ? 1f : -1f;

        float rotation =
            input *
            rotationSpeed *
            multiplier *
            direction *
            Time.deltaTime;

        transform.Rotate(0f, rotation, 0f);
    }

    private void DisablePlayer()
    {
        if (characterController != null)
            characterController.enabled = false;

        if (playerColliders != null)
        {
            foreach (Collider collider in playerColliders)
                collider.enabled = false;
        }

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (playerCameraLook != null)
            playerCameraLook.enabled = false;
    }

    private void EnablePlayer()
    {
        if (characterController != null)
            characterController.enabled = true;

        if (playerColliders != null)
        {
            foreach (Collider collider in playerColliders)
                collider.enabled = true;
        }

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (playerCameraLook != null)
            playerCameraLook.enabled = true;
    }

    private void SaveCameraTransform()
    {
        originalCameraParent = playerCamera.transform.parent;
        originalCameraLocalPosition = playerCamera.transform.localPosition;
        originalCameraLocalRotation = playerCamera.transform.localRotation;
    }

    private void RestoreCameraTransform()
    {
        playerCamera.transform.SetParent(originalCameraParent);

        playerCamera.transform.localPosition =
            originalCameraLocalPosition;

        playerCamera.transform.localRotation =
            originalCameraLocalRotation;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            interactionRadius
        );

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(
            transform.TransformPoint(seatOffset),
            0.2f
        );

        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(
            transform.TransformPoint(cameraOffset),
            0.2f
        );

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.TransformPoint(exitOffset),
            0.2f
        );
    }
}
