using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private int x;
    [SerializeField] private int z;
    [SerializeField] private int range;
    [SerializeField] private int exp;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            InventoryManager.Instance.AddPotionsTesting();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            InventoryManager.Instance.ChangeGoldAmount(5);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            PlayerStats.Instance.ChangeExp(exp);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    GridPosition center = new GridPosition(x, z);

    //    List<GridPosition> positions =
    //        HexRangeUtils.GetGridPositionsInRange(center, range);

    //    //positions.RemoveAll(p => !LevelGrid.Instance.IsValidGridPosition(p));

    //    foreach (var p in positions)
    //    {
    //        float offsetX = (p.z % 2 != 0) ? 0.5f : 0f;

    //        Vector3 worldPos = new Vector3(
    //            p.x + offsetX,
    //            0f,
    //            p.z * 0.75f   // same factor as HEX_VERTICAL_OFFSET_MULTIPLIER
    //        );

    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireSphere(worldPos, 0.25f);
    //    }

    //    // Center
    //    float centerOffsetX = (center.z % 2 != 0) ? 0.5f : 0f;
    //    Vector3 centerWorldPos = new Vector3(
    //        center.x + centerOffsetX,
    //        0f,
    //        center.z * 0.75f
    //    );

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawSphere(centerWorldPos, 0.3f);
    //}
}
