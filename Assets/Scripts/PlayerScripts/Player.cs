using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int startHp;
    int hp;
    public float invencibillityCooldown;
    float invencibillityTimer;
    void Start()
    {
        hp = startHp;
    }

    // Update is called once per frame
    void Update()
    {
        invencibillityTimer -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("aaaaaa");
        if (collision.gameObject.tag == "Bullet" && invencibillityTimer <= 0)
        {
            hp--;
            print("Player hit! HP: " + hp);
            invencibillityTimer = invencibillityCooldown;
            Destroy(collision.gameObject); // Destroy the bullet
        }
    }

}
