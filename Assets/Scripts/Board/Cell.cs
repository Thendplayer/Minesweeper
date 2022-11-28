using UnityEngine;

namespace Minesweeper.Board
{
    public class Cell
    {
        public enum CellType
        {
            Empty,
            Mine,
            Number
        }
        
        public enum CellState
        {
            Intact,
            Revealed,
            Flagged,
            Exploded
        }
        
        public readonly Vector2Int Position;
        public readonly bool IsInvalid;
        
        private CellState state;
        private CellType type;
        private int number;

        public CellState State => state;
        public CellType Type => type;
        public int Number => number;
        
        public Cell(int posX, int posY, bool invalidCell = false)
        {
            Position = new Vector2Int(posX, posY);
            IsInvalid = invalidCell;
            
            state = CellState.Intact;
            type = CellType.Empty;
        }
        
        public void SetState(CellState state)
        {
            this.state = state;
        }
        
        public void SetType(CellType type, int number = 0)
        {
            this.type = type;
            this.number = number;
        }
    }
}