using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;

/*
 * GameManager
 * 
 * Handles the overall Game of Life simulation
 * 
 * grid creation, UI controls, rule switching, stepping, and tilemap updates.
 */
public class GameManager : MonoBehaviour
{
    // --- For Rule Mode Slider ---
    private enum RuleMode
    {
        SingleRule,
        RuleSequence
    }

    // --- Grid Settings ---
    [Header("Grid Settings")]
    [SerializeField]
    private RuleMode ruleMode = RuleMode.RuleSequence;
    [SerializeField]
    private GameRule singleRule;
    [SerializeField]
    private int gridSizeX = 10;
    [SerializeField]
    private int gridSizeY = 10;
    [SerializeField, Range(0f, 100f)]
    private float aliveChance = 30f;

    // --- Simulation Control ---
    private bool isRunning = false;
    [SerializeField]
    private float stepInterval = 0.5f;
    private float timer = 0f;
    private int generation = 0;

    // --- references --- 
    [Header("References")]
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Text RunPauseButtonText;
    [SerializeField]
    private Text sizeText;
    [SerializeField]
    private Text aliveChanceText;
    [SerializeField]
    private Text generationText;
    [SerializeField]
    private Text RunSpeedText;
    [SerializeField]
    private RuleSequence ruleSequence;
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private RuleTile landTile;
    [SerializeField]
    private TileBase waterTile;
    [SerializeField]
    private Grid2D grid;
    [SerializeField]
    private Calculator calculator;
    [SerializeField]
    private GameRule[] ruleOptions;

    // --- Pattern Stamping ---
    private enum StampPattern
    {
        None,
        Block,
        Blinker,
        Glider
    }
    private StampPattern currentPattern = StampPattern.None;

    // Start is called before the first frame update
    void Start()
    {
        grid.CreateGrid2D(gridSizeX, gridSizeY);
        SetCamera();
        UpdateTilemap(grid.GetTiles());
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            timer += Time.deltaTime;

            if (timer >= stepInterval)
            {
                OneStep();
                timer = 0f;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            // Get the mouse position
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);

            Tile[,] tiles = grid.GetTiles();
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);

