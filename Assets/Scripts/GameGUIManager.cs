using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameGUIManager : MonoBehaviour
{
    public GameObject worldNotify;
    public TextMeshProUGUI notifyText;
    Vector3 notifyPos;

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
        worldNotify.transform.position = Vector3.Slerp(worldNotify.transform.position, notifyPos, 5 * Time.deltaTime);
    }
}
