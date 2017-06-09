using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{
    static MusicPlayer instance = null;

    // audio management

    AudioSource[] audios;
    AudioSource menuMusic;
    AudioSource gameMusic;

    void Start()
    {
        audios = GetComponents<AudioSource>();
        menuMusic = audios[ 0 ];
        gameMusic = audios[ 1 ];

        // singleton
        if( instance != null && instance != this )
        {
            Destroy( gameObject );
            print( "Duplicate music player self-destructing!" );
        }
        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad( gameObject );

        }

        // play audio clip


    }
    private void Update()
    {
        if( SceneManager.GetActiveScene().name.Equals( "Start Menu" ) )
        {
            if( !menuMusic.isPlaying )
            {
                foreach( AudioSource audio in audios )
                {
                    if( audio.isPlaying )
                    {
                        audio.Stop();
                    }
                }

                menuMusic.Play();
            }          
        }
        else
            if( !gameMusic.isPlaying )
        {
            foreach( AudioSource audio in audios )
            {
                if( audio.isPlaying )
                {
                    audio.Stop();
                }
            }

            gameMusic.Play();
        }
    }

}
