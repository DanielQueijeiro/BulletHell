using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
    public float speed = 10f;
    public string shooterTag; // Nueva variable para almacenar quién disparó

    // Definimos los límites de la cámara
    private float minX = -9f;
    private float maxX = 9f;
    private float minY = -5.5f;
    private float maxY = 5f;

    // Método para establecer quién disparó la bala
    public void SetShooter(string tag)
    {
        shooterTag = tag;
    }

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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        // Ignorar colisión si la bala golpea a quien la disparó
        if (other.CompareTag(shooterTag))
        {
            return;
        }

        // Check if collision is target via tag - needs target gameObjects to be tagged properly
        if (other.CompareTag("Boss"))
        {
            // Target has been hit - use script on target to deal damage to it
            float dmg = 1;
            other.GetComponent<Boss>().TakeDamage(dmg);
            // Damage has been dealt, can destroy self now
            Destroy(gameObject);
        }
        else if (other.CompareTag("Shark"))
        {
            // Target has been hit - use script on target to deal damage to it
            float dmg = 1;
            other.GetComponent<Shark>().TakeDamage(dmg);
            // Damage has been dealt, can destroy self now
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            // Player has been hit - use script on player to deal damage to it
            float dmg = 1;
            other.GetComponent<Player>().TakeDamage(dmg);
            // Damage has been dealt, can destroy self now
            Destroy(gameObject);
        }
    }
}