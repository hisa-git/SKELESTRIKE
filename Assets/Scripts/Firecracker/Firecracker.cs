using UnityEngine;

public class Firecracker : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] private float explosionForce = 3f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float minImpactSpeed = 2f;

    [Header("Explosion Behaviour")]
    [SerializeField] private bool destroyAfterExplosion = true;
    [SerializeField] private LayerMask damageLayers = ~0;

    private bool exploded;

    private void OnCollisionEnter(Collision collision)
    {
        if (exploded)
            return;

        float impactSpeed = collision.relativeVelocity.magnitude;

        if (impactSpeed < minImpactSpeed)
            return;

        Explode();
    }

    public void Explode()
    {
        if (exploded)
            return;

        exploded = true;

        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            explosionRadius,
            damageLayers
        );

        foreach (Collider collider in colliders)
        {
            PhysInteractions target = collider.GetComponentInParent<PhysInteractions>();

            if (target != null)
            {
                float distance = Vector3.Distance(
                    transform.position,
                    target.transform.position
                );

                float distanceMultiplier = 1f - Mathf.Clamp01(distance / explosionRadius);
                float finalDamage = damage * distanceMultiplier;

                target.Damage(finalDamage);
            }

            Rigidbody targetRb = collider.attachedRigidbody;

            if (targetRb != null)
            {
                targetRb.AddExplosionForce(
                    explosionForce,
                    transform.position,
                    explosionRadius,
                    1f,
                    ForceMode.Impulse
                );
            }
        }

        if (destroyAfterExplosion)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
