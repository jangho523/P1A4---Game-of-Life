using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // --- Simulation Control ---
    private bool isRunning = false;
    [SerializeField]
    private float stepInterval = 0.5f;
    private float timer = 0f;

    private int generation = 0;

    // Start is called before the first frame update
    void Start()
    {
        grid.CreateGrid2D(gridSizeX, gridSizeY);
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

        if (!isRunning)
        {
            timer = 0f;
        }
    }

    public void OneStep()
    {
        calculator.Initiate(grid.GetTiles());
        generation++;
    }

    public void ResetSimulation()
    {
        grid.ResetTiles();
        generation = 0;
        timer = 0f;
        isRunning = false;
    }
}
