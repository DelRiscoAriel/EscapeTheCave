using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staues : MonoBehaviour
{
    public Gates gate;
    Animator anim;
    Collider2D collider;

    public AudioClip statueClip;
    private AudioSource playerAudio;

    public ObjectiveText count;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        playerAudio = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            anim.SetTrigger("catch");
            collider.enabled = false;
            playerAudio.clip = statueClip;
            playerAudio.Play();
            count.collected += 1;
            Invoke("Next", 0.2f);
        }
    }

    void Next()
    {
        Destroy(gameObject);
        gate.move = true;
    }
}
