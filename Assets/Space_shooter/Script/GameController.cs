using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject background;
    public GameObject playerExplosion;
    public GameObject player;
    public GameObject enemies;
    private PlayerControllar playerController;
    private SkinnedMeshRenderer SMR;
    public GameObject[] hazards;
    public Texture[] textures;
    public Vector3 spawnValues;
    private float spawnWait;
    public float startWait;
    public float waveWait;

    public GUIText scoreText;
    public GUIText gemText;
    public GUIText gameOverText;
    public GUIText roundText;
    public GUIText medipackText;
    public GUIText bombText;

    private int hazardCount = 10;
    private int CHANGE_SCORE = 1;
    private int CHANGE_ROUND = 2;
    private int life;
    private bool gameOver;
    private int score;
    private int gem;
    private int round;
    private int bomb;

    private void Start()
    {
        gameOver = false;
        score = 0;
        gem = 0;
        round = 1;
        life = 2;
        bomb = 1;
        spawnWait = 1.0f;

        gameOverText.text = "";
        UpdateText(CHANGE_SCORE);
        UpdateGem(0);
        UpdateText(CHANGE_ROUND);
        StartCoroutine (SpawnWaves());
        SMR = background.GetComponent<SkinnedMeshRenderer>();
        playerController = player.GetComponent<PlayerControllar>();
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);

        while (gameOver == false)
        {
            UpdateText(CHANGE_ROUND);
            UpdateLevel();
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, 4)];
                if (round <= 3)
                    hazard = hazards[Random.Range(0, 3)];
                if (Random.Range(0.0f, 0.99f) > 0.8f)
                    hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation).transform.SetParent(enemies.transform);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(spawnWait);
            AddScore(round * 10);
            round++;
        }
    }

    public void AddScore (int newScoreValue)
    {
        score += newScoreValue;
        UpdateText(CHANGE_SCORE);
        if (round >= 5)
            SMR.material.mainTexture = textures[round / 5 - 1];
    }

    private void UpdateLevel()
    {
        hazardCount = (round / 5 + 2) * 5;
        spawnWait = 1.0f * Mathf.Max(0.6f, (1 - 0.25f * round));
    }

    void UpdateText(int op)
    {
        switch (op)
        {
            case 1:
                scoreText.text = "Score: " + score;
                break;
            case 2:
                roundText.text = "Round" + round;
                break;
        }
        
    }

    public void UpdateLife(int newLife)
    {
        if (life <3 || newLife < 0)
            life += newLife;
        medipackText.text = ": " + life;
        if(life <= 0)
        {
            Instantiate(playerExplosion, player.transform.position, player.transform.rotation);
            DestroyObject(player);
            GameOver();
        }
    }

    public void UpdateGem(int newGem)
    {
        gem += newGem;
        gemText.text = "Gem: " + gem;
    }

    public bool UpdateBomb(int newBomb)
    {
        if (newBomb <0 && bomb <= 0)
            return false;
        else
        {
            Debug.Log("bomb now: " + bomb + ", add: " + newBomb);
            if (bomb < 3 || newBomb < 0)
                bomb += newBomb;
            Debug.Log("after: " + bomb);
            bombText.text = ": " + bomb;
            return true;
        }
    }

    public int Bomb()
    {
        return bomb;
    }

    public void FireFast(int time)
    {
        Timer runonce = new System.Timers.Timer(time);
        runonce.Elapsed += (s, e) => {
            playerController.fireRate = 0.25f;
        };
        runonce.AutoReset = false;

        playerController.fireRate = 0.1f;
        runonce.Start();
    }

    public void GameOver()
    {
        gameOverText.text = "GAME OVER";
        gameOver = true;
    }
}
