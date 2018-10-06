using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(GameManager.manager.GoToScene(GameManager.manager.playerData.level + 1));
    }
}
