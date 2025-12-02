using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool isActivated = false;

    public bool IsAlive()
    {
        return isActivated;
    }

    public void ActivateTile()
    {
        isActivated = true;
    }

    public void ResetTile()
    {
        isActivated = false;
    }
}
