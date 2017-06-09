using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelCounter : MonoBehaviour
{
    [SerializeField]
    private List<string> _levelList;    
    private Text _level;
    private int _currentLevel;
    private string _levelName;
    private bool _changeLevel;

    [SerializeField]
    private List<int> _difficulty;
    public int difficulty;

    private float _timer;               // current time
    private float _lastTimer;           // time since level change
    private string _timeToPrint;        // time shown on screen
    public EnemySpawner enemySpawner;
    public LevelManager levelManager;

    /* GAME TIME CONVERSION IS:
     * 1-HR = 1-MIN (real time)
     * 1-MIN = 1-SEC (real time)
     */
    void Start()
    {
        _currentLevel = 0;
        _changeLevel = true;
        _timer = 420; // start timer at 420 minutes, or 7:00AM
        _lastTimer = 0;
        _level = gameObject.GetComponent<Text>();
        levelManager = FindObjectOfType<LevelManager>();
        enemySpawner = FindObjectOfType<EnemySpawner>();

        // add difficulty names
        _levelList.Add( "Opening\n" );          _difficulty.Add( 1 );        // 7:00
        _levelList.Add( "Early Morning\n" );    _difficulty.Add( 2 );        // 8:00
        _levelList.Add( "Morning Rush\n" );     _difficulty.Add( 4 );        // 9:00
        _levelList.Add( "Late Morning\n" );     _difficulty.Add( 3 );        // 10:00
        _levelList.Add( "Lunch Rush #1\n" );    _difficulty.Add( 4 );        // 11:00
        _levelList.Add( "Lunch Rush #2\n" );    _difficulty.Add( 5 );        // 12:00
        _levelList.Add( "Afternoon\n" );     _difficulty.Add( 1 );          // end game
    }

    // Update is called once per frame
    void Update()
    {
        GetTime();
        if( _changeLevel )
        {
            GetNextLevel();
            StartCoroutine( FlashText() );
            
        }
        if( _levelName.Equals( "Afternoon\n" ) )
        {
            Invoke( "LoadWinScreen", 2 );
        }

        _level.text = _levelName + _timeToPrint;

    }

    void GetTime()
    {
        _timer += Time.deltaTime;
        _lastTimer += Time.deltaTime;

        int inGameHours = Mathf.FloorToInt( _timer / 60f );
        int inGameMinutes = Mathf.FloorToInt( _timer - inGameHours * 60 );
        string niceTime = string.Format( "{0:0}:{1:00}", inGameHours, inGameMinutes );

        _timeToPrint = niceTime;

        if( _lastTimer >= 60 )      // change levels if 60 in-game minutes have passed
        {
            _changeLevel = true;
        }
    }


    void GetNextLevel()
    {
        _levelName = _levelList[ _currentLevel ];
        difficulty = _difficulty[ _currentLevel ];
        enemySpawner.difficulty = difficulty;
        _currentLevel++;

        // reset time since level change and disable level changing
        _lastTimer = 0;
        _changeLevel = false;
    }

    IEnumerator FlashText()
    {
        // turn text to another color to make sure it's noticeable
        _level.color = Color.green;
        yield return new WaitForSeconds( 1f );
        _level.color = Color.white;
    }

    void LoadWinScreen()
    {
        levelManager.LoadLevel( 3 );
    }
}
