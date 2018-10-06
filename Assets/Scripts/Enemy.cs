using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int id;

    private void Start()
    {
        id = GameManager.manager.enemyManager.enemies.Count;
        GameManager.manager.enemyManager.enemies.Add(this);
    }

    public void Alert(Vector3 position, float soundRadius, float visualRadius)
    {
        
    }
}
