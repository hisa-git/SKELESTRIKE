using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] private float power = 10f;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float damage = 10f;

    [Header("Behaviour")]
    [SerializeField] private LayerMask affectedLayers = ~0;
    [SerializeField] private bool destroyAfterExplosion = true;

    private bool exploded;

    public float Power => power;
    public float Radius => radius;
    public float DamageAmount => damage;
    public bool IsExploded => exploded;

    public void Explode()
    {
        Explode(transform.position);
    }

    public void Explode(Vector3 position)
    {
        if (exploded)
            return;

        exploded = true;

        Collider[] colliders = Physics.OverlapSphere(
            position,
            radius,
            affectedLayers
        );

        foreach (Collider collider in colliders)
        {
            Destructable target = collider.GetComponentInParent<Destructable>();

            if (target != null)
            {
                float distance = Vector3.Distance(
                    position,
                    target.transform.position
                );

                float multiplier = 1f - Mathf.Clamp01(distance / radius);
                float finalDamage = damage * multiplier;

                target.Damage(finalDamage);
            }

            Rigidbody rb = collider.attachedRigidbody;

            if (rb != null)
            {
                rb.AddExplosionForce(
                    power,
                    position,
                    radius,
                    1f,
                    ForceMode.Impulse
                );
            }
        }

        if (destroyAfterExplosion)
            Destroy(gameObject);
    }

    public void SetPower(float value)
    {
        power = value;
    }

    public void SetRadius(float value)
    {
        radius = value;
    }

    public void SetDamage(float value)
    {
        damage = value;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
