using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    public GameObject shot;
    public Transform[] shotSpawn;
    public float fireRate;
    public float delay;

	// Use this for initialization
	void Start () {
        InvokeRepeating("Fire", delay, fireRate);
	}
	
	void Fire () {
        for (int i = 0; i < shotSpawn.Length; i++)
            Instantiate(shot, shotSpawn[i].position, shotSpawn[i].rotation);
        
        GetComponent<AudioSource>().Play();
	}
}
