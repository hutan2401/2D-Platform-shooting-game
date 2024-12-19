using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjecTileCurve : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 3f;
    [SerializeField] private GameObject splatterPrefab;
    [SerializeField] private bool isRotatingProjectile = false;
    [SerializeField] private int damageEnemy = 1;
    [SerializeField] private string hitSoundName = "";
    [SerializeField] private bool playHitSound = true;
    private void Start()
    {
      
        Vector3 playerPos = PlayerControls.Instance.transform.position ;
        StartCoroutine(ProjectileCurveRoutine(transform.position, playerPos));
    }

    private IEnumerator ProjectileCurveRoutine(Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);

            Vector3 nextPosition = Vector2.Lerp(startPosition, endPosition, linearT) + new Vector2(0f, height);
            if (isRotatingProjectile == true)
            {

                // Calculate direction vector
                Vector3 direction = nextPosition - transform.position;

                // Apply rotation
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);

                // Move the projectile
                
            }
            transform.position = nextPosition;
            yield return null;
        }
        if (splatterPrefab != null)
        {
            Instantiate(splatterPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
        if (!collision.isTrigger &  player)
        {
            player.TakeDamage(damageEnemy, transform);         
            if (playHitSound && !string.IsNullOrEmpty(hitSoundName))
            {
                ManagerAudioSound.Instance.PlayHitSound(hitSoundName);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            if (playHitSound && !string.IsNullOrEmpty(hitSoundName))
            {
                ManagerAudioSound.Instance.PlayHitSound(hitSoundName);
            }
           // Instantiate(splatterPrefab,transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    
}
