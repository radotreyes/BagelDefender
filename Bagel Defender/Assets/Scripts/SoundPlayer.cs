using UnityEngine;
using System.Collections;

public class SoundPlayer : MonoBehaviour
{
    static SoundPlayer speaker = null;

    // audio management

    AudioSource audios;
    AudioSource peacefulMusic;
    AudioSource deathMetalMusic;

    void Start()
    {
        audios = GetComponent<AudioSource>();

        // singleton
        if( speaker != null && speaker != this )
        {
            Destroy( gameObject );
            print( "Duplicate music player self-destructing!" );
        }
        else
        {
            speaker = this;
            GameObject.DontDestroyOnLoad( gameObject );
        }
    }

    public void Play()
    {
        audios.Play();
    }
}
