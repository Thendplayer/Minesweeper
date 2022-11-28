using UnityEngine;

namespace Minesweeper.Board
{
    public class BoardModel
    {
        public readonly Cell[,] Cells;
        public readonly Vector2Int BoardSize;
        public readonly int MineCount;

        public BoardModel(Vector2Int boardSize, int mineCount)
        {
            BoardSize = boardSize;
            Cells = new Cell[boardSize.x, boardSize.y];
            
            for (var x = 0; x < boardSize.x; x++)
            {
                for (var y = 0; y < boardSize.y; y++)
                {
                    Cells[x, y] = new Cell(x, y);
                }
            }
            
            MineCount = mineCount;
            
            SetMines();
            SetNumbers();
        }
        
        public Cell GetCell(int x, int y)
        {
            return IsValid(x, y) ? Cells[x, y] : new Cell(x, y, true);
        }
        
        private void SetMines()
        {
            for (var i = 0; i < MineCount; i++)
            {
                var x = Random.Range(0, BoardSize.x);
                var y = Random.Range(0, BoardSize.y);

                while (Cells[x, y].Type == Cell.CellType.Mine)
                {
                    x++;

                    if (x < BoardSize.x) continue;
                    x = 0;
                    
                    y++;

                    if (y < BoardSize.y) continue;
                    y = 0;
                }

                Cells[x, y].SetType(Cell.CellType.Mine);
            }
        }
        
        private void SetNumbers()
        {
            for (var x = 0; x < BoardSize.x; x++)
            {
                for (var y = 0; y < BoardSize.y; y++)
                {
                    if (Cells[x, y].Type == Cell.CellType.Mine) continue;
                    
                    var minesCount = GetAdjacentMinesCount(x, y);
                    if (minesCount <= 0) continue;
                    
                    Cells[x, y].SetType(Cell.CellType.Number, minesCount);
                }
            }
        }
        
        private int GetAdjacentMinesCount(int cellX, int cellY)
        {
            var count = 0;

            for (var adjacentX = -1; adjacentX <= 1; adjacentX++)
            {
                for (var adjacentY = -1; adjacentY <= 1; adjacentY++)
                {
                    if (adjacentX == 0 && adjacentY == 0) continue;

                    var x = cellX + adjacentX;
                    var y = cellY + adjacentY;

                    if (GetCell(x, y).Type != Cell.CellType.Mine) continue;
                    
                    count++;
                }
            }

            return count;
        }

        private bool IsValid(int x, int y)
        {
            return x >= 0 && x < BoardSize.x && y >= 0 && y < BoardSize.y;
        }
        
        public void Flood(Cell cell)
        {
            if (cell.State == Cell.CellState.Revealed || cell.Type == Cell.CellType.Mine || cell.IsInvalid) return;

            cell.SetState(Cell.CellState.Revealed);
            Cells[cell.Position.x, cell.Position.y] = cell;

            if (cell.Type != Cell.CellType.Empty) return;
            
            Flood(GetCell(cell.Position.x - 1, cell.Position.y));
            Flood(GetCell(cell.Position.x + 1, cell.Position.y));
            Flood(GetCell(cell.Position.x, cell.Position.y - 1));
            Flood(GetCell(cell.Position.x, cell.Position.y + 1));
        }
        
        public void RevealMines()
        {
            for (var x = 0; x < BoardSize.x; x++)
            {
                for (var y = 0; y < BoardSize.y; y++)
                {
                    var cell = Cells[x, y];

                    if (cell.Type != Cell.CellType.Mine) continue;
                    cell.SetState(Cell.CellState.Revealed);
                }
            }
        }
    }
}