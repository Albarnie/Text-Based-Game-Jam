using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    public EnemySpawner spawner;
    bool alerted = false;

    private void Start()
    {
        GameManager.manager.enemyManager = this;
        enemies = new List<Enemy>();
    }

    public void Alert (Vector3 position, float soundRadius, float visualRadius)
    {
        foreach(Enemy enemy in enemies)
        {
            enemy.Alert(position, soundRadius, visualRadius);
        }
    }

    public void SetAlarm ()
    {
        if (alerted)
            return;
        spawner.Spawn();

        foreach (Electronic electronic in GameManager.manager.electronics)
        {
            if(electronic is Alarm && !electronic.disabled)
            {
                ((Alarm)electronic).SetOff();
            }
        }
        alerted = true;
    }

}
