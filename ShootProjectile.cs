using System.Collections;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public float forceMagnitude = 20f; // Constant force magnitude applied to the Projectile
    public float angleVariance = 3f; // Variance in angle
    public float forceVariance = 3f; // Variance in force
    public float ProjectileLifetime = 5f; // Time after which the Projectile gets destroyed
    public float finalForce = 20f;
    public AudioClip bow_load; // Drag your first MP3 file here in the inspector
    public AudioClip bow_release; // Drag your second MP3 file here in the inspector

    private AudioSource audioSource;
    private const float gravity = 9.81f; // Gravity constant
    private Transform firePoint; // Point from where the Projectile is fired
    private Transform target; // The target the Archer is aiming at

    /*private bool IsObstacleBetweenTarget()
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, target.position - firePoint.position, Vector2.Distance(firePoint.position, target.position), obstacleLayer);
        return hit.collider != null;
    }*/

    private void Awake()
    {
        firePoint = transform;
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (GetComponent<Archer>())
        {
            target = GetComponent<Archer>().target;
        }
    }
    private float CalculateFiringAngle(float distanceX, float distanceY)
    {
        // Calculate firing angle based on distance and constant force
        // Adjust calculation to account for Y position
        float angleRad = Mathf.Asin((gravity * distanceX) / Mathf.Pow(finalForce, 2)) / 2.0f;
        // Adjust for Y position difference
        angleRad += Mathf.Atan2(distanceY, distanceX);
        return angleRad * Mathf.Rad2Deg;
    }

    public void bowLoadSound()
    {
        audioSource.clip = bow_load; // Assign the second sound clip
        audioSource.Play();
    }

    public void FireProjectile()
    {
        if (target == null) return;

        Vector3 spawnPosition = new Vector3(firePoint.position.x, firePoint.position.y, 0.15f);
        audioSource.clip = bow_release; // Assign the second sound clip
        audioSource.Play();
        GameObject Projectile = Instantiate(ProjectilePrefab, spawnPosition, Quaternion.identity);
        Arrow arrowScript = Projectile.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.targetLayer = GetComponent<FindTarget>().targetLayer;
        }
        Rigidbody2D ProjectileRb = Projectile.GetComponent<Rigidbody2D>();

        Vector2 toTarget = target.position - firePoint.position;
        float distanceX = Mathf.Abs(toTarget.x);
        float distanceY = toTarget.y;

        finalForce = forceMagnitude + Random.Range(-forceVariance, forceVariance);

        //bool obstacleDetected = IsObstacleBetweenTarget();
        float firingAngle = CalculateFiringAngle(distanceX, distanceY/*, obstacleDetected*/) + Random.Range(-angleVariance, angleVariance);

        Vector2 forceDirection = new Vector2(Mathf.Sign(toTarget.x), 0).normalized;
        if (transform.localScale.x < 0) // Assuming negative scale means facing left
        {
            // Flip the angle if archer is facing left
            firingAngle = -firingAngle;
        }
        ProjectileRb.AddForce(Quaternion.Euler(0, 0, firingAngle) * forceDirection * finalForce, ForceMode2D.Impulse);

        StartCoroutine(DestroyProjectileAfterTime(Projectile, ProjectileLifetime));
    }

    public void FireProjectile(Transform setFirePoint)
    {
        if (target == null) return;

        Vector3 spawnPosition = setFirePoint.position;
        GameObject Projectile = Instantiate(ProjectilePrefab, spawnPosition, Quaternion.identity);
        Arrow arrowScript = Projectile.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.targetLayer = GetComponent<FindTarget>().targetLayer;
        }
        Rigidbody2D ProjectileRb = Projectile.GetComponent<Rigidbody2D>();

        Vector2 toTarget = target.position - setFirePoint.position;
        float distanceX = Mathf.Abs(toTarget.x);
        float distanceY = toTarget.y;

        finalForce = forceMagnitude + Random.Range(-forceVariance, forceVariance);

        //bool obstacleDetected = IsObstacleBetweenTarget();
        float firingAngle = CalculateFiringAngle(distanceX, distanceY/*, obstacleDetected*/) + Random.Range(-angleVariance, angleVariance);

        Vector2 forceDirection = new Vector2(Mathf.Sign(toTarget.x), 0).normalized;
        if (transform.localScale.x < 0) // Assuming negative scale means facing left
        {
            // Flip the angle if archer is facing left
            firingAngle = -firingAngle;
        }
        ProjectileRb.AddForce(Quaternion.Euler(0, 0, firingAngle) * forceDirection * finalForce, ForceMode2D.Impulse);

        StartCoroutine(DestroyProjectileAfterTime(Projectile, ProjectileLifetime));
    }

    public void FireProjectile(Transform setFirePoint, float minRange, float maxRange, float archHeight)
    {
        Vector3 spawnPosition = setFirePoint.position;
        GameObject Projectile = Instantiate(ProjectilePrefab, spawnPosition, Quaternion.identity);
        Arrow arrowScript = Projectile.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.targetLayer = GetComponent<FindTarget>().targetLayer;
        }
        Rigidbody2D ProjectileRb = Projectile.GetComponent<Rigidbody2D>();

        // Calculate a random target position within the specified range
        Vector2 randomTargetPosition = (Vector2)setFirePoint.position + new Vector2(Random.Range(minRange, maxRange), 0);

        // Modify the Y component of the toTarget vector based on the archHeight
        Vector2 toTarget = randomTargetPosition - (Vector2)setFirePoint.position;
        toTarget.y += archHeight; // Add arch height to the Y component

        float distanceX = Mathf.Abs(toTarget.x);
        float distanceY = toTarget.y;

        finalForce = forceMagnitude + Random.Range(-forceVariance, forceVariance);
        float firingAngle = CalculateFiringAngle(distanceX, distanceY) + Random.Range(-angleVariance, angleVariance);

        Vector2 forceDirection = new Vector2(Mathf.Sign(toTarget.x), 0).normalized;
        if (transform.localScale.x < 0)
        {
            firingAngle = -firingAngle;
        }
        ProjectileRb.AddForce(Quaternion.Euler(0, 0, firingAngle) * forceDirection * finalForce, ForceMode2D.Impulse);

        StartCoroutine(DestroyProjectileAfterTime(Projectile, ProjectileLifetime));
    }




    private IEnumerator DestroyProjectileAfterTime(GameObject Projectile, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(Projectile);
    }

}
