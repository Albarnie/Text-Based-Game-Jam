using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    [Header("Movement Settings")]
    public float speed = 3;

    [Header("Input")]
    public Vector2 input;
    public Vector2 lastVelocity = Vector2.zero;
    public float rotation;
    public Vector2 direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        if (input.sqrMagnitude > 1)
        {
            input.Normalize();
        }

        Move();
    }

    void Move ()
    {
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        direction.Normalize();
        if (direction.x > 0)
        {
            transform.localScale = Vector3.right + Vector3.up + Vector3.forward;
        }
        else if (direction.x < 0)
        {
            transform.localScale = Vector3.left + Vector3.up + Vector3.forward;
        }

        rotation = Mathf.Atan2(direction.y, Mathf.Abs(direction.x)) * Mathf.Rad2Deg;
        anim.SetFloat("Direction", rotation);
        anim.SetFloat("Speed", input.magnitude);
        Vector2 velocity = input;
        velocity *= speed;

        rb.velocity = velocity;
        lastVelocity = velocity;
    }

}
