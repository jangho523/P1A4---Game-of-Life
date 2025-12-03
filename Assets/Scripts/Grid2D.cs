using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Grid2D
 * 
 * Creates and manages a 2D grid of Tile objects.
 * 
 */
public class Grid2D : MonoBehaviour
{
    private Tile[,] tiles;
    [SerializeField]
    private Tile tilePrefab;

    /* GetTiles
     * 
     * Returns the tile grid.
     * 
     * Parameters: None
     * 
     * Return: Tile[,]
     */
    public Tile[,] GetTiles()
    {
        return tiles;
    }

    /* CreateGrid2D
     * 
     * Creates a new grid and spawns all tile instances.
     * 
     * Parameters: sizeX, sizeY
     * 
     * Return: None
     */
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


    /* CreateOneTile
     * 
     * Instantiates one tile at (xPosition, yPosition).
     * 
     * Parameters: xPosition, yPosition
     * 
     * Return: None
     */
    private void CreateOneTile(int xPosition, int yPosition)
    {
        Tile tile = Instantiate(tilePrefab);

        tiles[xPosition, yPosition] = tile;

        Vector3 newPosition = new Vector3(xPosition, yPosition, 0);
        tile.transform.position = newPosition;
    }

    /* ResetTiles
     * 
     * Resets all tiles in the grid to dead state.
     * 
     * Parameters: None
     * 
     * Return: None
     */
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

    /* DestroyAllTiles
     * 
     * Destroys all spawned tiles and clears the grid reference.
     * 
     * Parameters: None
     * 
     * Return: None
     */
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
