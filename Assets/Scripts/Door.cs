using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Electronic
{

    public override void Disable ()
    {
        base.Disable();
        GetComponent<Collider2D>().enabled = false;
    }

    public override void Enable ()
    {
        base.Enable();
        GetComponent<Collider2D>().enabled = true;
    }

}
