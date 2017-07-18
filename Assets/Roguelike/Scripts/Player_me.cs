using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_me : MovingObject_me {

    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerGem = 1;
    public int pointsPerChest = 5;
    public float restartLevelDelay = 0.8f;

    public Text foodText;
    public Text gemText;
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip gemSound;

    public AudioClip gameOverSound;

    private Animator animator;
    private int food;
    private int gem;
    private Vector2 touchOrigin = -Vector2.one;

	// Use this for initialization
	protected override void Start () {
        animator = GetComponent<Animator>();

        food = GameManager_me.instance.playerFoodPoints;
        gem = GameManager_me.instance.playerGemPoints;

        foodText.text = "Food: " + food;
        gemText.text = "Gem: " + gem;
        
        base.Start();
	}

    private void OnDisable()
    {
        GameManager_me.instance.playerFoodPoints = food;
        GameManager_me.instance.playerGemPoints = gem;
    }

    // Update is called once per frame
    void Update () {
        if (!GameManager_me.instance.playersTurn) return;

        int horizontal = 0;
        int vertical = 0;

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
            vertical = 0;
#else

        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.touches[0];

            if(myTouch.phase == TouchPhase.Began)
            {
                touchOrigin = myTouch.position;
            }

            else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
            {
                Vector2 touchEnd = myTouch.position;
                float x = touchEnd.x - touchOrigin.x;
                float y = touchEnd.y - touchOrigin.y;
                touchOrigin.x = -1;
                if (Mathf.Abs(x) > Mathf.Abs(y))
                    horizontal = x > 0 ? 1 : -1;
                else
                    vertical = y > 0 ? 1 : -1;
            }
        }

#endif

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall_me>(horizontal, vertical);
	}


    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;
        foodText.text = "Food: " + food;

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;
        if (Move (xDir, yDir, out hit))
        {
            SoundManager_me.instance.RandomizeSfx(moveSound1, moveSound2);
        }

        CheckIfGameOver();
        GameManager_me.instance.playersTurn = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Gem")
        {
            gem += pointsPerGem;
            gemText.text = " Gem: " + gem;
            SoundManager_me.instance.PlaySingle(gemSound);
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Chest")
        {
            gem += pointsPerChest;
            gemText.text = " Gem: " + gem;
            SoundManager_me.instance.PlaySingle(gemSound);
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            foodText.text = "+" + pointsPerFood + " Food: " + food;
            SoundManager_me.instance.RandomizeSfx(eatSound1, eatSound2);
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall_me hitWall = component as Wall_me;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
    }

    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void LoseFood (int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        foodText.text = "-" + loss + " Food: " + food;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            SoundManager_me.instance.PlaySingle(gameOverSound);
            SoundManager_me.instance.musicSource.Stop();
            GameManager_me.instance.GameOver();
        }
            
    }
}
