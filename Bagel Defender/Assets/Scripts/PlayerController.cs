using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    // score etc.
    public int score;                  
    public int score_catchBuzzer        { get; set; }
    public int score_giveOrder          { get; set; }
    public int score_missBuzzer         { get; set; }
    public int score_wrongOrder         { get; set; }

    // game flow properties
    public HealthBar health             { get; set; }
    public float currentHealth          { get; set; }
    public float maxHealth              { get; set; }
    public float damage_missBuzzer      { get; set; }
    public float damage_wrongOrder      { get; set; }
    public float damage_timeOut         { get; set; }

    // movement properties
    public float moveSpeed;
    private float _hMove;
    private Rigidbody2D _rb2d;

    // projectile properties
    public GameObject projectilePrefab;
    public float projectileMoveSpeed;
    public float projectileCooldown;
    public bool canFire;             
    private bool _requestShoot;

    // camera properties
    private float _leftBound;
    private float _rightBound;
    private float _topBound;
    private float _bottomBound;
    public float borderPadding          { get; set; }

    private void OnTriggerEnter2D( Collider2D other )
    {
        Buzzer otherObject = other.gameObject.GetComponent<Buzzer>();
        if( otherObject != null )
        {
            Destroy( other.gameObject );
            otherObject.Hit();
            otherObject.PutInBasket();
            otherObject.parentEnemy.UpdateText();

            AddScore( score_catchBuzzer );
        }
    }

    // Use this for initialization
    void Awake()
    {
        // player has 100 max health at the start
        health = FindObjectOfType<HealthBar>();
        maxHealth = 100;
        currentHealth = maxHealth;

        // set damage values
        damage_missBuzzer = 4;
        damage_wrongOrder = 2;
        damage_timeOut = 2;

        // score is 0 at the start and set score values
        score = 0;
        score_catchBuzzer = 50;
        score_giveOrder = 100;
        score_missBuzzer = -25;
        score_wrongOrder = -100;

        // can fire at the start
        canFire = true;

        // initialize children
        _rb2d = gameObject.GetComponent<Rigidbody2D>();
        _rb2d.isKinematic = true;

        // distance between object and camera
        float distance = transform.position.z - Camera.main.transform.position.z;

        // get screen bounds
        Vector3 getLeftBound = Camera.main.ViewportToWorldPoint( new Vector3( 0, 0, distance ) );
        Vector3 getRightBound = Camera.main.ViewportToWorldPoint( new Vector3( 1, 0, distance ) );
        Vector3 getTopBound = Camera.main.ViewportToWorldPoint( new Vector3( 0, 1, distance ) );
        Vector3 getBottomBound = Camera.main.ViewportToWorldPoint( new Vector3( 0, 0, distance ) );

        // set horizontal boundaries
        _leftBound = getLeftBound.x + borderPadding;
        _rightBound = getRightBound.x - borderPadding;

        // set vertical boundaries
        _topBound = getTopBound.y;
        _bottomBound = getBottomBound.y;
    }

    // INPUT/OUTPUT
    private void Update()
    {
        _hMove = Input.GetAxisRaw( "Horizontal" );
        RequestShoot();
    }
    private void FixedUpdate()
    {
        // move player        
        _rb2d.velocity = new Vector2( _hMove * moveSpeed * Time.deltaTime, _rb2d.velocity.y );

        // restrict player movement
        float boundsX = Mathf.Clamp( transform.position.x, _leftBound, _rightBound );
        transform.position = new Vector2( boundsX, transform.position.y );

        // process player inputs
        StartCoroutine( ProcessShoot() );
    }

    // SHOOT
    private void RequestShoot()
    {
        if( Input.GetKey( KeyCode.Space ) )
        {
            _requestShoot = true;
        }
        else
        {
            _requestShoot = false;
        }
    }
    private IEnumerator ProcessShoot()
    {
        if( _requestShoot && canFire )
        {
            GameObject projectileInstance = Instantiate( projectilePrefab, transform.position, Quaternion.identity ) as GameObject;
            projectileInstance.GetComponent<Rigidbody2D>().velocity = new Vector2( 0, 1000 * Time.deltaTime );
            canFire = false;
            yield return new WaitForSeconds( projectileCooldown );
            canFire = true;
        }
    }

    // ADD SCORE
    public void AddScore( int score )
    {
        this.score += score;
    }
}


