using System.Threading.Tasks;
using UnityEngine;
using Minesweeper.Input;

namespace Minesweeper.Board
{
    public class BoardMediator
    {
        private readonly BoardModel model;
        private readonly BoardView view;

        public BoardMediator(
            BoardModel model,
            BoardView view
        )
        {
            this.model = model;
            this.view = view;
            
            view.Setup(model.BoardSize, model.MineCount);
            DrawAllCells();

            Task.Delay(200).ContinueWith(t => SubscribeEvents());
        }

        private void SubscribeEvents() {
            TouchEventHandler.OnTouchDetectedEvent += (id, position) => 
            {
                if (id == 0) Reveal(position);
                if (id == 1) Flag(position);
            };
        }
        
        private void DrawAllCells()
        {
            foreach (var cell in model.Cells)
            {
                view.DrawCell(cell.Position, cell.State, cell.Type, cell.Number);
            }
        }
        
        private void Reveal(Vector3 position)
        {
            var cellPosition = view.WorldToCell(position);
            var cell = model.GetCell(cellPosition.x, cellPosition.y);

            if (cell.State is Cell.CellState.Revealed or Cell.CellState.Flagged || cell.IsInvalid) return;

            switch (cell.Type)
            {
                case Cell.CellType.Mine:
                    model.RevealMines();
                    DrawAllCells();
                    
                    TouchEventHandler.ClearEvents();
                    view.ShowGameOver(false); //Lose condition
                    break;

                case Cell.CellType.Empty:
                    model.Flood(cell);
                    DrawAllCells();

                    CheckWinCondition();
                    break;

                default:
                    cell.SetState(Cell.CellState.Revealed);
                    view.DrawCell(cellPosition, cell.State, cell.Type, cell.Number);

                    CheckWinCondition();
                    break;
            }

            view.DrawCell(cell.Position, cell.State, cell.Type, cell.Number);
        }
        
        private void Flag(Vector3 position)
        {
            var cellPosition = view.WorldToCell(position);
            var cell = model.GetCell(cellPosition.x, cellPosition.y);

            if (cell.State is Cell.CellState.Revealed || cell.IsInvalid) return;

            if (cell.State is not Cell.CellState.Flagged)
            {
                cell.SetState(Cell.CellState.Flagged);
                view.AddFlag();
            }
            else
            {
                cell.SetState(Cell.CellState.Intact);
                view.RemoveFlag(model.MineCount);
            }

            view.DrawCell(cellPosition, cell.State, cell.Type, cell.Number);
        }
        
        private void CheckWinCondition()
        {
            foreach (var cell in model.Cells)
            {
                if (cell.Type != Cell.CellType.Mine && cell.State != Cell.CellState.Revealed)
                {
                    return;
                }
            }
            
            foreach (var cell in model.Cells)
            {
                if (cell.Type == Cell.CellType.Mine)
                {
                    cell.SetState(Cell.CellState.Flagged);
                }
            }
            
            TouchEventHandler.ClearEvents();
            DrawAllCells();
            view.ShowGameOver(true); //Win condition
        }
    }
}