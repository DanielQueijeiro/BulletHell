using UnityEngine;

public class SharkBulletSpawner : MonoBehaviour
{
    private Shark enemyScript;
    public GameObject sharkPrefab;      // Prefab del tiburón
    public GameObject projectile;       // Proyectil a disparar
    public Transform firePoint;         // Punto de disparo
    public float fireRate = 0.5f;       // Velocidad de disparo
    public float moveSpeed = 2f;        // Velocidad de movimiento
    private Vector3 targetPosition;     // Posición objetivo
    private bool isMovingToTarget = true; // Estado de movimiento
    private float nextFireTime;
    public float minY, maxY;
    private bool isMovingUp = true;

    void Start()
    {
        enemyScript = sharkPrefab.GetComponent<Shark>();
        // Definir la posición objetivo inicial
        targetPosition = new Vector3(8f, 0f, 9.8f);
    }

    void Update()
    {
        if (isMovingToTarget)
        {
            MoveToTarget();
        }
        else
        {
            // Comenzar a disparar si está en posición
            if (Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + fireRate;
                FireWave();
            }
            MoveEnemy();
        }
    }

    void MoveToTarget()
    {
        enemyScript.isInvincible = true;
        // Moverse hacia la posición objetivo
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Verificar si ha llegado al destino
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMovingToTarget = false; // Cambiar al estado de disparo
            enemyScript.isInvincible = false;
        }
    }

    void MoveEnemy()
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

    void FireWave()
    {
        if (firePoint == null)
        {
            Debug.LogError("FirePoint no está asignado.");
            return;
        }

        float baseAngle = Mathf.Sin(Time.time * 2f) * 30f;
        int bulletCount = 5;
        float spreadAngle = 15f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = baseAngle + (i - bulletCount / 2) * spreadAngle;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(projectile, firePoint.position, rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = bullet.transform.right * 5f; // Velocidad del proyectil
            }
        }
    }
}
