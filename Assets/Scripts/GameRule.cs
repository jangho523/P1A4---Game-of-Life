using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * GameRule
 * 
 * A ScriptableObject that lets the user manually create and customize Game of Life rules.
 * 
 * Each index represents neighbour count(0~8)
 * 
 */
[CreateAssetMenu(menuName = "GameOfLife/GameRule")]
public class GameRule : ScriptableObject
{
    // Alive -> Alive rules based on neighbour count
    public bool[] aliveRule = new bool[9];

    // Dead -> Alive rules based on neighbour count
    public bool[] deadRule = new bool[9];

    // Toggles usage of alive and dead rules
    public bool useAliveRule = true;
    public bool useDeadRule = true;
}
