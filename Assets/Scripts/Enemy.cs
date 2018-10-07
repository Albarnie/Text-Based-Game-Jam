using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int id = 0;

    private void Start()
    {
        GameManager.manager.enemyManager.enemies.Add(this);
        id = GameManager.manager.enemyManager.enemies.Count -1;
    }

    public void Alert(Vector3 position, float soundRadius, float visualRadius)
    {
        
    }
}
