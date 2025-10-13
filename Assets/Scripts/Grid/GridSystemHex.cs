using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemHex<TGridObject>
{
    private const float HEX_VERTICAL_OFFSET_MULTIPLIER = .75f;
    private const float HEX_HORIZONTAL_OFFSET_MULTIPLIER = .5f;

    private int width;
    private int height;
    private float cellSize;
    private TGridObject[,] gridObjectArray;

    /// <summary>
    /// Constructor which creates a grid of size <see href="width"/> by <see href="height"/>
    /// where each cell of size <see href="cellSize"/> has a new <see cref="TGridObject"/> asigned 
    /// and added to the <see href="gridObjectArray"/> of this class
    /// </summary>
    /// <param name="width">Width of the grid</param>
    /// <param name="height">Height of the grid</param>
    /// <param name="cellSize">Size of the cell</param>
    public GridSystemHex(int width, int height, float cellSize, Func<GridSystemHex<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjectArray = new TGridObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x, z] = createGridObject(this, gridPosition);
            }
        }
    }

    /// <summary>
    /// Returns the world position of the given grid position (x, z index)
    /// </summary>
    /// <param name="gridPosition">Grid position (x, z index)</param>
    /// <returns><see cref="Vector3"/></returns>
    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return
            new Vector3(gridPosition.x, 0, 0) * cellSize +
            new Vector3(0, 0, gridPosition.z) * cellSize * HEX_VERTICAL_OFFSET_MULTIPLIER +
                (((gridPosition.z % 2) == 0) ? Vector3.zero : HEX_HORIZONTAL_OFFSET_MULTIPLIER * cellSize * new Vector3(1, 0, 0));
    }

    /// <summary>
    /// Returns the grid position (x, z index) of the given world position
    /// </summary>
    /// <param name="worldPosition">World position</param>
    /// <returns><see cref="GridPosition"/></returns>
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        GridPosition roughXZ = new GridPosition(
            Mathf.RoundToInt(worldPosition.x / cellSize),    
            Mathf.RoundToInt(worldPosition.z / cellSize / HEX_VERTICAL_OFFSET_MULTIPLIER)
        );

        bool oddRow = roughXZ.z % 2 != 0;

        List<GridPosition> neighbourGridPositionList = new List<GridPosition>
        {
            roughXZ + new GridPosition(-1, 0),
            roughXZ + new GridPosition(+1, 0),

            roughXZ + new GridPosition(0, +1),
            roughXZ + new GridPosition(0, -1),

            roughXZ + new GridPosition(oddRow ? +1 : -1, +1),
            roughXZ + new GridPosition(oddRow ? +1 : -1, -1)
        };

        GridPosition closestGridPosition = roughXZ;

        foreach (GridPosition neighbourGridPosition in neighbourGridPositionList)
        {
            if (Vector3.Distance(worldPosition, GetWorldPosition(neighbourGridPosition)) <
                Vector3.Distance(worldPosition, GetWorldPosition(closestGridPosition)))
                // Closer than the closest
            {
                closestGridPosition = neighbourGridPosition;
            }
        }

        return closestGridPosition;
    }

    /// <summary>
    /// Instantiates a debug prefab on each grid object (cell) of the grid
    /// </summary>
    /// <param name="debugPrefab">Debug prefab to instantiate</param>
    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    /// <summary>
    /// Returns the corresponding grid object of the given grid position
    /// </summary>
    /// <param name="gridPosition">Grid position (x, z index)</param>
    /// <returns><see cref="TGridObject"/></returns>
    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 && 
               gridPosition.z >= 0 && 
               gridPosition.x < width &&
               gridPosition.z < height;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }
}


