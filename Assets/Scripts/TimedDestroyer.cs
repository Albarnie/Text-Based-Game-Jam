using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroyer : MonoBehaviour
{
    public float time;
    float startTime;

    private void Awake()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if(Time.time >= time+startTime)
        {
            Destroy(gameObject);
        }
    }

}
