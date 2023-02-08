using Assets.Scripts;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour, ITilemapManager
{
    [SerializeField] private Tilemap _tilemap;

    private TileRefrences _tileRefs;

    public void Init(TileRefrences tileRefs)
    {
        _tileRefs = tileRefs;
    }
}
