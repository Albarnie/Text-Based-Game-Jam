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

    public static GameManager manager;
    [Header("References")]
    public GameGUIManager guiManager;
    public EnemyManager enemyManager;
    public MenuManager menuManager;
    public ConsoleManager consoleManager;

    [Header("Settings")]
    public SaveData initialData;

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
        SceneManager.sceneLoaded += OnLevelLoad;
    }

    public void Save ()
    {
        saveData.playerData = playerData;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Save.Ts");
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void Load ()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/Save.Ts", FileMode.Open);
        saveData = (SaveData)bf.Deserialize(file);
        file.Close();

        playerData = saveData.playerData;
        StartCoroutine(GoToScene(saveData.playerData.level));
    }

    public void New()
    {
        saveData = initialData;
        StartCoroutine(GoToScene(saveData.playerData.level));
    }

    public void Quit()
    {
        Application.Quit();
    }

    public IEnumerator GoToScene (int level)
    {
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(level);
        while (sceneLoad.progress < 1)
        {
            yield return null;
        }
    }

    void OnLevelLoad (Scene scene, LoadSceneMode mode)
    {

    }

}
