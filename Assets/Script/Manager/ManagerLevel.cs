using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerLevel : MonoBehaviour
{
    [SerializeField] private GameObject victoryMenu;
    [SerializeField] private float delayBeforeTranstion = 3f;

    private bool hasWon =false;
    private void Start()
    {
        if (victoryMenu != null)
        {
            victoryMenu.SetActive(false);
        }
    }

    public void OnBossDefeated()
    {
        if (hasWon) return;
        hasWon = true;

        // Display victory menu
        if (victoryMenu != null)
        {
            victoryMenu.SetActive(true);
        }

        // Trigger GameManager's stage completion
        StartCoroutine(HandleBossDefeat());
    }

    private IEnumerator HandleBossDefeat()
    {
        yield return new WaitForSeconds(delayBeforeTranstion);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnBossDefeated();
        }
    }

}
