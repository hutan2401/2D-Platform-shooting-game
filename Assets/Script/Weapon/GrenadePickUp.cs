using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePickUp : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Throwbomb grenade = collision.gameObject.GetComponent<Throwbomb>(); 
            if (grenade)
            {
                grenade.AddAGrenade();
                Destroy(gameObject);
            }
            ManagerAudioSound.Instance.PlayHitSound("ObtainSoundSFX");
        }
    }
}
