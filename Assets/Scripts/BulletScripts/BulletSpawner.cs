using UnityEngine;

public class EnemyBulletSpawner : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float fireRate = 1f;
    
    [Header("Spawn Pattern Settings")]
    [SerializeField] private int bulletsPerShot = 8;
    [SerializeField] private float spreadAngle = 360f;
    [SerializeField] private bool rotatePattern = true;
    [SerializeField] private float rotationSpeed = 30f;
    
    private float currentRotation = 0f;
    private float nextFireTime = 0f;
    
    private void Update()
    {
        if (Time.time >= nextFireTime)
        {
            SpawnBulletPattern();
            nextFireTime = Time.time + fireRate;
        }
        
        if (rotatePattern)
        {
            currentRotation += rotationSpeed * Time.deltaTime;
            if (currentRotation >= 360f) currentRotation -= 360f;
        }
    }
    
    private void SpawnBulletPattern()
    {
        float angleStep = spreadAngle / bulletsPerShot;
        float angle = -spreadAngle / 2;
        
        for (int i = 0; i < bulletsPerShot; i++)
        {
            float bulletDirX = transform.position.x + Mathf.Sin((angle + currentRotation) * Mathf.Deg2Rad);
            float bulletDirY = transform.position.y + Mathf.Cos((angle + currentRotation) * Mathf.Deg2Rad);
            
            Vector3 bulletMoveVector = new Vector3(bulletDirX, bulletDirY, 0f) - transform.position;
            Vector3 bulletDirection = bulletMoveVector.normalized;
            
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            
            if (rb != null)
            {
                rb.linearVelocity = bulletDirection * bulletSpeed;
            }
            
            angle += angleStep;
        }
    }
}