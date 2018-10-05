using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : Electronic
{

    public bool on;

    private void Start()
    {
        id = GameManager.manager.cameras.Count;
        GameManager.manager.cameras.Add(this);
    }

    public void Disable()
    {
        on = false;
    }

    public void Enable()
    {
        on = true;
    }

}
