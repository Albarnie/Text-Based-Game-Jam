using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

    public void New ()
    {
        GameManager.manager.New();
    }

    public void Continue ()
    {
        GameManager.manager.Load();
    }

    public void Quit ()
    {
        GameManager.manager.Quit();
    }

}
