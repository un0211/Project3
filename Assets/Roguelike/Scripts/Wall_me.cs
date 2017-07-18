using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_me : MonoBehaviour {

    public Sprite dmgSprite;
    public int hp = 4;
    public AudioClip chopSound;

    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}

    public void DamageWall (int loss)
    {
        SoundManager_me.instance.PlaySingle(chopSound);
        spriteRenderer.sprite = dmgSprite;
        hp -= loss;
        if (hp <= 0)
            gameObject.SetActive(false);
    }
}
