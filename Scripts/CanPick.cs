using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanPick : MonoBehaviour
{
    public bool pickable = false;
    public bool Pickable
    {
        get { return pickable; }
        set { pickable = value; }
    }

}
