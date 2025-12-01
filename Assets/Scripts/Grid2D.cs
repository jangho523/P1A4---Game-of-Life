using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2D : MonoBehaviour
{
    private Tile[,] tiles;
    [SerializeField]
    private Tile tilePrefab;

    public Tile[,] GetTiles()
    {
        return tiles;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateGrid2D(int sizeX, int sizeY)
    {
        tiles = new Tile[sizeX, sizeY];

        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                CreateOneTile(i, j);
            }
        }
    }

    private void CreateOneTile(int xPosition, int yPosition)
    {
        Tile tile = Instantiate(tilePrefab);

        tiles[xPosition, yPosition] = tile;

        Vector3 newPosition = new Vector3(xPosition, yPosition, 0);

        tile.transform.position = newPosition;
    }

    public void ResetTiles()
    {
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                tiles[i, j].ResetTile();
            }
        }
    }

    public void DestroyAllTiles()
    {
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                if (tiles[i, j] != null)
                {
                    Destroy(tiles[i, j].gameObject);
                }
            }
        }

        tiles = null;
    }
}
