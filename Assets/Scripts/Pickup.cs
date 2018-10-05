using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupType
    {
        Pickup,
        Gun
    }

    public PickupType type;
    public int id;
}
