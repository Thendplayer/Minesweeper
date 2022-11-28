using System;
using UnityEngine;
using UnityEngine.UI;

namespace Minesweeper
{
    public class UIManager : MonoBehaviour
    {
        [Serializable]
        public struct LevelButton
        {
            public string Id;
            public Button Button;
        }

        [Header("Screens")]
        [SerializeField] private RectTransform menuScreen;
        [SerializeField] private RectTransform gameScreen;
        [SerializeField] private GameObject grid;
        
        [Header("Buttons")]
        [SerializeField] private Button resetButton;
        [SerializeField] private LevelButton[] levelButtons;

        public void Start()
        {
            LoadMainMenu();
            
            foreach (var level in levelButtons)
            {
                level.Button.onClick.AddListener(() =>
                {
                    menuScreen.gameObject.SetActive(false);
                    gameScreen.gameObject.SetActive(true);
                    grid.SetActive(true);
                    
                    GameManager.Instance.LoadLevel(level.Id);
                });
            }
            
            resetButton.onClick.AddListener(() =>
            {
                GameManager.Instance.ReloadLevel();
            });
        }

        public void LoadMainMenu()
        {
            menuScreen.gameObject.SetActive(true);
            gameScreen.gameObject.SetActive(false);
            grid.SetActive(false);
        }
    }
}