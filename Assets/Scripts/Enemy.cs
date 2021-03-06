﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    AudioSource[] audioSources;
    Animator anim;

    [Header("Movement Settings")]
    public float speed = 6;

    [Header("Vision Settings")]
    public Vector3 startDirection;
    public LayerMask visionMask;
    public int FOV = 45;
    public float timeOutDuration = 15;
    public Vector3 lastPlayerPos, predictedPlayerPos, lastPlayerSpeed, playerPos;
    public float playerTime, infoAge;
    public bool hasSeenPlayer, canSeePlayer;

    float rotation;
    Vector3 targetPos;
    Vector2 input, direction;

    [Header("Combat Settings")]
    public int ammoInClip;
    public int health = 100;
    public Gun gun;
    public Transform shot, gunPivot;

    float delay;

    public int id = 0;

    private void Awake()
    {
        targetPos = transform.position + startDirection;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSources = GetComponents<AudioSource>();
    }

    private void Start()
    {
        GameManager.manager.enemyManager.enemies.Add(this);
        id = GameManager.manager.enemyManager.enemies.Count -1;
    }

    private void Update()
    {
        canSeePlayer = CanSeePlayer();
        playerPos = GameManager.manager.player.transform.position;
        if(canSeePlayer)
        {
            playerTime += Time.deltaTime;
        }
        else if (!hasSeenPlayer)
        {
            playerTime = 0;
        }

        if(canSeePlayer && playerTime > 0.3)
        {
            hasSeenPlayer = true;
            infoAge = 0;
            targetPos = playerPos;
            lastPlayerSpeed = (playerPos - lastPlayerPos) /Time.deltaTime;
            lastPlayerPos = playerPos;
            Debug.DrawLine(shot.position, playerPos, Color.green);
            input = lastPlayerPos - transform.position;
            targetPos = playerPos;
            anim.SetBool("Aiming", true);
        }
        else if (hasSeenPlayer && infoAge < timeOutDuration)
        {
            infoAge += Time.deltaTime;
            predictedPlayerPos = lastPlayerPos + (lastPlayerSpeed * infoAge);
            Debug.DrawLine(shot.position, predictedPlayerPos, Color.yellow);
            Debug.DrawLine(shot.position, lastPlayerPos, Color.red);
            input = lastPlayerPos - transform.position;
            targetPos = predictedPlayerPos;
            anim.SetBool("Aiming", true);
        }
        else
        {
            anim.SetBool("Aiming", false);
            infoAge = timeOutDuration;
        }
        if(Vector3.Distance(transform.position, playerPos) > 2 && hasSeenPlayer)
            Move();
        Rotate();
        if (Vector3.Distance(transform.position, playerPos) > 5 && delay < Time.time && canSeePlayer && hasSeenPlayer && ammoInClip > 0)
        {
            ammoInClip--;
            delay = Time.time + (1f / gun.RPS) + Random.Range(0, (0.5f / gun.RPS));
            Fire();
        }
        else if (ammoInClip <=0 && delay < Time.time)
        {
            delay = Time.time + gun.reloadTime;
            Reload();
        }
    }

    public void Alert(Vector3 position, float soundRadius, float visualRadius)
    {
        if (GameManager.Chance(Vector3.Distance(transform.position, position) / soundRadius) || CanSeePlayer() && Vector3.Distance(transform.position, position) < visualRadius)
        {
            infoAge = 0;
            hasSeenPlayer = true;
            lastPlayerPos = playerPos;
            targetPos = playerPos;
        }
    }

    void Move()
    {
        Debug.DrawLine(transform.position, targetPos);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, shot.rotation * Vector2.right, 1, visionMask);
        if(hit.collider != null)
        {
            float upDistance = Physics2D.Raycast(transform.position, shot.rotation * Vector2.up, 30, visionMask).distance;
            float downDistance = Physics2D.Raycast(transform.position, shot.rotation * Vector2.down, 30, visionMask).distance;
            if(upDistance > downDistance)
            {
                input = shot.rotation * Vector2.up;
                //direction *= shot.rotation * Vector2.up;
            }
            else
            {
                input = shot.rotation * Vector2.down;
                //direction *= shot.rotation * Vector2.down;
            }
        }
        direction.Normalize();
        if (direction.x > 0)
        {
            transform.localScale = Vector3.right + Vector3.up + Vector3.forward;
        }
        else if (direction.x < 0)
        {
            transform.localScale = Vector3.left + Vector3.up + Vector3.forward;
        }

        if(input.sqrMagnitude > 1)
        {
            input.Normalize();
        }
        Debug.DrawLine(transform.position, transform.position + (Vector3)(input * 2));
        Vector2 velocity = input;
        velocity *= speed;

        rb.velocity = velocity;
        anim.SetFloat("Speed", input.magnitude);
    }

    void Rotate ()
    {
        direction = targetPos - transform.position;
        rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gunPivot.localScale = transform.localScale;
        gunPivot.rotation = Quaternion.Euler(0, 0, rotation);
        anim.SetFloat("Direction", Mathf.Atan2(direction.y, Mathf.Abs(direction.x)) * Mathf.Rad2Deg);
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

    public void Damage (int amount)
    {
        Debug.DrawLine(playerPos, transform.position, Color.green, 1);
        Debug.Log(amount + ", " + health);
        health -= amount;
        if(health <= 0)
        {
            Die();
        }
    }

    public void Die ()
    {
        anim.SetTrigger("Die");
        rb.drag = 5;
        GameManager.manager.enemyManager.enemies.Remove(this);
        foreach(Collider2D col in GetComponentsInChildren<Collider2D>())
        {
            Destroy(col);
        }
        Destroy(this);
    }

    void Reload()
    {
        Debug.Log("reload");
        audioSources[0].clip = gun.reloadSound;
        audioSources[0].pitch = Random.Range(0.8f, 1.2f);
        audioSources[0].Play();
        ammoInClip = gun.ammoPerClip;
    }

    bool CanSeePlayer()
    {
        Debug.DrawLine(shot.position, shot.position + (shot.rotation * Vector2.right));
        Debug.DrawLine(shot.position, shot.position + (Quaternion.Euler(0, 0, FOV / 2) * shot.rotation * Vector2.right * 5));
        Debug.DrawLine(shot.position, shot.position + (Quaternion.Euler(0, 0, -FOV / 2) * shot.rotation * Vector2.right * 5));
        if (Vector3.Angle(shot.rotation*Vector2.right, playerPos - transform.position) < FOV / 2)
        {
            RaycastHit2D hit = Physics2D.Linecast(shot.position, playerPos, visionMask);
            if (hit.collider == null)
            {
                Debug.DrawLine(shot.position, playerPos, Color.yellow);
                return true;
            }
            else
            {
                Debug.DrawLine(shot.position, hit.point, Color.red);
            }
        }
        Debug.DrawLine(shot.position, playerPos, Color.red);
        return false;
    }
}
