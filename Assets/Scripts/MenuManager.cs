using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject[] menus;

    private void Start()
    {
        GameManager.manager.menuManager = this;
    }

    public void ChangeMenu (int id)
    {
        foreach(GameObject menu in menus)
        {
            menu.SetActive(false);
        }
        if(id < menus.Length && id >= 0)
            menus[id].SetActive(true);
    }

}
