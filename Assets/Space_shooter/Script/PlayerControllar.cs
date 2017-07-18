using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerControllar : MonoBehaviour {
    private Rigidbody rb;
    private float speed;
    public float tilt;
    public Boundary boundary;
    //public GameObject backBoundary;
    public GameObject bombExplosion;
    private GameController gameController;

    public GameObject shot;
    public Transform shotSpwan;
    public float fireRate;
    private float nextFire;
    private Vector3 bombStart = new Vector3(38, 71, 0);
    private Vector3 bombEnd = new Vector3(181, 196, 0);

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
    }

    private void Update()
    {

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
        speed = 5f;
#else
        speed = 0.45f;
#endif

        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            if (CheckBomb(Input.mousePosition) && gameController.Bomb()>0)
            {
                gameController.UpdateBomb(-1);
                Instantiate(bombExplosion, new Vector3(0, 0, 6), GetComponent<Transform>().rotation);
                
                foreach (Transform t in gameController.distruction.transform)
                {
                    Destroy(t.gameObject);
                    gameController.AddScore(20);
                }
                /*backBoundary.transform.localPosition = GetComponent<Transform>().position;
                backBoundary.transform.localScale = new Vector3(2, 2, 2);

                for (int i=0; i<10; i++)
                {
                    backBoundary.transform.localPosition = GetComponent<Transform>().position;
                    backBoundary.transform.localScale = new Vector3(2, 2, 2);
                }
                
                backBoundary.transform.localPosition = new Vector3(0, 0, 6);
                backBoundary.transform.localScale = new Vector3(14, 1, 20);
                */
            }
            else
            {
                Instantiate(shot, shotSpwan.position, shotSpwan.rotation);
                GetComponent<AudioSource>().Play();
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement;

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

#else
        float pointer_x = Input.GetAxis("Mouse X");
        float pointer_y = Input.GetAxis("Mouse Y");
        if (Input.touchCount > 0)
        {
            pointer_x = Input.touches[0].deltaPosition.x;
            pointer_y = Input.touches[0].deltaPosition.y;
        }
        
        movement = new Vector3(pointer_x, 0.0f, pointer_y);
        
#endif

        rb.velocity = movement * speed;

        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }

    public bool CheckBomb(Vector3 mouse)
    {
        if (bombStart.x < mouse.x && mouse.x < bombEnd.x && bombStart.y < mouse.y && mouse.y < bombEnd.y)
            return true;
        else
            return false;
    }
}
