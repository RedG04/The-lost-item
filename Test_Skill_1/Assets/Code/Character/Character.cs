using UnityEngine;

// Struct per la salute
public struct Health
{
    public int maxHealth;
    public int currentHealth;
    public bool isInvulnerable;

    public Health(int maxHealth)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth;
        this.isInvulnerable = false;
    }

    public void TakeDamage(int amount)
    {
        if (!isInvulnerable)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Max(currentHealth, 0);
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }
}

// Classe base astratta
public abstract class Character : MonoBehaviour
{
    [Header("Character Settings")]
    public CharacterData characterData;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    public Health health;
    public float moveSpeed;
    public int damage;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (characterData != null)
        {
            // Carica i dati
            moveSpeed = characterData.moveSpeed;
            damage = characterData.damage;
            health = new Health(characterData.maxHealth);

            if (spriteRenderer != null && characterData.characterSprite != null)
            {
                spriteRenderer.sprite = characterData.characterSprite;
            }
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} non ha un CharacterData assegnato!");
            health = new Health(100); // default
        }
    }

    public virtual void Move(Vector2 direction, float speed)
    {
        rb.linearVelocity = direction.normalized * speed;

        if (animator != null)
            animator.SetFloat("Speed", direction.magnitude);
    }

    public virtual void UpdateHealth(int amount)
    {
        if (amount < 0)
        {
            health.TakeDamage(-amount);
        }
        else
        {
            health.Heal(amount);
        }
    }

    public abstract void PerformAction();
}
