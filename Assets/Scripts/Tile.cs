using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Tile
 * 
 * Stores alive/dead state for each grid cell.
 * 
 */
public class Tile : MonoBehaviour
{
    private bool isActivated = false;

    /* IsAlive
     * 
     * Returns whether this tile is alive.
     * 
     * Parameters: None
     * 
     * Return: bool
     */
    public bool IsAlive()
    {
        return isActivated;
    }


    /* ActivateTile
     * 
     * Sets tile state to alive.
     * 
     * Parameters: None
     * 
     * Return: None
     */
    public void ActivateTile()
    {
        isActivated = true;
    }

    /* ResetTile
     * 
     * Sets tile state to dead.
     * 
     * Parameters: None
     * 
     * Return: None
     */
    public void ResetTile()
    {
        isActivated = false;
    }
}
