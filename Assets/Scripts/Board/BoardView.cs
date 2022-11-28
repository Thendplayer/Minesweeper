using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Minesweeper.Libraries;
using TMPro;
using UnityEngine.UI;

namespace Minesweeper.Board
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private BoardLibrary boardLibrary;
        [SerializeField] private Tilemap tilemap;
        
        [Space, SerializeField] private Image faceImage;
        [SerializeField] private TextMeshProUGUI mineCountText;
        [SerializeField] private TextMeshProUGUI timeLeftText;
        
        private const float camera_fix_z = -10f;
        private int currentMineCount = 0;
        private bool gameOver = false;

        public Camera MainCamera => mainCamera ? mainCamera : Camera.main;

        public void Setup(Vector2Int boardSize, int mineCount)
        {
            if (tilemap.size.x > 0 || tilemap.size.y > 0)
            {
                tilemap.ClearAllTiles();
            }

            gameOver = false;
            currentMineCount = mineCount;
            mineCountText.text = mineCount.ToString("000");
            faceImage.sprite = boardLibrary.SmilingFace;
            
            MainCamera.transform.position = new Vector3(boardSize.x / 2f, boardSize.y / 2f, camera_fix_z);
            StartCoroutine(UpdateTime());
        }

        public void DrawCell(Vector2Int position, Cell.CellState state, Cell.CellType type, int number)
        {
            var tile = state switch
            {
                Cell.CellState.Intact => boardLibrary.UnknownTile,
                
                Cell.CellState.Flagged => boardLibrary.FlagTile,
                
                Cell.CellState.Exploded => boardLibrary.ExplodedTile,
                
                Cell.CellState.Revealed => type switch
                {
                    Cell.CellType.Mine => boardLibrary.MineTile,
                
                    Cell.CellType.Number => boardLibrary.NumeratedTiles[number],
                    
                    _ => boardLibrary.EmptyTile
                },
                
                _ => boardLibrary.EmptyTile
            };

            tilemap.SetTile((Vector3Int)position, tile);
        }

        public Vector2Int WorldToCell(Vector3 position)
        {
            var worldPosition = MainCamera.ScreenToWorldPoint(position);
            var cellPosition = tilemap.WorldToCell(worldPosition);
            return (Vector2Int) cellPosition;
        }

        public void ShowGameOver(bool winner)
        {
            gameOver = true;
            faceImage.sprite = winner ? boardLibrary.CoolFace : boardLibrary.SadFace;
        }
        
        public void AddFlag()
        {
            if (currentMineCount > 0)
                currentMineCount--;
            
            mineCountText.text = currentMineCount.ToString("000");
        }

        public void RemoveFlag(int mineCount)
        {
            if (currentMineCount < mineCount)
                currentMineCount++;
            
            mineCountText.text = currentMineCount.ToString("000");
        }
        
        private IEnumerator UpdateTime()
        {
            var time = 0;
            timeLeftText.text = time.ToString("000");

            while(!gameOver)
            {
                yield return new WaitForSeconds(1f);
                if (gameOver) break;
                timeLeftText.text = (++time).ToString("000");
            }
        }
    }
}