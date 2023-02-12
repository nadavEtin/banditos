using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(fileName = "GameParams", menuName = "ScriptableObjects/Game params")]
public class GameParams : ScriptableObject
{
    public int RowAmnt, ColumnAmnt;
    public Vector3Int PlayerStartPos, ExitPos;
    [Range(1, 5)]
    public int StarTiles;
    [Range(1, 5)]
    public int LavaTiles;
    [Range(1, 5)]
    public int WallTiles;
}
