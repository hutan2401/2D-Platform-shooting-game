using UnityEngine;

public class PickUp : MonoBehaviour
{
    private enum PickUpType
    {
        ScorePoint,
        HealthPoint,
    }
    [SerializeField] private PickUpType pickUpType;
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
        switch (pickUpType)
        {
            case PickUpType.ScorePoint:
                ScoreManager.Instance.UpdateScore();
                Debug.Log("Coin");
                break;
            case PickUpType.HealthPoint:
                PlayerHealth.Instance.HealPlayer();
                Debug.Log("health");
                break;
        }
    }
}