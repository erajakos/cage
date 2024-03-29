﻿using UnityEngine;
using UnityEngine.Tilemaps;

public class ReferenceManager : MonoBehaviour
{
    public Tilemap groundTileMap;
    public Grid grid;
    public MovementManager movementManager;
    public GridManager gridManager;
    public PositionManager positionManager;
    public StrategyManager strategyManager;

    private static ReferenceManager instance;
    public static ReferenceManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }
}