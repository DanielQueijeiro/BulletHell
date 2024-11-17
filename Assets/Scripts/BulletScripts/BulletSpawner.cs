using System.Collections;
using UnityEngine;

public class BossShooting : MonoBehaviour
{
    public GameObject projectileType1;
    public GameObject projectileType2;
    public GameObject projectileType3;
    public Transform firePoint;
    public float fireRate = 0.2f;
    public float moveSpeed = 2f;
    public float minY, maxY;
    public Vector3 firePointOffset = new Vector3(0, 1f, 0);
    public float bulletSpeed = 5f;
    public float spiralSpeed = 100f;

    // Nuevas variables para manejar rotaciones iniciales de prefabs
    public Vector3 projectile1InitialRotation = new Vector3(0, -90, -90);
    public Vector3 projectile2InitialRotation = Vector3.zero;
    public Vector3 projectile3InitialRotation = Vector3.zero;

    private enum ShootMode { Spiral, Circle, Flower, Wave, Cross }
    private ShootMode currentMode;
    private float nextFireTime;
    private bool isShooting = true;
    private Vector3 targetPosition;
    private float angleOffset = 0f;
    private bool isMovingUp = true;

    void Start()
    {
        currentMode = ShootMode.Spiral;
        Vector3 startPos = transform.position;
        startPos.y = Mathf.Clamp(startPos.y, minY, maxY);
        transform.position = startPos;
        SetNewTargetPosition();
        StartCoroutine(ChangeShootingMode());
    }

    void Update()
    {
        if (isShooting && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
        MoveBoss();
        firePoint.position = transform.position + firePointOffset;
    }

    // Método auxiliar para instanciar proyectiles con rotación correcta
    private GameObject InstantiateBullet(GameObject prefab, Vector3 position, float angle, Vector3 initialRotation)
    {
        // Combinamos la rotación del patrón con la rotación inicial del prefab
        Quaternion patternRotation = Quaternion.Euler(0, 0, angle);
        Quaternion prefabRotation = Quaternion.Euler(initialRotation);
        Quaternion finalRotation = patternRotation * prefabRotation;

        GameObject bullet = Instantiate(prefab, position, finalRotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Calculamos la dirección basada en la rotación del patrón, no la rotación final
            Vector2 direction = patternRotation * Vector2.right;
            rb.linearVelocity = direction * bulletSpeed;
        }
        return bullet;
    }

    void FireSpiral()
    {
        float angle = angleOffset;
        int bulletCount = 3;
        float angleStep = 20f;

        for (int i = 0; i < bulletCount; i++)
        {
            float currentAngle = angle + (i * angleStep);
            InstantiateBullet(projectileType1, firePoint.position, currentAngle, projectile1InitialRotation);
        }
        angleOffset += spiralSpeed * Time.deltaTime;
    }

    void FireCircle()
    {
        int bulletCount = 12;
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            InstantiateBullet(projectileType2, firePoint.position, angle, projectile2InitialRotation);
        }
    }

    void FireFlower()
    {
        int petalCount = 5;
        float baseAngle = angleOffset;
        
        for (int i = 0; i < petalCount; i++)
        {
            float angle = baseAngle + (i * (360f / petalCount));
            for (int j = 0; j < 3; j++)
            {
                float spreadAngle = angle + (j - 1) * 10f;
                InstantiateBullet(projectileType3, firePoint.position, spreadAngle, projectile3InitialRotation);
            }
        }
        angleOffset += 10f;
    }

    void FireWave()
    {
        float baseAngle = Mathf.Sin(Time.time * 2f) * 30f;
        int bulletCount = 5;
        float spreadAngle = 15f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = baseAngle + (i - bulletCount/2) * spreadAngle;
            InstantiateBullet(projectileType1, firePoint.position, angle, projectile1InitialRotation);
        }
    }

    void FireCross()
    {
        float[] angles = { 0, 90, 180, 270 };
        foreach (float angle in angles)
        {
            float currentAngle = angle + angleOffset;
            InstantiateBullet(projectileType2, firePoint.position, currentAngle, projectile2InitialRotation);

            // Bullets adicionales a los lados
            InstantiateBullet(projectileType3, firePoint.position, currentAngle - 15, projectile3InitialRotation);
            InstantiateBullet(projectileType3, firePoint.position, currentAngle + 15, projectile3InitialRotation);
        }
        angleOffset += 5f;
    }

    // El resto del código permanece igual...
    void Shoot()
    {
        switch (currentMode)
        {
            case ShootMode.Spiral:
                FireSpiral();
                break;
            case ShootMode.Circle:
                FireCircle();
                break;
            case ShootMode.Flower:
                FireFlower();
                break;
            case ShootMode.Wave:
                FireWave();
                break;
            case ShootMode.Cross:
                FireCross();
                break;
        }
    }

    IEnumerator ChangeShootingMode()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            currentMode = (ShootMode)(((int)currentMode + 1) % 5);
        }
    }

    void MoveBoss()
    {
        Vector3 currentPos = transform.position;
        currentPos.y = Mathf.Clamp(currentPos.y, minY, maxY);
        transform.position = currentPos;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f ||
            transform.position.y >= maxY || transform.position.y <= minY)
        {
            SetNewTargetPosition();
        }
    }

    void SetNewTargetPosition()
    {
        if (transform.position.y >= maxY)
        {
            isMovingUp = false;
        }
        else if (transform.position.y <= minY)
        {
            isMovingUp = true;
        }
        else
        {
            isMovingUp = Random.value > 0.5f;
        }

        float targetY;
        if (isMovingUp)
        {
            targetY = Mathf.Min(transform.position.y + Random.Range(2f, 4f), maxY);
        }
        else
        {
            targetY = Mathf.Max(transform.position.y - Random.Range(2f, 4f), minY);
        }

        targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
    }
}