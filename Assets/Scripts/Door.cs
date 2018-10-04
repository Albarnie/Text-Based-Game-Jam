using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Electronic
{
    SpriteRenderer spriteRenderer;

    public Sprite[] sprites = new Sprite[2];

    public bool open;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        id = GameManager.manager.doors.Count;
        GameManager.manager.doors.Add(this);
    }

    public void Open ()
    {
        open = true;
        GetComponent<Collider2D>().enabled = false;
        spriteRenderer.sprite = sprites[1];
    }

    public void Close ()
    {
        open = false;
        GetComponent<Collider2D>().enabled = true;
        spriteRenderer.sprite = sprites[0];
    }

}
