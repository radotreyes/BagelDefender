using UnityEngine;
using System.Collections;

public class Buzzer : MonoBehaviour
{
    public Enemy parentEnemy;
    public PlayerController player;

    private AudioSource[] _audio;
    private AudioClip _shootSound;
    private AudioClip _hurtSound;
    private AudioClip _coinSound;
    private AudioClip _whackSound;

    void Start()
    {   // play audio and wait for clip to end before destroying
        player = FindObjectOfType<PlayerController>();

        _audio = gameObject.GetComponents<AudioSource>();
        _shootSound = _audio[ 0 ].clip;
        _hurtSound = _audio[ 1 ].clip;
        _coinSound = _audio[ 2 ].clip;

        AudioSource.PlayClipAtPoint( _shootSound, new Vector3( 0f, 0f, 0 ), 0.5f );
    }

    public void Hit()
    {
        parentEnemy.buzzerActive = false;
        parentEnemy.canShoot = true;
        Destroy( gameObject );
    }

    public void PutInBasket()
    {
        parentEnemy.buzzerInBasket = true;
        parentEnemy.canShoot = true;
        AudioSource.PlayClipAtPoint( _coinSound, new Vector3( 0.5f, 0.5f, 0 ), 1 );
    }

    public void MissBuzzer()
    {
        AudioSource.PlayClipAtPoint( _hurtSound, new Vector3( 0, 0, 0 ), 0.5f );
        player.health.TakeDamage( player.damage_missBuzzer );
        player.AddScore( player.score_missBuzzer );
    }

    public void OutOfBounds()
    {
        parentEnemy.buzzerActive = false;
        parentEnemy.canShoot = true;
        Destroy( gameObject );
    }
}
