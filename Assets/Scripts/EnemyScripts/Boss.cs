using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Stats")]
    public int startHp;
    public int hp;
    
    [Header("Invincibility Settings")]
    public float invincibilityDuration = 2f;
    public float blinkInterval = 0.2f;
    
    [Header("Scale Pulse Settings")]
    public float scalePulseDuration = 0.3f;
    public float maxScaleMultiplier = 1.2f; // Aumentará hasta un 120% del tamaño original
    
    private float invincibilityTimer;
    private float blinkTimer;
    private float scalePulseTimer;
    private bool isInvincible = false;
    private bool isPulsing = false;
    private MeshRenderer meshRenderer;
    private bool isVisible = true;
    private Vector3 originalScale;
    private Vector3 targetScale;

    void Start()
    {
        hp = startHp;
        meshRenderer = GetComponent<MeshRenderer>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isInvincible)
        {
            HandleInvincibility();
            HandleBlinking();
        }

        if (isPulsing)
        {
            HandleScalePulse();
        }
    }

    void HandleScalePulse()
    {
        if (scalePulseTimer < scalePulseDuration)
        {
            scalePulseTimer += Time.deltaTime;
            float progress = scalePulseTimer / scalePulseDuration;
            
            // Usar una curva sinusoidal para el efecto de pulso
            float curveProgress = Mathf.Sin(progress * Mathf.PI);
            float currentMultiplier = 1f + (maxScaleMultiplier - 1f) * curveProgress;
            
            transform.localScale = originalScale * currentMultiplier;
            
            if (progress >= 1f)
            {
                isPulsing = false;
                transform.localScale = originalScale; // Asegurar que vuelva al tamaño original
            }
        }
    }

    void StartScalePulse()
    {
        isPulsing = true;
        scalePulseTimer = 0f;
        targetScale = originalScale * maxScaleMultiplier;
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
        if(isInvincible)
        {
            return;
        }

        StartScalePulse(); // Iniciar el efecto de pulso

        hp -= (int)damage;
        if (hp <= 0)
        {
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
            }
            GameManager.Instance.EndGame("Victoria", gameObject);
        }
        else
        {
            isInvincible = true;
            invincibilityTimer = invincibilityDuration;
            blinkTimer = blinkInterval;
        }
    }
}