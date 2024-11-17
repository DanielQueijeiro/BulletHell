using System.Collections;
using UnityEngine;

public class BossBulletSpawner : MonoBehaviour
{
    public GameObject projectile;
    public Transform firePoint;
    public float fireRate = 0.2f;
    public float moveSpeed = 2f;
    public float minY, maxY;
    public float minX = 3f, maxX = 8f;
    public Vector3 firePointOffset = new Vector3(0, 1f, 0);
    public float bulletSpeed = 5f;


    private enum ShootMode {Circle, Flower, Wave, Cross }
    private ShootMode currentMode;
    private float nextFireTime;
    private bool isShooting = true;
    private Vector3 targetPosition;
    private float angleOffset = 0f;
    private bool isMovingUp = true;
    private bool isMovingRight = true;

    void Start()
    {
        currentMode = ShootMode.Cross;
        Vector3 startPos = transform.position;
        startPos.x = Mathf.Clamp(startPos.x, minX, maxX);
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


    void FireCircle()
    {
        int bulletCount = 12;
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(projectile, firePoint.position, rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = bullet.transform.right * bulletSpeed;
            }
        }
    }

    void FireFlower()
    {
        int petalCount = 5;
        float baseAngle = angleOffset;
        
        for (int i = 0; i < petalCount; i++)
        {
            float angle = baseAngle + (i * (360f / petalCount));
            for (int j = 0; j < 3; j++) // 3 bullets per petal
            {
                float spreadAngle = angle + (j - 1) * 10f;
                Quaternion rotation = Quaternion.Euler(0, 0, spreadAngle);
                GameObject bullet = Instantiate(projectile, firePoint.position, rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = bullet.transform.right * bulletSpeed;
                }
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
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(projectile, firePoint.position, rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = bullet.transform.right * bulletSpeed;
            }
        }
    }

    void FireCross()
    {
        float[] angles = { 0, 90, 180, 270 };
        foreach (float angle in angles)
        {
            // Centro del cruz
            Quaternion rotation = Quaternion.Euler(0, 0, angle + angleOffset);
            GameObject bullet = Instantiate(projectile, firePoint.position, rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = bullet.transform.right * bulletSpeed;
            }

            // Bullets adicionales a los lados
            Quaternion rotationLeft = Quaternion.Euler(0, 0, angle + angleOffset - 15);
            Quaternion rotationRight = Quaternion.Euler(0, 0, angle + angleOffset + 15);
            
            GameObject bulletLeft = Instantiate(projectile, firePoint.position, rotationLeft);
            GameObject bulletRight = Instantiate(projectile, firePoint.position, rotationRight);
            
            Rigidbody2D rbLeft = bulletLeft.GetComponent<Rigidbody2D>();
            Rigidbody2D rbRight = bulletRight.GetComponent<Rigidbody2D>();
            
            if (rbLeft != null) rbLeft.linearVelocity = bulletLeft.transform.right * bulletSpeed;
            if (rbRight != null) rbRight.linearVelocity = bulletRight.transform.right * bulletSpeed;
        }
        angleOffset += 5f;
    }

    // El resto del código permanece igual...
    void Shoot()
    {
        switch (currentMode)
        {
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
            currentMode = (ShootMode)(((int)currentMode + 1) % 4);
        }
    }

    void MoveBoss()
    {
        Vector3 currentPos = transform.position;
        // Aplicar límites tanto vertical como horizontalmente
        currentPos.y = Mathf.Clamp(currentPos.y, minY, maxY);
        currentPos.x = Mathf.Clamp(currentPos.x, minX, maxX);
        transform.position = currentPos;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f ||
            transform.position.y >= maxY || transform.position.y <= minY ||
            transform.position.x >= maxX || transform.position.x <= minX)
        {
            SetNewTargetPosition();
        }
    }

    void SetNewTargetPosition()
    {
        // Manejar movimiento vertical
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

        // Manejar movimiento horizontal
        if (transform.position.x >= maxX)
        {
            isMovingRight = false;
        }
        else if (transform.position.x <= minX)
        {
            isMovingRight = true;
        }
        else
        {
            isMovingRight = Random.value > 0.5f;
        }

        // Calcular nueva posición Y
        float targetY;
        if (isMovingUp)
        {
            targetY = Mathf.Min(transform.position.y + Random.Range(2f, 4f), maxY);
        }
        else
        {
            targetY = Mathf.Max(transform.position.y - Random.Range(2f, 4f), minY);
        }

        // Calcular nueva posición X
        float targetX;
        if (isMovingRight)
        {
            targetX = Mathf.Min(transform.position.x + Random.Range(2f, 4f), maxX);
        }
        else
        {
            targetX = Mathf.Max(transform.position.x - Random.Range(2f, 4f), minX);
        }

        targetPosition = new Vector3(targetX, targetY, transform.position.z);
    }
}