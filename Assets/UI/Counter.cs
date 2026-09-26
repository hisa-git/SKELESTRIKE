using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    [SerializeField] private float punchScale = 1.35f;
    [SerializeField] private float jumpHeight = 50f;
    [SerializeField] private float rotationAmount = 4f;
    [SerializeField] private float animationDuration = 0.2f;

    private Vector3 basePosition;
    private Vector3 baseScale;
    private Quaternion baseRotation;

    private int value;
    private float animationTimer;

    private void Awake()
    {
        basePosition = transform.localPosition;
        baseScale = transform.localScale;
        baseRotation = transform.localRotation;

        value = CoolnessManager.Score;

        if (text != null)
            text.text = value.ToString();
    }

    private void OnEnable()
    {
        CoolnessManager.ScoreChanged += SetValue;
    }

    private void OnDisable()
    {
        CoolnessManager.ScoreChanged -= SetValue;
    }

    private void Update()
    {
        if (animationTimer <= 0f)
            return;

        animationTimer -= Time.deltaTime;

        float progress = 1f - animationTimer / animationDuration;
        float punch = Mathf.Sin(progress * Mathf.PI);

        transform.localPosition =
        basePosition + Vector3.up * (punch * jumpHeight);

        transform.localScale =
        baseScale * (1f + punch * (punchScale - 1f));

        transform.localRotation =
        baseRotation * Quaternion.Euler(
            0f,
            0f,
            punch * rotationAmount
        );

        if (animationTimer <= 0f)
        {
            transform.localPosition = basePosition;
            transform.localScale = baseScale;
            transform.localRotation = baseRotation;
        }
    }

    public void SetValue(int newValue)
    {
        value = newValue;

        if (text != null)
            text.text = value.ToString();

        animationTimer = animationDuration;
    }
}
