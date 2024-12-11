using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private LootItem lootItem;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            DetectPickupType();
        }
    }
    private void DetectPickupType()
    {
        switch (lootItem.GetPickUpType())
        {
            case LootItem.PickUpType.ScorePoint:
                ScoreManager.Instance.UpdateScore(lootItem.score); // Use score from LootItem
                break;

            case LootItem.PickUpType.HealthGlobe:
                PlayerHealth.Instance.HealPlayer(lootItem.healpoint); // Use heal points from LootItem
                break;

            default:
                Debug.LogWarning("Unknown pickup type!");
                break;
        }
    }
}