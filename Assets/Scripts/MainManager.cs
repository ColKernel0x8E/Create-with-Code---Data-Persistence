using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;
    public GameObject NewHighScoreText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOverMenu = false;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        if (ScoreManager.Instance != null )
        {
            if (ScoreManager.Instance.highScore > 0)
            {
                HighScoreText.text = $"Best Score : {ScoreManager.Instance.highScoreName} : {ScoreManager.Instance.highScore}";
            }
        }       
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOverMenu)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(1);
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        if (ScoreManager.Instance != null)
        {
            if (m_Points > ScoreManager.Instance.highScore)
            {
                NewHighScoreText.SetActive(true);
            }
        }
        else
        {
            m_GameOverMenu = true;
            GameOverText.SetActive(true);
        }
    }

    public void SubmitHighScore()
    {
        TMP_InputField nameField = NewHighScoreText.GetComponentInChildren< TMP_InputField>(true);
        string highScoreName;

        if (nameField != null)
        {
            highScoreName = nameField.text;
        }
        else
        {
            highScoreName = "";
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.highScore = m_Points;
            ScoreManager.Instance.highScoreName = highScoreName;
            HighScoreText.text = $"Best Score : {ScoreManager.Instance.highScoreName} : {ScoreManager.Instance.highScore}";
        }

        NewHighScoreText.SetActive(false);
        m_GameOverMenu = true;
        GameOverText.SetActive(true);
    }
}
