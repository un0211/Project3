using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

    public GameObject explosion;
    public int scoreValue = 0;

    private GameController gameController;

    public void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy"))
        {
            return;
        }
        
        if (other.tag == "Player")
        {
            if (tag == "Diamond")
            {
                gameController.UpdateGem(5);
            } else if (tag == "Star")
            {
                gameController.UpdateGem(1);
            } else if (tag == "MediPack")
            {
                gameController.UpdateLife(1);
            } else if (tag == "Arrows")
            {
                Debug.Log("Eat Arrow");
                gameController.FireFast(5000);
            } else if (tag == "Bomb")
            {
                gameController.UpdateBomb(1);
            } else
            {
                gameController.UpdateLife(-1);
            }
            if (explosion != null)
                Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if (!(tag == "Diamond" || tag == "Star" || tag == "MediPack" || tag == "Arrows" || tag == "Bomb"))
        {
            if (explosion != null)
                Instantiate(explosion, transform.position, transform.rotation);
            
            gameController.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }
}
