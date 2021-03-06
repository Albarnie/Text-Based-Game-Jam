﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : Electronic
{
    [Header("Camera Settings")]
    public LayerMask visionMask;
    public int FOV = 45;
    public float detection;
    public float maxDetection;

    private void Update()
    {
        if(CanSeePlayer() && !disabled)
        {
            detection += Time.deltaTime;
            if(detection > maxDetection)
            {
                GameManager.manager.enemyManager.SetAlarm();
            }
        }
        else if (detection > 0)
        {
            detection -= Time.deltaTime;
        }
    }

    public override void Disable()
    {
        base.Disable();
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public override void Enable()
    {
        base.Enable();
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public bool CanSeePlayer ()
    {
        Debug.DrawLine(transform.position, transform.position + (Quaternion.Euler(0, 0, FOV / 2) * (transform.localScale.x * Vector2.right)* 10));
        Debug.DrawLine(transform.position, transform.position + (Quaternion.Euler(0, 0, -FOV / 2) * (transform.localScale.x * Vector2.right)* 10));
        Vector3 playerPos = GameManager.manager.player.transform.position;
        if (Vector3.Angle(transform.localScale * Vector2.right, playerPos - transform.position) < FOV / 2 && Vector3.Distance(playerPos, transform.position) < 12.8)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, playerPos, visionMask);
            if(hit.collider == null)
            {
                Debug.DrawLine(transform.position, playerPos, Color.yellow);
                return true;
            }
            else
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);
            }
        }
        return false;
    }
}
