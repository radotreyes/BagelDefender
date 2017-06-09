using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private Vector3 _originalPos;
    public static CameraShake _instance;

    public static float _shakeDuration = 0.3f;
    public static float _shakeAmount = 0.15f;

    void Awake()
    {
        _originalPos = transform.localPosition;

        _instance = this;
    }

    public static void Shake()
    {
        _instance.StopAllCoroutines();
        _instance.StartCoroutine( _instance.cShake( _shakeDuration, _shakeAmount ) );
    }

    public IEnumerator cShake( float duration, float amount )
    {
        float endTime = Time.time + duration;

        while( Time.time < endTime )
        {
            transform.localPosition = _originalPos + Random.insideUnitSphere * amount;

            duration -= Time.deltaTime;

            yield return null;
        }

        transform.localPosition = _originalPos;
    }
}