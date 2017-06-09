using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject placeholderPrefab;

    // formation bounds
    public float width = 10f;
    public float height = 10f;
    public float formationRightEdge;
    public float formationLeftEdge;

    // number of enemies in formation
    public int enemyCount;
    public bool allEnemiesDead;
    public List<GameObject> enemyPositions;

    // spawning enemies and difficulty
    public float spawnTimeMin;
    public float spawnTimeMax;
    public int difficulty;

    // movement properties
    public float moveSpeed;
    private Vector3 _hDirection;

    // movement bounds
    private float _leftBound;
    private float _rightBound;
    public float borderPadding;

    // Use this for initialization
    void Start()
    {
        // set direction to RIGHT at first
        _hDirection = Vector3.right;

        // get distance between object and camera
        float distance = transform.position.z - Camera.main.transform.position.z;

        // get screen bounds
        Vector3 getLeftBound = Camera.main.ViewportToWorldPoint( new Vector3( 0, 0, distance ) );
        Vector3 getRightBound = Camera.main.ViewportToWorldPoint( new Vector3( 1, 0, distance ) );

        // set boundaries
        _leftBound = getLeftBound.x + borderPadding;
        _rightBound = getRightBound.x - borderPadding;

        // make sure enemies can spawn at first load
        allEnemiesDead = false;
        StartCoroutine( SpawnEnemies() );
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube( transform.position, new Vector3( width, height ) );
    }

    void Update()
    {
        // get formation edges based on leftmost and rightmost enemy position
        GetFormationBounds();

        // restrict formation
        float boundsX = Mathf.Clamp( transform.position.x, _leftBound, _rightBound );
        if( formationLeftEdge <= _leftBound )
        { // if formation hits the edges
            _hDirection = Vector3.right; // reverse direction
        }
        if( formationRightEdge >= _rightBound )
        { // if formation hits the edges
            _hDirection = Vector3.left; // reverse direction
        }

        // move formation
        transform.Translate( _hDirection * Time.deltaTime * moveSpeed );
        Mathf.Clamp( transform.position.x, _leftBound, _rightBound );

        // check if all are dead and spawn until full if they are
        if( allEnemiesDead )
        {
            StartCoroutine( SpawnUntilFull() );
        }
    }

    // DIFFICULTY-DEPENDENT
    private IEnumerator SpawnEnemies()
    {
        foreach( Transform child in transform )
        {
            yield return new WaitForSeconds( Random.Range( spawnTimeMin - (0.1f * (difficulty - 1)), spawnTimeMax - (0.2f * (difficulty - 1)) ) );
            GameObject enemy = Instantiate( enemyPrefab, child.transform.position, Quaternion.identity ) as GameObject;
            enemy.transform.parent = child;

            child.transform.position = RandomizePosition( child.transform.position );

            enemyPositions.Add( enemy );
        }
    }

    private IEnumerator SpawnUntilFull()
    {
        // make sure enemies are not marked as dead when this is called
        if( allEnemiesDead )
        {
            allEnemiesDead = false;
        }

        Transform freePosition = NextFreePosition();
        if( freePosition )
        {
            GameObject placeholder = Instantiate( placeholderPrefab, freePosition.transform.position, Quaternion.identity ) as GameObject;
            placeholder.transform.parent = freePosition;
            yield return new WaitForSeconds( Random.Range( spawnTimeMin - (0.1f * (difficulty - 1)), spawnTimeMax - (0.2f * (difficulty - 1)) ) );
            Destroy( placeholder );
            GameObject enemy = Instantiate( enemyPrefab, freePosition.transform.position, Quaternion.identity ) as GameObject;
            enemy.transform.parent = freePosition;
            freePosition.transform.position = RandomizePosition( freePosition.transform.position );

            enemyPositions.Add( enemy );

        }
        if( NextFreePosition() )
        {
            yield return StartCoroutine( SpawnUntilFull() );
        }
    }

    public bool EnemiesAliveCheck()
    {
        Debug.Log( enemyPositions.Count );
        if( enemyPositions.Count <= difficulty - 1 )
        {
            return true;
        }
        return false;
    }

    // DIFFICULTY-DEPENDENT
    public Vector3 RandomizePosition( Vector3 position )
    {   // randomize enemy spawn positions by some X, Y offset

        float offset_x = 0.3f * difficulty;
        float offset_y = 0.1f * difficulty;
        Vector3 posRandomizer = new Vector3( Random.Range( -offset_x, offset_x ), Random.Range( -offset_y, offset_y ), 0 );
        Vector3 randomPosition = position + posRandomizer;

        return randomPosition;
    }

    public Transform NextFreePosition()
    {
        foreach( Transform childPosition in transform )
        {
            if( childPosition.childCount == 0 )
            {
                return childPosition;
            }
        }

        return null;
    }

    private void ResetFormationBounds()
    {
        formationRightEdge = 0;
        formationLeftEdge = 0;
    }

    private void GetFormationBounds()
    {   // dynamically resize the formation bounds
        //  get the rightmost enemy's position
        float rightEdge = 0;
        foreach( Transform child in transform )
        {
            if( child.childCount != 0 && child.transform.position.x > rightEdge )
            { // ONLY calculate this if the child position is populated
                rightEdge = child.transform.position.x;
            }
        }
        // set the rightmost bound to rightmost enemy position
        formationRightEdge = rightEdge;

        // repeat with left enemy
        float leftEdge = 0;
        foreach( Transform child in transform )
        {
            if( child.childCount != 0 && child.transform.position.x < leftEdge )
            {
                leftEdge = child.transform.position.x;
            }

        }
        // set leftmost bound to leftmost enemy position
        formationLeftEdge = leftEdge;
    }
}
