using System.Collections.Generic;
using UnityEngine;

public static class HexRangeUtils
{
    private struct Cube
    {
        public int x;
        public int y;
        public int z;

        public Cube(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    // Offset ODD-R to the right
    private static Cube OffsetToCube(GridPosition p)
    {
        int cx = p.x - (p.z - (p.z & 1)) / 2;
        int cz = p.z;
        int cy = -cx - cz;
        return new Cube(cx, cy, cz);
    }

    private static GridPosition CubeToOffset(Cube c)
    {
        int x = c.x + (c.z - (c.z & 1)) / 2;
        int z = c.z;
        return new GridPosition(x, z);
    }

    /// <summary>
    /// Return all the GridPositions inside range of a real hex
    /// (ODD-R, optimized)
    /// </summary>
    public static List<GridPosition> GetGridPositionsInRange(
        GridPosition center,
        int range
    )
    {
        List<GridPosition> result = new List<GridPosition>(
            3 * range * (range + 1) + 1
        );

        Cube centerCube = OffsetToCube(center);

        for (int dx = -range; dx <= range; dx++)
        {
            int minDy = Mathf.Max(-range, -dx - range);
            int maxDy = Mathf.Min(range, -dx + range);

            for (int dy = minDy; dy <= maxDy; dy++)
            {
                int dz = -dx - dy;

                Cube cube = new Cube(
                    centerCube.x + dx,
                    centerCube.y + dy,
                    centerCube.z + dz
                );

                result.Add(CubeToOffset(cube));
            }
        }

        return result;
    }
}
