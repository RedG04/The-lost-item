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

    // Enum per macchina a stati
    protected enum CharacterState
    {
        Idle,
        Moving,
        Jumping,
        Attacking
    }

    protected CharacterState currentState = CharacterState.Idle;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (characterData != null)
        {
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

    // Movimento base
    public virtual void Move(Vector2 direction, float speed)
    {
        rb.linearVelocity = direction.normalized * speed;

        if (direction.magnitude > 0.1f)
            currentState = CharacterState.Moving;
        else
            currentState = CharacterState.Idle;

        if (animator != null)
            animator.SetFloat("Speed", direction.magnitude);
    }

    public virtual void Jump()
    {
        // Aggiungi la logica di salto qui se necessario
        currentState = CharacterState.Jumping;

        if (animator != null)
            animator.SetTrigger("Jump");
    }

    public virtual void Attack()
    {
        currentState = CharacterState.Attacking;

        if (animator != null)
            animator.SetTrigger("Attack");
    }

    // Gestione salute
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

    // Metodo opzionale per sincronizzare stato-animazione
    protected virtual void UpdateAnimatorState()
    {
        switch (currentState)
        {
            case CharacterState.Idle:
                animator.SetFloat("Speed", 0);
                break;
            case CharacterState.Moving:
                // Speed è già settato in Move
                break;
            case CharacterState.Jumping:
                animator.SetTrigger("Jump");
                break;
            case CharacterState.Attacking:
                animator.SetTrigger("Attack");
                break;
        }
    }

    public abstract void PerformAction();
}
