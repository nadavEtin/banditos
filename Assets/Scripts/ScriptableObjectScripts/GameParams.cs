using UnityEngine;

[CreateAssetMenu(fileName = "GameParams", menuName = "ScriptableObjects/Game params")]
public class GameParams : ScriptableObject
{
    public int RowAmnt, ColumnAmnt, LevelMinXVal, LevelMinYVal, LevelMaxXVal, LevelMaxYVal;
    public Vector3Int PlayerStartPos, ExitPos;
}
