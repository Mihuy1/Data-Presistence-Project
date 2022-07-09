using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject BestScoreParent;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    private int bestScorePoints;
    private string currentPlayerSave;
    
    private bool m_GameOver = false;



    private void Awake()
    {
        LoadPoints(); 
    }

    private void OnApplicationQuit()
    {
        SavePoints();
    }


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
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        m_GameOver = true;
        GameOverText.SetActive(true);
        BestScoreParent.SetActive(true);

        if (bestScorePoints == 0)
        {
            BestScoreText.text = MenuManager.Instance.playerName + "has the best score of: " + m_Points; 
        } else if (bestScorePoints < m_Points)
        {
            BestScoreText.text = MenuManager.Instance.playerName + "has the best score of: " + m_Points;
        }else
        BestScoreText.text = currentPlayerSave + " has the best score: " + bestScorePoints; 
    }

    [System.Serializable]

    public class SaveScore
    {
        public static SaveScore Instance;

        public int M_Points;
        public string currentPlayer;
    }

    public void SavePoints()
    {
        
        SaveScore data = new SaveScore();

        if (m_Points > data.M_Points)
        {
            data.M_Points = m_Points;
            data.currentPlayer = MenuManager.Instance.playerName;

            string json = JsonUtility.ToJson(data);

            File.WriteAllText(Application.persistentDataPath + "/savepointsfile.json", json);
        }
    }

    public void LoadPoints()
    {
        string path = Application.persistentDataPath + "/savepointsfile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveScore data = JsonUtility.FromJson<SaveScore>(json);

            bestScorePoints = data.M_Points;
            currentPlayerSave = data.currentPlayer;
            Debug.Log(bestScorePoints);
        }
    }
}
