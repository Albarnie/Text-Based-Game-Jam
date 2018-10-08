using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : Electronic
{
    [Header("Camera Settings")]
    public LayerMask visionMask;
    public int FOV = 45;
    public float detection;
    public float maxDetection;
    public Transform lense;

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
        Debug.DrawLine(lense.position, lense.position + (Quaternion.Euler(0, 0, FOV / 2) * lense.rotation * (transform.localScale.x * Vector2.right)* 10));
        Debug.DrawLine(lense.position, lense.position + (Quaternion.Euler(0, 0, -FOV / 2) * lense.rotation * (transform.localScale.x * Vector2.right)* 10));
        Vector3 playerPos = GameManager.manager.player.transform.position;
        Vector3 dir = lense.rotation * (transform.localScale.x * Vector3.right);
        Debug.DrawRay(transform.position, dir.normalized);
        Debug.DrawRay(transform.position, (playerPos - lense.position).normalized);
        //Debug.Log(Vector3.Angle(dir.normalized, (playerPos - lense.position).normalized));
        //Debug.DrawLine(transform.position, transform.position + (Quaternion.Euler(0, 0, Vector3.Angle(dir.normalized, (playerPos - lense.position).normalized)) * Vector3.right * transform.localScale.x));
        if (Vector3.Angle(dir.normalized, (playerPos - lense.position).normalized) < FOV / 2 && Vector3.Distance(playerPos, lense.position) < 12.8)
        {
            Debug.Log("Small angle");
            RaycastHit2D hit = Physics2D.Linecast(lense.position, playerPos, visionMask);
            if(hit.collider == null)
            {
                Debug.DrawLine(lense.position, playerPos, Color.yellow);
                return true;
            }
            else
            {
                Debug.DrawLine(lense.position, hit.point, Color.red);
            }
        }
        return false;
    }
}