            // select the tile under it
            if (cellPosition.x >= 0 && cellPosition.x < width && cellPosition.y >= 0 && cellPosition.y < height)
            {
                Tile tile = tiles[cellPosition.x, cellPosition.y];

                // toggles its state.
                if (tile.IsAlive())
                {
                    tile.ResetTile();
                }
                else
                {
                    tile.ActivateTile();
                }

                UpdateTilemap(tiles);
            }
        }
    }
    
    /* ToggleRun
     * 
     * Starts or pauses the simulation.
     * 
     * Parameters: None
     * 
     * Return: None
     */
    public void ToggleRun()
    {
        isRunning = !isRunning;

        if (isRunning)
        {
            RunPauseButtonText.text = "Pause";
        }
        else
        {
            RunPauseButtonText.text = "Run";
            timer = 0f;
        }
    }


    /* OneStep
     * 
     * Do one generation step based on the selected rule mode
     * 
     * Parameters: None
     * 
     * Return: None
     */
    public void OneStep()
    {
        if(ruleMode == RuleMode.SingleRule)
        {
            calculator.SetRule(singleRule);
        }
        else if(ruleMode == RuleMode.RuleSequence)
        {
            int index = generation % ruleSequence.rules.Count; // to limit index value 
            calculator.SetRule(ruleSequence.rules[index]);
        }

        calculator.Initiate(grid.GetTiles());
        generation++;
        UpdateTilemap(grid.GetTiles()); // Tilemape visual update
        generationText.text = "Generation: " + generation.ToString();
    }

    /* ResetSimulation
     * 
     * Resets grid, generation count, and stops the simulation.
     * 
     * Parameters: None
     * 
     * Return: None
     */
    public void ResetSimulation()
    {
        grid.ResetTiles();
        generation = 0;
        generationText.text = "Generation: " + generation.ToString();
        timer = 0f;
        if (isRunning)
        {
            isRunning = false;
            RunPauseButtonText.text = "Run";
        }
        UpdateTilemap(grid.GetTiles());
    }

    /* SetCamera
     * 
     * set the camera view to the center and adjusts zoom based on grid size
     * 
     * Parameters: None
     * 
     * Return: None
     */
    public void SetCamera()
    {
        float posX = (gridSizeX - 1) / 2f;
        float posY = (gridSizeY - 1) / 2f;

        mainCamera.transform.position = new Vector3(posX, posY, -10f);

        if (gridSizeX >= gridSizeY)
        {
            mainCamera.orthographicSize = (gridSizeX / 2f) + 3f;
        }
        else
        {
            mainCamera.orthographicSize = (gridSizeY / 2f) + 3f;
        }
    }
    
    /* GridSizeChange
     * 
     * Updates grid size value and recreates the grid.
     * 
     * it is called by the grid size slider.
     * 
     * Parameters: value
     * 
     * Return: None
     */
    public void GridSizeChange(float value)
    {
        int newSize = Mathf.RoundToInt(value);

        gridSizeX = newSize;
        gridSizeY = newSize;

        sizeText.text = "Grid Size: " + newSize.ToString();

        RecreateGrid();
    }

    /* RecreateGrid
     * 
     * Recreate all tiles and simulation state after changing grid size with the slider.
     * 
     * Parameters: None
     * 
     * Return: None
     */
    public void RecreateGrid()
    {
        grid.DestroyAllTiles();

        grid.CreateGrid2D(gridSizeX, gridSizeY);

        generation = 0;
        generationText.text = "Generation: " + generation.ToString();
        timer = 0f;
        if (isRunning)
        {
            isRunning = false;
            RunPauseButtonText.text = "Run";
        }
        SetCamera();
        UpdateTilemap(grid.GetTiles());
    }

    /* RandomizeGrid
     * 
     * Randomly activates tiles based on aliveChance
     * 
     * connected with Randomize UI button
     * 
     * Parameters: None
     * 
     * Return: None
     */
    public void RandomizeGrid()
    {
        grid.ResetTiles();

        Tile[,] tiles = grid.GetTiles();

        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                if (Random.value * 100f < aliveChance)
                {
                    tiles[i, j].ActivateTile();
                }
                else
                {
                    tiles[i, j].ResetTile();
                }
            }
        }

        generation = 0;
        generationText.text = "Generation: " + generation.ToString();
        timer = 0f;
        if (isRunning)
        {
            isRunning = false;
            RunPauseButtonText.text = "Run";
        }
        UpdateTilemap(grid.GetTiles());
    }


    /* LoadCenterPattern
     * 
     * Loads the selected pattern at the grid center.
     * 
     * Parameters: None
     * 
     * Return: None
     */
    public void LoadCenterPattern()
    {
        ResetSimulation();

        Tile[,] tiles = grid.GetTiles();

        int centerX = tiles.GetLength(0) / 2;
        int centerY = tiles.GetLength(1) / 2;

        switch (currentPattern)
        {
            case StampPattern.Blinker:
                StampBlinker(tiles, centerX, centerY);
                break;

            case StampPattern.Glider:
                StampGlider(tiles, centerX, centerY);
                break;

            case StampPattern.Block:
                StampBlock(tiles, centerX, centerY);
                break;

            case StampPattern.None:
                Debug.Log("No pattern selected.");
                break;
        }
        UpdateTilemap(grid.GetTiles());
    }

    /* SetAliveChance
     * 
     * Updates aliveChance value and refreshes UI text.
     * 
     * connected with the Alive chance slider
     * 
     * Parameters: value
     * 
     * Return: None
     */
    public void SetAliveChance(float value)
    {
        aliveChance = Mathf.RoundToInt(value * 100f);
        aliveChanceText.text = "Alive chance: " + aliveChance.ToString() + "%";
    }

    /* SetRunSpeed
     * 
     * Changes simulation speed and updates UI text.
     * 
     * connected with the Run Speed slider
     * 
     * Parameters: value
     * 
     * Return: None
     */
    public void SetRunSpeed(float value)
    {
        if (value >= 1.99f)
        {
            isRunning = false;
            RunPauseButtonText.text = "Run";
            timer = 0f;
        }
        else
        {
            isRunning = true;
            RunPauseButtonText.text = "Pause";
            stepInterval = value;
            RunSpeedText.text = "Run Speed";
        }
    }

    /* ActivateStampTiles
     * 
     * Activates a tile if within grid bounds.
     * 
     * Parameters: tiles, x, y
     * 
     * Return: None
     */
    private void ActivateStampTiles(Tile[,] tiles, int x, int y)
    {
        // within boundary (actually it's a pointless condition since it's always placed at the center but maybe for the future features)
        if (x >= 0 && x < tiles.GetLength(0) && y >= 0 && y < tiles.GetLength(1))
        {
            tiles[x, y].ActivateTile();
        }
    }

    /* StampBlinker , Glider, Block
     * 
     * Places these patterns at the center.
     * 
     * Parameters: tiles, centerX, centerY
     * 
     * Return: None
     */
    private void StampBlinker(Tile[,] tiles, int centerX, int centerY)
    {
        ActivateStampTiles(tiles, centerX, centerY);
        ActivateStampTiles(tiles, centerX, centerY - 1);
        ActivateStampTiles(tiles, centerX, centerY + 1);
    }

    private void StampGlider(Tile[,] tiles, int centerX, int centerY)
    {
        ActivateStampTiles(tiles, centerX, centerY);
        ActivateStampTiles(tiles, centerX + 1, centerY);
        ActivateStampTiles(tiles, centerX + 1, centerY + 1);
        ActivateStampTiles(tiles, centerX, centerY - 1);
        ActivateStampTiles(tiles, centerX - 1, centerY + 1);
    }

    private void StampBlock(Tile[,] tiles, int centerX, int centerY)
    {
        ActivateStampTiles(tiles, centerX, centerY);
        ActivateStampTiles(tiles, centerX + 1, centerY);
        ActivateStampTiles(tiles, centerX, centerY + 1);
        ActivateStampTiles(tiles, centerX + 1, centerY + 1);
    }


    /* UpdateTilemap
     * 
     * Updates the Tilemap to match tile alive/dead states.(tilemap visual update)
     * 
     * Parameters: tiles
     * 
     * Return: None
     */
    private void UpdateTilemap(Tile[,] tiles)
    {
        tilemap.ClearAllTiles();

        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                if (tiles[x, y].IsAlive())
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), landTile);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), waterTile);
                }
            }
        }
    }

    /* SelectBlock
     * 
     * Sets the pattern to Block and loads it at center
     * 
     * Parameters: None
     * 
     * Return: None
     */
    public void SelectBlock()
    {
        currentPattern = StampPattern.Block;
        LoadCenterPattern();
    }

    /* SelectBlinker
     * 
     * Sets the pattern to Blinker and loads it at center
     * 
     * Parameters: None
     * 
     * Return: None
     */
    public void SelectBlinker()
    {
        currentPattern = StampPattern.Blinker;
        LoadCenterPattern();
    }

    /* SelectGlider
     * 
     * Sets the pattern to Glider and loads it at center.
     * 
     * Parameters: None
     * 
     * Return: None
     */
    public void SelectGlider()
    {
        currentPattern = StampPattern.Glider;
        LoadCenterPattern();
    }

    /* SetRuleMode
     * 
     * Changes between SingleRule and RuleSequence modes(Dropdown UI)
     * 
     * Parameters: value
     * 
     * Return: None
     */
    public void SetRuleMode(int value)
    {
        ruleMode = (RuleMode)value;
    }

    /* SetSingleRule
     * 
     * Selects a rule from ruleOptions for SingleRule mode.(Dropdown UI)
     * 
     * Parameters: index
     * 
     * Return: None
     */
    public void SetSingleRule(int index)
    {
        singleRule = ruleOptions[index];
    }
}
