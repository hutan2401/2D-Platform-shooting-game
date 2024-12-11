using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerLevel : MonoBehaviour
{
    [SerializeField] private GameObject victoryMenu;
    [SerializeField] private string nextScene;
    [SerializeField] private float delayBeforeTranstion = 3f;

    private bool hasWon =false;
    private void Start()
    {
        if (victoryMenu != null)
        {
            victoryMenu.SetActive(false);

        }
    }
        // Update is called once per frame
    public void OnbossDefeated()
    {
        if (hasWon) return;
        hasWon = true;
        victoryMenu.SetActive(true);
        StartCoroutine(TransitionNextScene());
    }

    private IEnumerator TransitionNextScene()
    {

        yield return new WaitForSeconds(delayBeforeTranstion);
        if (!string.IsNullOrEmpty(nextScene))
        {
            SceneManager.LoadScene(nextScene);
        }
    }

}
