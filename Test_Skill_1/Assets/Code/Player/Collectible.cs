using UnityEngine;

public class Collectible : MonoBehaviour
{
    public CollectibleData data;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                ApplyEffect(player);
                Debug.Log($"Raccolto: {data.collectibleName} (+{data.pointValue} punti)");
            }

            Destroy(gameObject);
        }
    }

    private void ApplyEffect(Player player)
    {
        switch (data.effect)
        {
            case CollectibleEffect.Heal:
                player.UpdateHealth((int)data.effectStrength);
                break;
            case CollectibleEffect.Buff:
                // Aggiungi qui logica per buff personalizzato
                break;
            case CollectibleEffect.ExtraPoints:
                // player.AddScore(data.pointValue); // Se hai un sistema di punteggio
                break;
            case CollectibleEffect.SpeedBoost:
                // esempio: aumenta la velocità temporaneamente
                StartCoroutine(SpeedBoost(player));
                break;
        }
    }

    private System.Collections.IEnumerator SpeedBoost(Player player)
    {
        float originalSpeed = player.moveSpeed;
        player.moveSpeed *= data.effectStrength;
        yield return new WaitForSeconds(5f);
        player.moveSpeed = originalSpeed;
    }
}
