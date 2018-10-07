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
        public int health, level, gun;
        public int[] ammoInClip;
        public int idPrefix;
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
    public Player player;

    [Header("Settings")]
    public SaveData initialData;
    public Gun[] guns;

    [Header("Object Lists")]
    public List<Electronic> electronics;

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
        playerData.level = SceneManager.GetActiveScene().buildIndex;
    }

    public void Save ()
    {
        Debug.Log("Save");
        saveData.playerData = playerData;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Save.Ts");
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void Load ()
    {
        Debug.Log("Load");
        if (File.Exists(Application.persistentDataPath + "/Save.Ts"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Save.Ts", FileMode.Open);
            saveData = (SaveData)bf.Deserialize(file);
            file.Close();

            playerData = saveData.playerData;
            StartCoroutine(GoToScene(saveData.playerData.level));
        }
        else
        {
            New();
        }
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

    public void Credits ()
    {
        StartCoroutine(GoToScene(1));
    }

    public IEnumerator GoToScene (int level)
    {
        playerData.level = level;
        playerData.health = 100;
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(level);
        while (sceneLoad.progress < 1)
        {
            yield return null;
        }
    }

    void OnLevelLoad (Scene scene, LoadSceneMode mode)
    {
        electronics.Clear();
        if(enemyManager != null)
            enemyManager.enemies.Clear();
        playerData.idPrefix = Random.Range(0, 2000);
    }
    
    public static bool Chance (float chance)
    {
        return chance > Random.value;
    }

}
