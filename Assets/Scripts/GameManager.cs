using UnityEngine;
using Minesweeper.Board;
using Minesweeper.Utils;
using Minesweeper.Libraries;

namespace Minesweeper
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance = null;
        
        [SerializeField] private GameLibrary gameLibrary;
        [SerializeField] private BoardView boardView;

        private string currentLevel = "";

        private void Awake()
        {
            Instance = this;
        }

        public void ReloadLevel()
        {
            if (currentLevel == "") return;
            LoadLevel(currentLevel);
        }

        public void LoadLevel(string id)
        {
            TouchEventHandler.ClearEvents();
            currentLevel = id;
            
            var levelConfig = gameLibrary.GetLevel(id);
            var boardModel = new BoardModel(levelConfig.Size, levelConfig.MineCount);
            boardView.MainCamera.orthographicSize = levelConfig.CameraSize;
            new BoardMediator(boardModel, boardView);
        }
    }
}