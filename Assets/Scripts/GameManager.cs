using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Grid2D grid;

    [SerializeField]
    private Calculator calculator;

    // --- Grid Settings ---
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

    // Start is called before the first frame update
    void Start()
    {
        grid.CreateGrid2D(gridSizeX, gridSizeY);
        SetCamera();
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
    }

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

    public void OneStep()
    {
        calculator.Initiate(grid.GetTiles());
        generation++;
        generationText.text = "Generation: " + generation.ToString();
    }

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
    }

    public void SetCamera()
    {
        float posX = (gridSizeX - 1) / 2f;
        float posY = (gridSizeY - 1) / 2f;

        mainCamera.transform.position = new Vector3(posX, posY, -10f);

        if(gridSizeX >= gridSizeY)
        {
            mainCamera.orthographicSize = (gridSizeX / 2f) + 3f;
        }
        else
        {
            mainCamera.orthographicSize = (gridSizeY / 2f) + 3f;
        }
    }

    public void GridSizeChange(float value)
    {
        int newSize = Mathf.RoundToInt(value);

        gridSizeX = newSize;
        gridSizeY = newSize;

        sizeText.text = "Grid Size: " + newSize.ToString();

        RecreateGrid();
    }

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
    }

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
    }

    public void SetAliveChance(float value)
    {
        aliveChance = Mathf.RoundToInt(value * 100f);
        aliveChanceText.text = "Alive chance: " + aliveChance.ToString() + "%";
    }

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
}
