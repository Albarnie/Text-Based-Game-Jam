using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    PlayerController controller;
    Animator anim;
    AudioSource[] audioSources;

    float delay;
    public Gun gun;
    public Transform shot, gunPivot;
    GameManager.PlayerData playerData;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        audioSources = GetComponents<AudioSource>();
    }

    private void Start()
    {
        gun = GameManager.manager.guns[GameManager.manager.playerData.gun];
        GameManager.manager.player = this;
    }

    private void Update()
    {
        playerData = GameManager.manager.playerData;
        if (Input.GetMouseButton(1))
        {
            anim.SetBool("Aiming", true);
            gunPivot.gameObject.SetActive(true);
            gunPivot.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(controller.direction.y, controller.direction.x) * Mathf.Rad2Deg);
            gunPivot.localScale = new Vector3(transform.localScale.x, 1, 1);
            if (Input.GetMouseButtonDown(0) && !gun.automatic && Time.time > delay || Input.GetMouseButton(0) && gun.automatic && Time.time > delay)
            {
                if(playerData.ammoInClip[playerData.gun] > 0)
                {
                    delay = Time.time + (1f / gun.RPS);
                    Debug.Log(1f / gun.RPS);
                    Fire();
                    GameManager.manager.playerData.ammoInClip[playerData.gun]--;
                }
                else
                {
                    audioSources[0].clip = gun.noAmmoSound;
                    audioSources[0].pitch = Random.Range(0.8f, 1.2f);
                    audioSources[0].Play();
                }
            }
        }
        else
        {
            gunPivot.gameObject.SetActive(false);
            anim.SetBool("Aiming", false);
        }

        if(Input.GetKeyDown("r"))
        {
            delay = Time.time + gun.reloadTime;
            Reload();
        }
    }

    public void Damage (int amount)
    {
        GameManager.manager.playerData.health -= amount;
        if(GameManager.manager.playerData.health <= 0)
        {
            Die();
        }
    }

    void Reload ()
    {
        Debug.Log("reload");
        audioSources[0].clip = gun.reloadSound;
        audioSources[0].pitch = Random.Range(0.8f, 1.2f);
        audioSources[0].Play();
        GameManager.manager.playerData.ammoInClip[playerData.gun] = gun.ammoPerClip;
    }

    public void Die ()
    {

    }

    void Fire ()
    {
        audioSources[0].clip = gun.shotShound;
        audioSources[0].pitch = Random.Range(0.8f, 1.2f);
        audioSources[0].Play();

        for (int i = 0; i < gun.bulletAmount; i++)
        {
            Quaternion rotation = shot.rotation * Quaternion.Euler(0, 0, Random.Range(gun.bulletSpread, -gun.bulletSpread));
            GameObject bullet = Instantiate(gun.bulletPrefab, shot.position, rotation);
            bullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(gun.bulletVelocity, 0));
            bullet.GetComponent<Bullet>().gun = gun;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.tag)
        {
            case "Pickup":
                switch ((int)other.GetComponent<Pickup>().type)
                {
                    case 0:
                        break;
                    case 1:
                        GameManager.manager.playerData.gun = other.GetComponent<Pickup>().id;
                        gun = GameManager.manager.guns[other.GetComponent<Pickup>().id];
                        break;
                }
                break;
        }
    }
}
