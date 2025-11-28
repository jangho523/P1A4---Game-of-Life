using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class Calculator : MonoBehaviour
{
    private Tile[,] currentTiles;
    private bool[,] nextAlive;
    private int[,] aliveCount;
    private bool isStart = false;

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
                CheckNeighbours(i, j); // neighbours는 currentTiles에서만 읽음

                if (currentTiles[i, j].IsAlive())
                {
                    if (aliveCount[i, j] < 2 || aliveCount[i, j] > 3)
                    {
                        nextAlive[i, j] = false;
                    }
                    else
                    {
                        nextAlive[i, j] = true;
                    }
                }
                else
                {
                    if (aliveCount[i, j] == 3)
                    {
                        nextAlive[i, j] = true;
                    }
                    else
                    {
                        nextAlive[i, j] = false;
                    }
                }
            }
        }
    }

    public void CheckNeighbours(int i, int j)
    {
        aliveCount[i, j] = 0;

        if (i - 1 >= 0 && currentTiles[i - 1, j].IsAlive())
        {
            aliveCount[i, j]++;
        }
        if (i + 1 < currentTiles.GetLength(0) && currentTiles[i + 1, j].IsAlive())
        {
            aliveCount[i, j]++;
        }
        if (j - 1 >= 0 && currentTiles[i, j - 1].IsAlive())
        {
            aliveCount[i, j]++;
        }
        if (j + 1 < currentTiles.GetLength(1) && currentTiles[i, j + 1].IsAlive())
        {
            aliveCount[i, j]++;
        }
        if (i - 1 >= 0 && j - 1 >= 0 && currentTiles[i - 1, j - 1].IsAlive())
        {
            aliveCount[i, j]++;
        }
        if (i + 1 < currentTiles.GetLength(0) && j + 1 < currentTiles.GetLength(1) && currentTiles[i + 1, j + 1].IsAlive())
        {
            aliveCount[i, j]++;
        }
        if (i + 1 < currentTiles.GetLength(0) && j - 1 >= 0 && currentTiles[i + 1, j - 1].IsAlive())
        {
            aliveCount[i, j]++;
        }
        if (i - 1 >= 0 && j + 1 < currentTiles.GetLength(1) && currentTiles[i - 1, j + 1].IsAlive())
        {
            aliveCount[i, j]++;
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
}
