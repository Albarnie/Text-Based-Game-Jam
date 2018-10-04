using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int id;

    private void Start()
    {
        id = GameManager.manager.enemies.Count;
        GameManager.manager.enemies.Add(this);
    }

    public void Alert(Vector3 position, float soundRadius, float visualRadius)
    {
        
    }
}
