using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSound : MonoBehaviour
{
    private AudioSource playerAudio;

    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerAudio.Play();
            Destroy(other.gameObject, 1f);
        }
    }
}
