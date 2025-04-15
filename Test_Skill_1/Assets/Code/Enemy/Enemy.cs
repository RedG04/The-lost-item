using UnityEngine;

public class Enemy : Character
{
    [Header("AI Settings")]
    public new float moveSpeed = 2f;
    public float patrolRange = 3f;
    public float attackRange = 1.5f;
    public new int damage = 10;
    public float attackCooldown = 2f;

    private Vector2 startPos;
    private bool movingRight = true;
    private float nextAttackTime = 0f;
    private Transform player;

    protected override void Awake()
    {
        base.Awake();
        startPos = transform.position;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    private void Update()
    {
        Patrol();

        if (player != null && Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            if (Time.time >= nextAttackTime)
            {
                PerformAction(); // Attacca il player
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    // Movimento automatico di pattuglia
    private void Patrol()
    {
        float direction = movingRight ? 1f : -1f;
        Move(new Vector2(direction, 0f), moveSpeed);

        if (spriteRenderer != null)
            spriteRenderer.flipX = !movingRight;

        // Cambia direzione se supera i limiti di pattuglia
        if (movingRight && transform.position.x >= startPos.x + patrolRange)
            movingRight = false;
        else if (!movingRight && transform.position.x <= startPos.x - patrolRange)
            movingRight = true;
    }

    // Azione di attacco verso il player
    public override void PerformAction()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            Debug.Log("Nemico attacca il giocatore!");
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.UpdateHealth(-damage);
            }

            if (animator != null)
                animator.SetTrigger("Attack");
        }
    }
}
