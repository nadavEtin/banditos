using UnityEngine;

[CreateAssetMenu(fileName = "GameParams", menuName = "ScriptableObjects/Game params")]
public class GameParams : ScriptableObject
{
    public int LevelMinXVal, LevelMinYVal, LevelMaxXVal, LevelMaxYVal;
}
