using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
    public float speed = 10f;
    public float lifeTime = 2f;

    // Definimos los límites de la cámara
    private float minX = -9f;
    private float maxX = 9f;
    private float minY = -5f;
    private float maxY = 5f;

    private void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        
        // Verificamos si la bala está fuera de los límites
        Vector3 position = transform.position;
        if (position.x < minX || position.x > maxX || 
            position.y < minY || position.y > maxY)
        {
            Destroy(gameObject);
            return;
        }

        // Control del tiempo de vida
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Handle collision logic here (e.g., destroy the bullet)
        Destroy(gameObject);
    }


}
