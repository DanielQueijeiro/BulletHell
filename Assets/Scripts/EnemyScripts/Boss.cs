using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Stats")]
    public int startHp;
    public int hp;
    
    [Header("Invincibility Settings")]
    public float invincibilityDuration = 2f;
    public float blinkInterval = 0.2f;  // Tiempo entre cada parpadeo
    
    private float invincibilityTimer;
    private float blinkTimer;
    private bool isInvincible = false;
    private MeshRenderer meshRenderer;
    private bool isVisible = true;

    void Start()
    {
        hp = startHp;
        meshRenderer = GetComponent<MeshRenderer>();
        
        // Si el objeto tiene varios MeshRenderers hijos, puedes usar:
        // meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    void Update()
    {
        if (isInvincible)
        {
            HandleInvincibility();
            HandleBlinking();
        }
    }

    void HandleInvincibility()
    {
        invincibilityTimer -= Time.deltaTime;

        // Verificar si la invulnerabilidad ha terminado
        if (invincibilityTimer <= 0)
        {
            isInvincible = false;
            // Asegurarnos de que el renderer quede visible al terminar
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
            }
        }
    }

    void HandleBlinking()
    {
        blinkTimer -= Time.deltaTime;
        
        if (blinkTimer <= 0)
        {
            blinkTimer = blinkInterval;
            isVisible = !isVisible;
            
            if (meshRenderer != null)
            {
                meshRenderer.enabled = isVisible;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if(isInvincible)
        {
            return;
        }

        hp -= (int)damage;
        if (hp <= 0)
        {
            // Asegurarnos de que el renderer esté visible antes de destruir
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
            }
            Destroy(gameObject);
        }
        else
        {
            isInvincible = true;
            invincibilityTimer = invincibilityDuration;
            blinkTimer = blinkInterval;  // Reiniciar el timer de parpadeo
        }
    }
}
