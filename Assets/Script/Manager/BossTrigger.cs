using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    private bool bossMusicPlayed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !bossMusicPlayed)
        {
            bossMusicPlayed = true;
            ManagerAudioSound.Instance.PlayBossMusic();
        }

    }
}
