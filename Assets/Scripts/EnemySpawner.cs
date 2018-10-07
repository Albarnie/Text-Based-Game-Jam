using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int amount = 1;
    public GameObject enemyPrefab;

    private void Start()
    {
        GameManager.manager.enemyManager.spawner = this;
    }

    public void Spawn ()
    {
        for(int i = 0; i < amount; i++)
        {
            Enemy enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity).GetComponent<Enemy>();
            enemy.Alert(GameManager.manager.player.transform.position, 100, 200);
        }
    }
}
