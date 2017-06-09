using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    private PlayerController _player;
    private Scene _scene;
    private bool _inGame;

    private void Start()
    {
        GetGameVars();
    }

    private void FixedUpdate()
    {
        if( _inGame )
        {   // only look for scene changes in FixedUpdate when actually in game
            Debug.Log( _player.currentHealth );
            if( _player.currentHealth <= 0 )
            {   // if the player dies, go to lose screen
                LoadLevel( 4 );
            }
        }
    }

    public void GetGameVars()
    {
        // look for the active scene
        _scene = SceneManager.GetActiveScene();
        if( _scene.name == "Game" )
        {
            // if the active scene is the game, then the game has started
            _inGame = true;
            _player = FindObjectOfType<PlayerController>();
        }
        else
        {
            _inGame = false;
        }
    }

    public void LoadLevel( int sceneIndex )
    {
        SceneManager.LoadScene( sceneIndex );
    }

    public void QuitRequest()
    {
        Debug.Log( "Quit requested" );
        Application.Quit();
    }

}
