using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameOfLife/GameRule")]
public class GameRule : ScriptableObject
{
    public bool[] aliveRule = new bool[9];
    public bool[] deadRule = new bool[9];

    public bool useAliveRule = true;
    public bool useDeadRule = true;
}
