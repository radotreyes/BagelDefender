using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public PlayerController player;

    // Use this for initialization
    void Start()
    {   // look for the player
        player = FindObjectOfType<PlayerController>();

        // set health values as appropriate
        maxHealth = player.maxHealth;
        currentHealth = player.currentHealth;
    }

    public void TakeDamage( float damage )
    {      
        // calculate current health within script
        currentHealth = currentHealth - damage;

        // make health to display in HUD a fraction of max health
        float scaledHealth = (currentHealth / maxHealth);

        // make sure health bar scaling doesn't go negative and set scaling
        scaledHealth = Mathf.Clamp( scaledHealth, 0, 1 );
        SetHealthBar( scaledHealth );

        // calculate **player's** current health to reflect in scene changes
        player.currentHealth = player.currentHealth - damage;        
    }

    private void SetHealthBar( float health )
    {
        gameObject.transform.localScale = new Vector3( health, transform.localScale.y, transform.localScale.z );
    }
}
