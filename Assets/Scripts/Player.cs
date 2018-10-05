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

    private void Awake()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        audioSources = GetComponents<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            anim.SetBool("Aiming", true);
            gunPivot.gameObject.SetActive(true);
            gunPivot.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(controller.direction.y, controller.direction.x) * Mathf.Rad2Deg);
            gunPivot.localScale = new Vector3(transform.localScale.x, 1, 1);
            if (Input.GetMouseButtonDown(0) && Time.time > delay)
            {
                delay = Time.time + (1f / gun.RPS);
                Debug.Log(1 / gun.RPS);
                Fire();
            }
        }
        else
        {
            gunPivot.gameObject.SetActive(false);
            anim.SetBool("Aiming", false);
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

    public void Die ()
    {
    }

    void Fire ()
    {
        audioSources[0].clip = gun.shotShound;
        audioSources[0].pitch = Random.Range(0.8f, 1.2f);
        audioSources[0].Play();

        GameObject bullet = Instantiate(gun.bulletPrefab, shot.position, shot.rotation);
        bullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(gun.speed, 0));
        bullet.GetComponent<Bullet>().gun = gun; 
    }
}
