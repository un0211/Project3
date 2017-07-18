using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader_me : MonoBehaviour
{

    public GameObject gameManager;

    void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        if (GameManager_me.instance == null)
            Instantiate(gameManager);
    }
}
