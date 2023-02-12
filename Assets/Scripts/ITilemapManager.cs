using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts
{
    public interface ITilemapManager
    {
        LevelTile GetTileAtPos(Vector3Int newPos);
        void Init(TileRefrences tileRefs, IUIManager uiManager, GameParams gameParams);
        void SetPlayerPos(Vector3Int newPos);

        void PlayerDied();
    }
}
