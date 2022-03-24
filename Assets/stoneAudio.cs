using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stoneAudio : MonoBehaviour
{

     public AudioSource mAudio;
     public float timer = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
          mAudio = GetComponent<AudioSource>();
          mAudio.volume = 0.2f;

    }

    // Update is called once per frame
    void Update()
    {
          timer -= Time.deltaTime;
    }

     private void OnCollisionEnter(Collision collision)
     {
          if(timer < 0)
               mAudio.Play();
          timer = 1.0f;
     }
}
