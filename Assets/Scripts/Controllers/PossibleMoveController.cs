using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = CustomGrid.Grid;
using EventBusSystem;

public interface IPossiblemoveChecker
{
    List<ICell> GetPossibleMoves(IFigure figure, ICell cell);
}

public class PossibleMoveController : ICleanupable, IPossiblemoveChecker, ITurnChangedHandler
{
    private Grid _grid;
    private Rules _sessionRules;
    private Turn _currentTurn;

    public PossibleMoveController(Grid grid, Rules rules)
    {
        _grid = grid;
        _sessionRules = rules;
        EventBus.Subscribe(this);
    }
    
    public void Cleanup()
    {
        EventBus.Unsubscribe(this);
    }

    public List<ICell> GetPossibleMoves(IFigure figure, ICell cell)
    {
        List<ICell> possibleCells = new List<ICell>();
        if (_currentTurn == Turn.Pale && figure.Owner != FigureOwner.Pale ||
        _currentTurn == Turn.Dark && figure.Owner != FigureOwner.Dark)
        {
            return possibleCells;
        }

        if (figure is Pawn pawn)
        {
            var startX = _grid.GetCoordinates(cell.Coordinates).x;
            var startY = _grid.GetCoordinates(cell.Coordinates).y;

            if (_sessionRules == Rules.DiagonalJump)
            {
                #region TopRight
                var coordinatesToCheck = new Vector2Int(startX + 2, startY + 2);
                if (coordinatesToCheck.x < _grid.Width && coordinatesToCheck.y < _grid.Height)
                {
                    var cellToCheck = _grid.GetValue(startX + 1, startY + 1);
                    var cellToMove = _grid.GetValue(coordinatesToCheck.x, coordinatesToCheck.y);

                    if (cellToCheck.Figure != null && cellToMove.Figure == null)
                    {
                        possibleCells.Add(_grid.GetValue(coordinatesToCheck));
                    }
                }
                #endregion
                #region Topleft
                coordinatesToCheck = new Vector2Int(startX - 2, startY + 2);
                if (coordinatesToCheck.x >= 0 && coordinatesToCheck.y < _grid.Height)
                {
                    var cellToCheck = _grid.GetValue(startX - 1, startY + 1);
                    var cellToMove = _grid.GetValue(coordinatesToCheck.x, coordinatesToCheck.y);
                    if (cellToCheck.Figure != null && cellToMove.Figure == null)
                    {
                        possibleCells.Add(_grid.GetValue(coordinatesToCheck));
                    }
                }
                #endregion
                #region BotRight
                coordinatesToCheck = new Vector2Int(startX + 2, startY - 2);
                if (coordinatesToCheck.x < _grid.Width && coordinatesToCheck.y >= 0)
                {
                    var cellToCheck = _grid.GetValue(startX + 1, startY - 1);
                    var cellToMove = _grid.GetValue(coordinatesToCheck.x, coordinatesToCheck.y);
                    if (cellToCheck.Figure != null && cellToMove.Figure == null)
                    {
                        possibleCells.Add(_grid.GetValue(coordinatesToCheck));
                    }
                }
                #endregion
                #region BotLeft
                coordinatesToCheck = new Vector2Int(startX - 2, startY - 2);
                if (coordinatesToCheck.x >= 0 && coordinatesToCheck.y >= 0)
                {
                    var cellToCheck = _grid.GetValue(startX - 1, startY - 1);
                    var cellToMove = _grid.GetValue(coordinatesToCheck.x, coordinatesToCheck.y);
                    if (cellToCheck.Figure != null && cellToMove.Figure == null)
                    {
                        possibleCells.Add(_grid.GetValue(coordinatesToCheck));
                    }
                }
                #endregion
            }
            else if (_sessionRules == Rules.HorizontalVerticalJump)
            {
                #region Top
                var coordinatesToCheck = new Vector2Int(startX, startY + 2);
                if (coordinatesToCheck.y < _grid.Height)
                {
                    var cellToCheck = _grid.GetValue(startX, startY + 1);
                    var cellToMove = _grid.GetValue(coordinatesToCheck.x, coordinatesToCheck.y);

                    if (cellToCheck.Figure != null && cellToMove.Figure == null)
                    {
                        possibleCells.Add(_grid.GetValue(coordinatesToCheck));
                    }
                }
                #endregion
                #region Right
                coordinatesToCheck = new Vector2Int(startX + 2, startY);
                if (coordinatesToCheck.x < _grid.Width)
                {
                    var cellToCheck = _grid.GetValue(startX + 1, startY);
                    var cellToMove = _grid.GetValue(coordinatesToCheck.x, coordinatesToCheck.y);

                    if (cellToCheck.Figure != null && cellToMove.Figure == null)
                    {
                        possibleCells.Add(_grid.GetValue(coordinatesToCheck));
                    }
                }
                #endregion
                #region Bot
                coordinatesToCheck = new Vector2Int(startX, startY - 2);
                if (coordinatesToCheck.y >= 0)
                {
                    var cellToCheck = _grid.GetValue(startX, startY - 1);
                    var cellToMove = _grid.GetValue(coordinatesToCheck.x, coordinatesToCheck.y);

                    if (cellToCheck.Figure != null && cellToMove.Figure == null)
                    {
                        possibleCells.Add(_grid.GetValue(coordinatesToCheck));
                    }
                }
                #endregion
                #region Left
                coordinatesToCheck = new Vector2Int(startX - 2, startY);
                if (coordinatesToCheck.x >= 0)
                {
                    var cellToCheck = _grid.GetValue(startX - 1, startY);
                    var cellToMove = _grid.GetValue(coordinatesToCheck.x, coordinatesToCheck.y);

                    if (cellToCheck.Figure != null && cellToMove.Figure == null)
                    {
                        possibleCells.Add(_grid.GetValue(coordinatesToCheck));
                    }
                }
                #endregion
            }
            else if (_sessionRules == Rules.AllDirections)
            {
                #region Top
                var coordinatesToCheck = new Vector2Int(startX, startY + 1);
                if (coordinatesToCheck.y < _grid.Height)
                {
                    var cellToMove = _grid.GetValue(coordinatesToCheck.x, coordinatesToCheck.y);

                    if (cellToMove.Figure == null)
                    {
                        possibleCells.Add(_grid.GetValue(coordinatesToCheck));
                    }
                }
                #endregion
                #region TopRight
                coordinatesToCheck = new Vector2Int(startX + 1, startY + 1);
                if (coordinatesToCheck.x < _grid.Width && coordinatesToCheck.y < _grid.Height)
                {
                    var cellToMove = _grid.GetValue(coordinatesToCheck.x, coordinatesToCheck.y);

                    if (cellToMove.Figure == null)
                    {
                        possibleCells.Add(_grid.GetValue(coordinatesToCheck));
                    }
                }
                #endregion
                #region Right
                coordinatesToCheck = new Vector2Int(startX + 1, startY);
                if (coordinatesToCheck.x < _grid.Width)
                {
                    var cellToMove = _grid.GetValue(coordinatesToCheck.x, coordinatesToCheck.y);

                    if (cellToMove.Figure == null)
                    {
                        possibleCells.Add(_grid.GetValue(coordinatesToCheck));
                    }
                }
                #endregion
                #region BotRight
                coordinatesToCheck = new Vector2Int(startX + 1, startY - 1);
                if (coordinatesToCheck.x < _grid.Width && coordinatesToCheck.y >= 0)
                {
                    var cellToMove = _grid.GetValue(coordinatesToCheck.x, coordinatesToCheck.y);

                    if (cellToMove.Figure == null)
                    {
                        possibleCells.Add(_grid.GetValue(coordinatesToCheck));
                    }
                }
                #endregion
                #region Bot
                coordinatesToCheck = new Vector2Int(startX, startY - 1);
                if (coordinatesToCheck.y >= 0)
                {
                    var cellToMove = _grid.GetValue(coordinatesToCheck.x, coordinatesToCheck.y);

                    if (cellToMove.Figure == null)
                    {
                        possibleCells.Add(_grid.GetValue(coordinatesToCheck));
                    }
                }
                #endregion
                #region BotLeft
                coordinatesToCheck = new Vector2Int(startX - 1, startY - 1);
                if (coordinatesToCheck.x >= 0 && coordinatesToCheck.y >= 0)
                {
                    var cellToMove = _grid.GetValue(coordinatesToCheck.x, coordinatesToCheck.y);

                    if (cellToMove.Figure == null)
                    {
                        possibleCells.Add(_grid.GetValue(coordinatesToCheck));
                    }
                }
                #endregion
                #region Left
                coordinatesToCheck = new Vector2Int(startX - 1, startY);
                if (coordinatesToCheck.x >= 0)
                {
                    var cellToMove = _grid.GetValue(coordinatesToCheck.x, coordinatesToCheck.y);

                    if (cellToMove.Figure == null)
                    {
                        possibleCells.Add(_grid.GetValue(coordinatesToCheck));
                    }
                }
                #endregion
                #region TopLeft
                coordinatesToCheck = new Vector2Int(startX - 1, startY + 1);
                if (coordinatesToCheck.x >= 0 && coordinatesToCheck.y < _grid.Height)
                {
                    var cellToMove = _grid.GetValue(coordinatesToCheck.x, coordinatesToCheck.y);

                    if (cellToMove.Figure == null)
                    {
                        possibleCells.Add(_grid.GetValue(coordinatesToCheck));
                    }
                }
                #endregion
            }
        }
        return possibleCells;
    }

    public void OnTurnChangedHandler(Turn turn)
    {
        _currentTurn = turn;
    }
}
