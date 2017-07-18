using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_me : MovingObject_me {

    public int playerDemage;

    private Animator animator;
    private Transform target;
    private bool skipMove;
    public AudioClip enemyAttack;
    
	protected override void Start () {
        GameManager_me.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
	}

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }
        base.AttemptMove<T>(xDir, yDir);

        skipMove = true;
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            yDir = target.position.y > transform.position.y ? 1 : -1;
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Player_me>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player_me hitPlayer = component as Player_me;

        animator.SetTrigger("enemyAttack");

        hitPlayer.LoseFood(playerDemage);

        SoundManager_me.instance.PlaySingle(enemyAttack);
    }
}
