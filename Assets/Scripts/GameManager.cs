using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    public GameGUIManager gui;
    public EnemyManager enemyManager;

    [System.Serializable]
    public struct PlayerData
    {
        public int health;
    }

    public List<Door> doors;
    public List<Enemy> enemies;
    public List<SecurityCamera> cameras;

    public PlayerData playerData;

    private void Awake()
    {
        if(manager == null)
        {
            manager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (manager != this)
        {
            Destroy(gameObject);
        }
    }

}
