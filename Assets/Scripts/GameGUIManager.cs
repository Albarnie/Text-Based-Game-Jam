﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameGUIManager : MonoBehaviour
{
    public GameObject worldNotify;
    public TextMeshProUGUI notifyText, ammoText, healthText, gunText;
    Vector3 notifyPos;
    public bool paused;

    private void Start()
    {
        GameManager.manager.guiManager = this;
    }

    public void UpdateWorldNotify(string text, Vector3 position)
    {
        notifyText.text = text;
        notifyPos = position;
    }

    private void Update()
    {
        if(worldNotify != null)
            worldNotify.transform.position = Vector3.Slerp(worldNotify.transform.position, notifyPos, 5 * Time.deltaTime);
        if (Input.GetKeyDown("escape") && paused)
        {
            UnPause();
        }
        else if (Input.GetKeyDown("escape"))
        {
            Pause();
        }
        healthText.text = GameManager.manager.playerData.health.ToString();
        ammoText.text = GameManager.manager.playerData.ammoInClip[GameManager.manager.playerData.gun].ToString();
        gunText.text = GameManager.manager.guns[GameManager.manager.playerData.gun].name;
    }

    public void Pause ()
    {
        paused = true;
        GameManager.manager.menuManager.ChangeMenu(0);
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        paused = false;
        GameManager.manager.menuManager.ChangeMenu(2);
        Time.timeScale = 1;
    }

    public void Save ()
    {
        GameManager.manager.Save();
        UnPause();
    }

    public void MainMenu ()
    {
        UnPause();
        GameManager.manager.Save();
        StartCoroutine(GameManager.manager.GoToScene(0));
    }

    public void Credits ()
    {
        UnPause();
        StartCoroutine(GameManager.manager.GoToScene(1));
    }
}
