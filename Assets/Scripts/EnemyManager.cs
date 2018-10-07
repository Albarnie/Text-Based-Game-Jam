using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    private void Start()
    {
        GameManager.manager.enemyManager = this;
        enemies = new List<Enemy>();
    }

    public List<Enemy> enemies = new List<Enemy>();

    public void Alert (Vector3 position, float soundRadius, float visualRadius)
    {
        foreach(Enemy enemy in enemies)
        {
            enemy.Alert(position, soundRadius, visualRadius);
        }
    }

    public void SetAlarm ()
    {
        foreach (Electronic electronic in GameManager.manager.electronics)
        {
            if(electronic is Alarm && !electronic.disabled)
            {
                ((Alarm)electronic).SetOff();
            }
        }
    }

}
