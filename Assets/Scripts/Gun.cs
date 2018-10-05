using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Gun
{
    public string name = "Gun";
    public GameObject bulletPrefab;

    public int RPS = 10;
    public int bulletVelocity = 25000, bulletDamage = 30, bulletAmount = 1, bulletSpread = 10, ammoPerClip = 16;
    public float reloadTime = 2;
    public bool automatic;

    [Header("Sound Settings")]

    public AudioClip shotShound;
    public AudioClip reloadSound, noAmmoSound;
}
