using UnityEngine;
using System.Collections;

public class ProjectileWall : MonoBehaviour
{
    private void OnTriggerEnter2D( Collider2D other )
    {
        if( other.CompareTag( "Buzzer" ) )
        {            
            CameraShake.Shake();
            other.gameObject.GetComponent<Buzzer>().Hit();
            other.gameObject.GetComponent<Buzzer>().MissBuzzer();

        }
        else if( other.CompareTag( "Bag" ) )
        {
            other.gameObject.GetComponent<Bag>().OutOfBounds();
        }
        else
        {
            Destroy( other.gameObject );
        }
    }
}
