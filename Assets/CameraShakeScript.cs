using UnityEngine;
using System.Collections;

public class CameraShakeScript : MonoBehaviour
{
    public float StartingShakeDistance = 0.8f;
    public float DecreasePercentage = 0.5f;
    public float ShakeSpeed = 55f;
    public int NumberOfShakes = 10;
    public Camera Cam;

    private Vector3 originalPosition;

    void Awake()
    {
        EventDispatcher.AddEventListener<CameraShakeEvent>(OnCameraShake);
    }

    public void OnCameraShake(CameraShakeEvent e)
    {
        BeginShake();
    }

    public void BeginShake()
    {
        if (Cam == null)
            Cam = Camera.main;
        StopCoroutine("ShakeRoutine");
        originalPosition = Cam.transform.position;
        StartCoroutine("ShakeRoutine");
    }

    public void StopShake()
    {
        StopCoroutine("ShakeRoutine");
        originalPosition = Cam.transform.position;
    }

    IEnumerator ShakeRoutine()
    {
        float hitTime = Time.time;
        //Vector3 originalPosition = Cam.transform.localPosition;
        int shake = NumberOfShakes;
        float shakeDistance = StartingShakeDistance;

        while (shake >= 0)
        {
            float timer = (Time.time - hitTime) * ShakeSpeed;
            Cam.transform.position = originalPosition + new Vector3(Mathf.Sin(timer) * shakeDistance, 0, -10);

            if (timer > Mathf.PI * 2)
            {
                hitTime = Time.time;
                shakeDistance *= DecreasePercentage;
                shake--;
            }

            yield return true;
        }
        Cam.transform.position = originalPosition;
    }
}