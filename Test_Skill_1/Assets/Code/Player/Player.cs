using UnityEngine;

public class Player : Character
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 300f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private bool isGrounded;

    // Controllo del terreno per permettere il salto solo da terra
    private void Update()
    {
        HandleMovement();
        HandleJump();

        if (Input.GetKeyDown(KeyCode.E))
        {
            PerformAction(); // E per raccogliere o attaccare
        }
    }

    // Movimento del player con Input
    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Vector2 moveDirection = new Vector2(moveInput, 0f);
        Move(moveDirection, moveSpeed);

        if (spriteRenderer != null)
            spriteRenderer.flipX = moveInput < 0;
    }

    // Gestione salto
    private void HandleJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce);
            if (animator != null)
                animator.SetTrigger("Jump");
        }
    }

    // Implementazione di PerformAction (es. raccogliere o attaccare)
    public override void PerformAction()
    {
        // Semplice attacco o raccolta con un raycast frontale
        Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.5f);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Collectible"))
            {
                Debug.Log("Oggetto raccolto: " + hit.collider.name);
                Destroy(hit.collider.gameObject);
            }
            else if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Attacco al nemico!");
                // Potresti aggiungere un metodo TakeDamage() nel nemico
            }
        }
    }
}
