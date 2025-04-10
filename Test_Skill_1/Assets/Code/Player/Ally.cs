using UnityEngine;

public class Ally : Character
{
    [Header("Support Settings")]
    public float followDistance = 2f;
    public float moveSpeed = 3f;
    public float supportRange = 2f;
    public float healAmount = 10f;
    public float buffDuration = 5f;
    public float supportCooldown = 3f;

    private Transform player;
    private float nextSupportTime = 0f;
    private bool isBuffing = false;

    protected override void Awake()
    {
        base.Awake();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    private void Update()
    {
        if (player == null) return;

        FollowPlayer();

        if (Time.time >= nextSupportTime && Vector2.Distance(transform.position, player.position) <= supportRange)
        {
            PerformAction(); // Supporto attivo
            nextSupportTime = Time.time + supportCooldown;
        }
    }

    private void FollowPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > followDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            Move(direction, moveSpeed);
        }
        else
        {
            Move(Vector2.zero, 0f); // Stop se vicino
        }

        if (spriteRenderer != null)
            spriteRenderer.flipX = (player.position.x < transform.position.x);
    }

    public override void PerformAction()
    {
        Debug.Log("Alleato cura o potenzia il giocatore!");

        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null)
        {
            // Cura il giocatore
            playerScript.UpdateHealth((int)healAmount);

            // Applica un potenziamento temporaneo alla velocità
            if (!isBuffing)
                StartCoroutine(ApplySpeedBuff(playerScript));
        }

        if (animator != null)
            animator.SetTrigger("Support");
    }

    private System.Collections.IEnumerator ApplySpeedBuff(Player playerScript)
    {
        isBuffing = true;
        float originalSpeed = playerScript.moveSpeed;
        playerScript.moveSpeed *= 1.5f;

        yield return new WaitForSeconds(buffDuration);

        playerScript.moveSpeed = originalSpeed;
        isBuffing = false;
    }
}
