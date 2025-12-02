using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UI;

public class Calculator : MonoBehaviour
{
    private Tile[,] currentTiles;
    private bool[,] nextAlive;
    private int[,] aliveCount;
    private bool isStart = false;
    [SerializeField]
    private bool useWrapping = false;
    [SerializeField]
    private bool edgeAliveMode = false;
    [SerializeField]
    private Toggle edgeAliveToggle;

    // Rule SO
    [SerializeField]
    private GameRule gameRule;

    public void SetRule(GameRule rule)
    {
        gameRule = rule;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            CalculateTiles();
            UpdateTiles();
            isStart = false;
        }
    }

    public void Initiate(Tile[,] getTiles)
    {
        isStart = true;
        currentTiles = getTiles;

        aliveCount = new int[currentTiles.GetLength(0), currentTiles.GetLength(1)];
        nextAlive = new bool[currentTiles.GetLength(0), currentTiles.GetLength(1)];
    }

    public void CalculateTiles()
    {
        for (int i = 0; i < currentTiles.GetLength(0); i++)
        {
            for (int j = 0; j < currentTiles.GetLength(1); j++)
            {
                CheckNeighbours(i, j);

                if (currentTiles[i, j].IsAlive())
                {
                    if (gameRule.useAliveRule)
                    {
                        nextAlive[i, j] = gameRule.aliveRule[aliveCount[i, j]];
                    }
                    else
                    {
                        nextAlive[i, j] = currentTiles[i, j].IsAlive();
                    }
                }
                else
                {
                    if (gameRule.useDeadRule)
                    {
                        nextAlive[i, j] = gameRule.deadRule[aliveCount[i, j]];
                    }
                    else
                    {
                        nextAlive[i, j] = currentTiles[i, j].IsAlive();
                    }
                }
            }
        }
    }

    public void CheckNeighbours(int i, int j)
    {
        aliveCount[i, j] = 0;

        // Wrapping mode
        if (useWrapping)
        {
            // Left
            if (i - 1 == -1) // if left neighbour is out of the boundary, do wrapping
            {
                if (currentTiles[currentTiles.GetLength(0) - 1, j].IsAlive())
                {
                    aliveCount[i, j]++;
                }
            }
            else if (currentTiles[i - 1, j].IsAlive()) // if it's not out of the boundary, just count it
            {
                aliveCount[i, j]++;
            }

            // Right
            if (i + 1 == currentTiles.GetLength(0)) // if Right neighbour is out of the boundary, do wrapping
            {
                if (currentTiles[0, j].IsAlive())
                {
                    aliveCount[i, j]++;
                }
            }
            else if (currentTiles[i + 1, j].IsAlive()) // if it's not out of the boundary, just count it
            {
                aliveCount[i, j]++;
            }

            // Down
            if (j - 1 == -1) // if Down neighbour is out of the boundary, do wrapping
            {
                if (currentTiles[i, currentTiles.GetLength(1) - 1].IsAlive())
                {
                    aliveCount[i, j]++;
                }
            }
            else if (currentTiles[i, j - 1].IsAlive()) // if it's not out of the boundary, just count it
            {
                aliveCount[i, j]++;
            }

            // Up
            if (j + 1 == currentTiles.GetLength(1)) // if Up neighbour is out of the boundary, do wrapping
            {
                if (currentTiles[i, 0].IsAlive())
                {
                    aliveCount[i, j]++;
                }
            }
            else if (currentTiles[i, j + 1].IsAlive()) // if it's not out of the boundary, just count it
            {
                aliveCount[i, j]++;
            }

            // Left Down
            if (i - 1 == -1 || j - 1 == -1)
            {
                if (i - 1 == -1 && j - 1 == -1)
                {
                    if (currentTiles[currentTiles.GetLength(0) - 1, currentTiles.GetLength(1) - 1].IsAlive())
                    {
                        aliveCount[i, j]++;
                    }
                }
                else if (i - 1 == -1 && j - 1 >= 0)
                {
                    if (currentTiles[currentTiles.GetLength(0) - 1, j - 1].IsAlive())
                    {
                        aliveCount[i, j]++;
                    }
                }
                else if (i - 1 >= 0 && j - 1 == -1)
                {
                    if (currentTiles[i - 1, currentTiles.GetLength(1) - 1].IsAlive())
                    {
                        aliveCount[i, j]++;
                    }
                }
            }
            else if (currentTiles[i - 1, j - 1].IsAlive()) // if it's not out of the boundary, just count it
            {
                aliveCount[i, j]++;
            }

            // Right Up
            if (i + 1 == currentTiles.GetLength(0) || j + 1 == currentTiles.GetLength(1))
            {
                if (i + 1 == currentTiles.GetLength(0) && j + 1 == currentTiles.GetLength(1))
                {
                    if (currentTiles[0, 0].IsAlive())
                    {
                        aliveCount[i, j]++;
                    }
                }
                else if (i + 1 == currentTiles.GetLength(0) && j + 1 < currentTiles.GetLength(1))
                {
                    if (currentTiles[0, j + 1].IsAlive())
                    {
                        aliveCount[i, j]++;
                    }
                }
                else if (i + 1 < currentTiles.GetLength(0) && j + 1 == currentTiles.GetLength(1))
                {
                    if (currentTiles[i + 1, 0].IsAlive())
                    {
                        aliveCount[i, j]++;
                    }
                }
            }
            else if (currentTiles[i + 1, j + 1].IsAlive()) // if it's not out of the boundary, just count it
            {
                aliveCount[i, j]++;
            }

            // Right Down
            if (i + 1 == currentTiles.GetLength(0) || j - 1 == -1)
            {
                if (i + 1 == currentTiles.GetLength(0) && j - 1 == -1)
                {
                    if (currentTiles[0, currentTiles.GetLength(1) - 1].IsAlive())
                    {
                        aliveCount[i, j]++;
                    }
                }
                else if (i + 1 == currentTiles.GetLength(0) && j - 1 >= 0)
                {
                    if (currentTiles[0, j - 1].IsAlive())
                    {
                        aliveCount[i, j]++;
                    }
                }
                else if (i + 1 < currentTiles.GetLength(0) && j - 1 == -1)
                {
                    if (currentTiles[i + 1, currentTiles.GetLength(1) - 1].IsAlive())
                    {
                        aliveCount[i, j]++;
                    }
                }
            }
            else if (currentTiles[i + 1, j - 1].IsAlive()) // if it's not out of the boundary, just count it
            {
                aliveCount[i, j]++;
            }

            // Left Up
            if (i - 1 == -1 || j + 1 == currentTiles.GetLength(1))
            {
                if (i - 1 == -1 && j + 1 == currentTiles.GetLength(1))
                {
                    if (currentTiles[currentTiles.GetLength(0) - 1, 0].IsAlive())
                    {
                        aliveCount[i, j]++;
                    }
                }
                else if (i - 1 == -1 && j + 1 < currentTiles.GetLength(1))
                {
                    if (currentTiles[currentTiles.GetLength(0) - 1, j + 1].IsAlive())
                    {
                        aliveCount[i, j]++;
                    }
                }
                else if (i - 1 >= 0 && j + 1 == currentTiles.GetLength(1))
                {
                    if (currentTiles[i - 1, 0].IsAlive())
                    {
                        aliveCount[i, j]++;
                    }
                }
            }
            else if (currentTiles[i - 1, j + 1].IsAlive()) // if it's not out of the boundary, just count it
            {
                aliveCount[i, j]++;
            }
        }
        else if(!useWrapping) // Walls mode
        {
            if (i - 1 >= 0)
            {
                if (currentTiles[i - 1, j].IsAlive())
                {
                    aliveCount[i, j]++;
                }
            }
            else if (edgeAliveMode)
            {
                aliveCount[i, j]++;
            }

            // Right
            if (i + 1 < currentTiles.GetLength(0))
            {
                if (currentTiles[i + 1, j].IsAlive())
                {
                    aliveCount[i, j]++;
                }
            }
            else if (edgeAliveMode)
            {
                aliveCount[i, j]++;
            }

            // Down
            if (j - 1 >= 0)
            {
                if (currentTiles[i, j - 1].IsAlive())
                {
                    aliveCount[i, j]++;
                }
            }
            else if (edgeAliveMode)
            {
                aliveCount[i, j]++;
            }

            // Up
            if (j + 1 < currentTiles.GetLength(1))
            {
                if (currentTiles[i, j + 1].IsAlive())
                {
                    aliveCount[i, j]++;
                }
            }
            else if (edgeAliveMode)
            {
                aliveCount[i, j]++;
            }

            // Left Down
            if (i - 1 >= 0 && j - 1 >= 0)
            {
                if (currentTiles[i - 1, j - 1].IsAlive())
                {
                    aliveCount[i, j]++;
                }
            }
            else if (edgeAliveMode)
            {
                aliveCount[i, j]++;
            }

            // Right Up
            if (i + 1 < currentTiles.GetLength(0) && j + 1 < currentTiles.GetLength(1))
            {
                if (currentTiles[i + 1, j + 1].IsAlive())
                {
                    aliveCount[i, j]++;
                }
            }
            else if (edgeAliveMode)
            {
                aliveCount[i, j]++;
            }

            // Right Down
            if (i + 1 < currentTiles.GetLength(0) && j - 1 >= 0)
            {
                if (currentTiles[i + 1, j - 1].IsAlive())
                {
                    aliveCount[i, j]++;
                }
            }
            else if (edgeAliveMode)
            {
                aliveCount[i, j]++;
            }

            // Left Up (i - 1, j + 1)
            if (i - 1 >= 0 && j + 1 < currentTiles.GetLength(1))
            {
                if (currentTiles[i - 1, j + 1].IsAlive())
                {
                    aliveCount[i, j]++;
                }
            }
            else if (edgeAliveMode)
            {
                aliveCount[i, j]++;
            }
        }
    }

    public void UpdateTiles()
    {
        for (int i = 0; i < currentTiles.GetLength(0); i++)
        {
            for (int j = 0; j < currentTiles.GetLength(1); j++)
            {
                if (nextAlive[i, j])
                {
                    currentTiles[i, j].ActivateTile();
                }
                else
                {
                    currentTiles[i, j].ResetTile();
                }
            }
        }
    }

    public void SetUseWrapping(bool value)
    {
        useWrapping = value;

        if (useWrapping)
        {
            edgeAliveMode = false;
            edgeAliveToggle.isOn = false;
        }
    }

    public void SetEdgeAliveMode(bool value)
    {
        edgeAliveMode = value;
    }
}
