using UnityEngine;

public class Shark : MonoBehaviour
{
    [Header("Stats")]
    public int startHp;
    public int hp;
    
    [Header("Invincibility Settings")]
    public float invincibilityDuration = 2f;
    public float blinkInterval = 0.2f;
    
    [Header("Rotation Settings")]
    public float rotationSpeed = 720f; // Velocidad de rotación en grados por segundo
    
    private float invincibilityTimer;
    private float blinkTimer;
    public bool isInvincible = false;
    private bool isRotating = false;
    private float rotationAmount = 0f;
    private MeshRenderer meshRenderer;
    private bool isVisible = true;

    public delegate void SharkDefeated(Shark shark);
    public static event SharkDefeated OnSharkDefeated;

    void Start()
    {
        hp = startHp;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (isInvincible)
        {
            HandleInvincibility();
            HandleBlinking();
        }

        if (isRotating)
        {
            // Rotar el tiburón
            float rotationThisFrame = rotationSpeed * Time.deltaTime;
            transform.Rotate(0, rotationThisFrame, 0);
            
            rotationAmount += rotationThisFrame;
            
            // Cuando completamos 360 grados, detenemos la rotación
            if (rotationAmount >= 360f)
            {
                isRotating = false;
                rotationAmount = 0f;
            }
        }
    }

    void HandleInvincibility()
    {
        invincibilityTimer -= Time.deltaTime;

        if (invincibilityTimer <= 0)
        {
            isInvincible = false;
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
        if (isInvincible)
        {
            return;
        }

        // Iniciar la rotación
        isRotating = true;
        rotationAmount = 0f;

        hp -= (int)damage;
        if (hp <= 0)
        {
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
            }

            OnSharkDefeated?.Invoke(this);
            Destroy(gameObject);
        }
        else
        {
            isInvincible = true;
            invincibilityTimer = invincibilityDuration;
            blinkTimer = blinkInterval;
        }
    }
}