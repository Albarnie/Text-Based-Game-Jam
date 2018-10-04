﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 lastPos = Vector3.zero;
    public Gun gun;
    public LayerMask mask;
    public GameObject hitPrefab;
    int continues;

    private void Awake()
    {
        lastPos = transform.position;
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Linecast(lastPos, transform.position, mask);
        if(hit.collider != null)
        {
            GetComponent<Rigidbody2D>().velocity *= 0.9f;
            Vector3 inPoint = hit.point;
            Vector3 outPoint = Physics2D.Linecast(transform.position, lastPos, mask).point;
            Instantiate(hitPrefab, inPoint, Quaternion.identity);
            Instantiate(hitPrefab, outPoint, Quaternion.identity);
            Debug.DrawLine(inPoint, outPoint, Color.green, 2);
            switch(hit.collider.tag)
            {
                case "Enemy":
                    break;
            }

            continues++;
        }
        lastPos = transform.position;
        if (GetComponent<Rigidbody2D>().velocity.magnitude < 2 && continues >0)
            Destroy(gameObject);
    }
}