using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartMenu : MonoBehaviour
{
    public GameObject HighScoreText;

    void Start()
    {
        if (ScoreManager.Instance != null)
        {
            if (ScoreManager.Instance.highScore > 0)
            {
                HighScoreText.SetActive(true);
                HighScoreText.GetComponent<TMP_Text>().text = $"Best Score : {ScoreManager.Instance.highScoreName} : {ScoreManager.Instance.highScore}";
            }
        }
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}
