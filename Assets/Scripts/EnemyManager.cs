﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    private void Start()
    {
        GameManager.manager.enemyManager = this;
    }


    public List<Enemy> enemies;

    public void Alert (Vector3 position, float soundRadius, float visualRadius)
    {
        foreach(Enemy enemy in enemies)
        {
            enemy.Alert(position, soundRadius, visualRadius);
        }
    }

}
