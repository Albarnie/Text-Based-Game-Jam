using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.manager.playerData.level + 1 < SceneManager.sceneCountInBuildSettings)
        {
            GameManager.manager.playerData.level++;
            GameManager.manager.Save();
            StartCoroutine(GameManager.manager.GoToScene(GameManager.manager.playerData.level));
        }
        else
        {
            StartCoroutine(GameManager.manager.GoToScene(1));
        }
    }
}
