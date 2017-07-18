using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_me : MonoBehaviour {

    public float levelStartDelay = .5f;
    public float turnDelay = .05f;
    public int playerFoodPoints = 100;
    public int playerGemPoints = 0;
    public int playerScorePoints = 0;
    public static GameManager_me instance = null;
    [HideInInspector] public bool playersTurn = true;

    private Text levelText;
    private Text scoreText;
    private GameObject levelImage;
    public BoardManager_me boardScript;
    private int level = 1;
    private int score = 0;
    private List<Enemy_me> enemies;
    private bool enemiesMoving;
    private bool doingSetup = true;

    

	// Use this for initialization
	void Awake () {

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy_me>();
        boardScript = GetComponent<BoardManager_me>();
        InitGame();
	}

    void OnLevelWasLoaded(int index)
    {
        score += (level * 10);
        
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        scoreText.text = "Score: " + score;
        level++;

        InitGame();
    }

    void InitGame()
    {
        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Stage " + level;

        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        scoreText.text = "Score: " + score;

        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear();
        boardScript.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }
	
    public void GameOver()
    {
        levelText.text = "GAME OVER!";
        levelImage.SetActive(true);
        enabled = false;
    }

	// Update is called once per frame
	void Update () {
        if (playersTurn || enemiesMoving || doingSetup)
            return;

        StartCoroutine(MoveEnemies());
	}

    public void AddEnemyToList(Enemy_me script)
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }
        for (int i=0; i<enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }
}
