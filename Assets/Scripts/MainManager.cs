using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public static int BestPoints;
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public Text MaxScoreText;
    public Text ScoreText;
    public GameObject GameOverText;
    public string Name;
    public string BetterName;
    public int BetterPoints = 0;
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
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
        SetMaxPoints(BestPoints);
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
    public void SetName(){
        MenuScript pesos = new MenuScript();
        Name = pesos.getName();
    }
    void SetMaxPoints(int Mpoint){
        SetName();
        SaveData ejt = new SaveData();
        ejt.LoadInfo();
        BetterName = ejt.SaveName;
        BetterPoints = ejt.SaveScore;
        MaxScoreText.text = "Best Score Ever: " + ejt.SaveScore +"  Best so far: " + Mpoint;
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        if(m_Points > BetterPoints){
            BetterPoints = m_Points;
            BetterName = Name;
            BestPoints = m_Points;
        }else if(BestPoints < m_Points){
            BestPoints = m_Points;
            }
        SaveData jumonji = new SaveData();
        jumonji.SaveName = BetterName;
        jumonji.SaveScore = BetterPoints;
        jumonji.SaveInfo();
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
    [System.Serializable]
    class SaveData{
        public string SaveName;
        public int SaveScore;
        public void SaveInfo(){
            SaveData data = new SaveData();
            data.SaveName = SaveName;
            data.SaveScore = SaveScore;
            string json = JsonUtility.ToJson(data);
File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        }

        public void LoadInfo(){
            string path = Application.persistentDataPath + "/savefile.json";
    if (File.Exists(path))
    {
        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

         SaveName = data.SaveName;
         SaveScore = data.SaveScore;
        }
    }
}}
