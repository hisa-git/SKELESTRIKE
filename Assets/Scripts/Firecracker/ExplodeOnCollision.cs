using UnityEngine;

[RequireComponent(typeof(Explosion))]
public class ExplodeOnCollision : MonoBehaviour
{
    [Header("Collision")]
    [SerializeField] private float minImpactSpeed = 2f;

    private Explosion explosion;

    private void Awake()
    {
        explosion = GetComponent<Explosion>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (explosion.IsExploded)
            return;

        float impactSpeed = collision.relativeVelocity.magnitude;

        if (impactSpeed < minImpactSpeed)
            return;

        explosion.Explode();
    }
}
