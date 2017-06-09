using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour
{
    private Text _score;
    public PlayerController player;

    // Use this for initialization
    void Start()
    {
        _score = gameObject.GetComponent<Text>();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        _score.text = player.score.ToString();
    }
}
