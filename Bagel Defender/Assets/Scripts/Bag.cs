using UnityEngine;
using System.Collections;

public class Bag : MonoBehaviour
{
    private AudioSource[] _audio;
    private AudioClip _pewpew;
    private AudioClip _whack;

    // Use this for initialization
    void Start()
    {
        // play audio and wait for clip to end before destroying
        _audio = gameObject.GetComponents<AudioSource>();
        _pewpew = _audio[ 0 ].clip;
        _whack = _audio[ 1 ].clip;

        AudioSource.PlayClipAtPoint( _pewpew, new Vector3( 0, 0, 0 ), 0.5f );
    }

    public void Hit()
    {
        Destroy( gameObject );
        AudioSource.PlayClipAtPoint( _whack, new Vector3( 0, 0, 0 ), 0.5f );
    }

    public void OutOfBounds()
    {
        Destroy( gameObject );
    }
}
