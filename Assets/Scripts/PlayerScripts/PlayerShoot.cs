using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectile; // El proyectil a disparar
    public Transform firePoint;   // El punto de salida del proyectil
    public float bulletSpeed = 10f; // Velocidad del proyectil
    public float fireRate = 0.2f;  // Intervalo entre disparos (en segundos)

    private float nextFireTime = 0f; // Tiempo hasta el próximo disparo permitido

    void Update()
    {
        // Detectar si se presiona la barra espaciadora y se permite disparar
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate; // Actualizar el tiempo para el siguiente disparo
        }
    }

    void Shoot()
    {
        // Crear una instancia del proyectil en el firePoint con la rotación actual
        GameObject bullet = Instantiate(projectile, firePoint.position, firePoint.rotation);

        // Aplicar velocidad al proyectil
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * bulletSpeed; // Ajustar para moverse en línea recta
        }
    }
}
