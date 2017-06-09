using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    // declare parent formation and player controller
    public EnemySpawner enemySpawner;
    public PlayerController player;

    // display text
    public Text text;

    // projectile properties
    private float _timeBetweenShots;
    public float minShotTime;
    public float maxShotTime;
    private float _shotTime;
    public GameObject projectilePrefab;
    public float projectileMoveSpeed;

    // buzzer states
    public bool hasFood;
    public bool buzzerInBasket;
    public bool canShoot;
    public bool buzzerActive;

    // sprites
    private SpriteRenderer _spriteRenderer;
    public Sprite[] hitSprite;

    // spawn text
    private List<string> _customerSpawnText = new List<string>( new string[]
    { "a Cali Fresh",
      "a black coffee with room",
      "a caffe frio",
      "a bEYAH-KLOH",
      "... I forgot lol",
      "a tall vanilla latte",
      "a hot coco-ah",
      "an apple muffin",
      "a whole wheat bagel",
      "a chocolate muffin"
   });
    // exit text
    private string[] _customerExitText = { "About time.", "Thanks!", "..." };

    private void Start()
    {
        // declare parent and player
        enemySpawner = gameObject.GetComponentInParent<EnemySpawner>();
        player = FindObjectOfType<PlayerController>();

        // render random text
        text = gameObject.GetComponentInChildren<Text>();
        text.text = "I ordered " + _customerSpawnText[ ( int )Random.Range( 0, _customerSpawnText.Count ) ];
        Debug.Log( _customerSpawnText.Count  );

        // render starting sprite
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = hitSprite[ 0 ];

        // customer gives buzzer soon after they spawn
        StartCoroutine( WaitForInBounds() );
        StartCoroutine( GiveBuzzer() );

        // buzzer isn't in basket at start
        buzzerInBasket = false;
    }

    // OUTPUT
    private void FixedUpdate()
    {
        if( !buzzerInBasket )
        {
            StartCoroutine( GiveBuzzer() );
        }
    }

    private void OnTriggerEnter2D( Collider2D other )
    {
        Bag otherObject = other.gameObject.GetComponent<Bag>();
        if( otherObject != null )
        {
            if( !buzzerInBasket )
            {
                // customer complains
                StartCoroutine( WrongOrder( otherObject ) );

            }
            else if( buzzerInBasket )
            {
                // customer takes their order and leaves
                TakeFood( otherObject );
            }
            else if( hasFood )
            {
                otherObject.Hit();
                text.text = "Okay okay I'm leaving jeez";
            }
        }
    }

    private IEnumerator WaitForInBounds()
    {
        canShoot = false;
        while( transform.position.x > enemySpawner.formationRightEdge || transform.position.x < enemySpawner.formationLeftEdge )
        {
            yield return null;
        }

        canShoot = true;
    }

    private IEnumerator GiveBuzzer()
    {
        if( canShoot && !buzzerInBasket )
        {// if buzzer is NOT in basket

            // set buzzer as active so shots don't keep repeating
            canShoot = false;

            // get wait time between shots
            _shotTime = Random.Range( minShotTime, maxShotTime );
            yield return new WaitForSeconds( _shotTime );

            // instantiate new buzzer shot
            GameObject projectile = Instantiate( projectilePrefab, transform.position, Quaternion.identity ) as GameObject;
            yield return new WaitForFixedUpdate();
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector2( 0, -projectileMoveSpeed * Time.deltaTime );
            projectile.GetComponent<Buzzer>().parentEnemy = gameObject.GetComponent<Enemy>();
            buzzerActive = true;
        }
    }

    public void UpdateText()
    {
        // placeholder text to indicate waiting for food
        text.text = "Waiting for food...";
        StartCoroutine( Complain() );
    }

    public IEnumerator WrongOrder( Bag bag )
    {
        // indicate customer has been hit
        bag.Hit();
        player.AddScore( player.score_wrongOrder );
        player.health.TakeDamage( player.damage_wrongOrder );
        text.text = "Bitch this isn't my order";
        CameraShake.Shake();
        _spriteRenderer.sprite = hitSprite[ 1 ];

        // wait for next shot before returning to normal color/text
        yield return new WaitForSeconds( 1f );
        _spriteRenderer.sprite = hitSprite[ 0 ];
    }

    public IEnumerator Complain()
    {
        yield return new WaitForSeconds( 10f );
        text.text = "What's taking so long?";
        CameraShake.Shake();
        _spriteRenderer.sprite = hitSprite[ 1 ];

        player.AddScore( player.score_wrongOrder );
        player.health.TakeDamage( player.damage_timeOut );

        // wait for next shot before returning to normal color/text
        yield return new WaitForSeconds( 1f );
        _spriteRenderer.sprite = hitSprite[ 0 ];
    }

    public void TakeFood( Bag bag )
    {
        // indicate that the customer has their food
        hasFood = true;
        text.text = _customerExitText[ ( int )Random.Range( 0, 3 ) ];

        // process hit
        bag.Hit();
        player.AddScore( player.score_giveOrder );

        // feedback
        Destroy( gameObject, 0.5f );
        _spriteRenderer.sprite = hitSprite[ 2 ];

        // remove enemy from enemy list
        enemySpawner.enemyPositions.RemoveAt( enemySpawner.enemyPositions.IndexOf( gameObject ) );
        enemySpawner.allEnemiesDead = enemySpawner.EnemiesAliveCheck();
    }
}
