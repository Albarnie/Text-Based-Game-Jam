using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public struct PlayerData
    {
        public int health, level;
    }

    [System.Serializable]
    public struct SaveData
    {
        public PlayerData playerData;
    }

    [Header("References")]
    public static GameManager manager;
    public GameGUIManager guiManager;
    public EnemyManager enemyManager;
    public MenuManager menuManager;
    public ConsoleManager consoleManager;

    [Header("Object Lists")]
    public List<Door> doors;
    public List<Enemy> enemies;
    public List<SecurityCamera> cameras;

    [Header("Data")]
    public PlayerData playerData;
    public SaveData saveData;

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

    public void Save ()
    {
        saveData.playerData = playerData;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Save.Ts");
        bf.Serialize(file, saveData);
        file.Close();
    }

}
