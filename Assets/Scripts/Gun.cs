using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Gun
{
    public GameObject bulletPrefab;

    public int RPS = 10;
    public int speed, damage;
    [Header("Sound Settings")]

    public AudioClip shotShound;
    public AudioClip reloadSound, noAmmoSound;
}
