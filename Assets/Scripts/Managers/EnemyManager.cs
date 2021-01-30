using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public MovementManager movementManager;
    public PositionManager positionManager;
    public Grid grid;

    public Vector3Int CalculateNextMove(List<Vector3Int> movementOptions, Vector3 towards)
    {
        var distances = new List<(int, float)>();

        for (int i = 0; i < movementOptions.Count; i++) {
            distances.Add((
                i,
                Vector3.Distance(grid.CellToWorld(movementOptions[i]), towards)
            ));
        }

        distances.Sort((x, y) => x.Item2.CompareTo(y.Item2));

        for (int i = 0; i < distances.Count; i++)
        {
            Debug.Log(distances[i]);
        }

        return movementOptions[distances[0].Item1];
    }
}
