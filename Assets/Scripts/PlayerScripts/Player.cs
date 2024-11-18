using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public int startHp;
    public int hp;
    
    [Header("Invincibility Settings")]
    public float invincibilityDuration = 2f;
    public float blinkInterval = 0.2f;
    
    // Referencias a componentes
    [Header("References")]
    [SerializeField] private MeshRenderer modelRenderer;

    private MeshRenderer[] allRenderers;
    
    private float invincibilityTimer;
    private float blinkTimer;
    private bool isInvincible = false;
    private bool isVisible = true;

    void Start()
    {
        hp = startHp;
        

        if (modelRenderer == null)
        {
            modelRenderer = GetComponentInChildren<MeshRenderer>();
        }

        allRenderers = GetComponentsInChildren<MeshRenderer>();
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

        if (invincibilityTimer <= 0)
        {
            isInvincible = false;
            SetRenderersVisible(true);
        }
    }

    void HandleBlinking()
    {
        blinkTimer -= Time.deltaTime;
        
        if (blinkTimer <= 0)
        {
            blinkTimer = blinkInterval;
            isVisible = !isVisible;
            SetRenderersVisible(isVisible);
        }
    }

    void SetRenderersVisible(bool visible)
    {

        foreach (var renderer in allRenderers)
        {
            renderer.enabled = visible;
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
            SetRenderersVisible(true);
            GameManager.Instance.EndGame("Game Over", gameObject);
        }
        else
        {
            isInvincible = true;
            invincibilityTimer = invincibilityDuration;
            blinkTimer = blinkInterval;
        }
    }
}