using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electronic : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;

    [Header("Electronic settings")]

    [Multiline]
    public string deviceName;
    public int id;
    public bool connected = true;

    public Sprite[] sprites = new Sprite[2];

    public bool disabled;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (connected)
        {
            id = GameManager.manager.electronics.Count;
            GameManager.manager.electronics.Add(this);
        }
    }

    public virtual void Enable ()
    {
        disabled = false;
        spriteRenderer.sprite = sprites[0];
    }

    public virtual void Disable()
    {
        disabled = true;
        spriteRenderer.sprite = sprites[1];
    }

}
