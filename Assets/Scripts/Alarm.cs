using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : Electronic
{
    [Header("Alarm Settings")]
    public int range;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetOff ()
    {
        audioSource.Play();
        GameManager.manager.enemyManager.Alert(transform.position, range, range * 2);
    }

    public override void Disable()
    {
        base.Disable();
        audioSource.Stop();
    }

}
