using UnityEngine;
using UnityEngine.Tilemaps;

public class ReferenceManager : MonoBehaviour
{
    public Tilemap groundTileMap;
    public Grid grid;

    private static ReferenceManager instance;
    public static ReferenceManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }
}