using UnityEngine;

public class GridObstacle : MonoBehaviour
{
    private void Start()
    {
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }
}