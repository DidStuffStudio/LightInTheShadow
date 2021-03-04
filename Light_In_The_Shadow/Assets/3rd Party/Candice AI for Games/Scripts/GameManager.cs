using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ViridaxGameStudios.AI;

public class GameManager : MonoBehaviour
{
    public Text stageText;
    public Text subText;
    public int stageNumber;
    public string stageInfo;
    public GameObject player;
    public List<Transform> spawnPositions;
    public List<GameObject> LevelEnemies;
    public GameObject levelBoss;
    public int enemyCount = 5;
    public float timeToSpawn = 1f;
    int sceneIndex = 0;
    public int score = 0;
    public int scoreToWin = 5;
    public GameObject gameInfoPanel;
    bool gameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        subText.text = stageInfo;
        stageText.text = "Stage: " + stageNumber;
        int spawnCount = spawnPositions.Count;
        StartCoroutine("SpawnEnemy");
    }
    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    IEnumerator SpawnEnemy()
    {
        Debug.Log("Spawning Enemies every " + timeToSpawn + " seconds.");
        foreach (GameObject obj in LevelEnemies)
        {
            Vector3 position = RandomisePosition();
            CandiceAIController agent = (Instantiate<GameObject>(obj, position, Quaternion.identity)).GetComponent<CandiceAIController>();
            
            yield return new WaitForSeconds(timeToSpawn);
        }
    }
    Vector3 RandomisePosition()
    {
        Vector3 position = Vector3.one;
        int index = Random.Range(0, spawnPositions.Count);
        position = spawnPositions[index].position;
        return position;
    }

    public void Restart()
    {
        SceneManager.LoadScene(sceneIndex);
    }
    // Update is called once per frame
    void Update()
    {
        if(score >= scoreToWin)
        {
            gameOver = true;
            LevelWon();
        }
    }

    public void LevelWon()
    {
        subText.text = "Congratulations, you completed the level";
        stageText.text = "Stage: " + stageNumber + " Complete!";
        gameInfoPanel.SetActive(true);
        Time.timeScale = 0f;
        StartCoroutine("NextLevel");
    }
    public IEnumerator NextLevel()
    {
        yield return new WaitForSecondsRealtime(3f);
        if(gameOver)
        {
            Time.timeScale = 1f;
            if(stageNumber >= 3)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex) + 1);
            }
            
        }
        
    }
}
