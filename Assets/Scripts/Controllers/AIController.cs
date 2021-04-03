using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBusSystem;

public class AIController : ICleanupable, ITurnChangedHandler
{
    private List<ICell> _targetCells;
    private List<ICell> _aiPopulatedCells;
    private IPossiblemoveChecker _moveChecker;

    public AIController(List<ICell> targetCells, List<ICell> aiPopulatedCells, IPossiblemoveChecker moveChecker)
    {
        EventBus.Subscribe(this);
        _targetCells = targetCells;
        _aiPopulatedCells = aiPopulatedCells;
        _moveChecker = moveChecker;
    }

    public void Cleanup()
    {
        EventBus.Unsubscribe(this);
    }

    public void OnTurnChangedHandler(Turn turn)
    {
        if (Turn.Dark == turn)
        {
            float moveDistance = 99999;
            ICell fromCell = null;
            ICell toCell = null;

            Dictionary<ICell, List<ICell>> possibleToMoveCells = new Dictionary<ICell, List<ICell>>();
            foreach (var aiCell in _aiPopulatedCells)
            {
                var possibleMoves = _moveChecker.GetPossibleMoves(aiCell.Figure, aiCell);
                if (possibleMoves.Count > 0)
                {
                    possibleToMoveCells.Add(aiCell, possibleMoves);
                }
            }

            var keys = possibleToMoveCells.Keys.ToArray();

            var randomCell = keys[Random.Range(0, keys.Length)];

            foreach (var possibleCell in possibleToMoveCells[randomCell])
            {
                foreach (var targetCell in _targetCells)
                {
                    var distance = Vector3.Distance(possibleCell.transform.position,
                                                      targetCell.transform.position);
                    if (distance < moveDistance)
                    {
                        moveDistance = distance;
                        fromCell = randomCell;
                        toCell = possibleCell;
                    }
                }

            }
            if (fromCell != null && toCell != null)
            {
                var args = new FigureInteractionArgs()
                {
                    Figure = fromCell.Figure,
                    isCanceled = false,
                    StartCell = fromCell,
                    PlacementCell = toCell
                };
                toCell.Figure = fromCell.Figure;
                toCell.Figure.transform.position = toCell.Coordinates;
                fromCell.Figure = null;
                _aiPopulatedCells.Remove(fromCell);
                _aiPopulatedCells.Add(toCell);
                EventBus.RaiseEvent<IFigurePlacedHandler>(x => x.FigurePlacedHandler(args));
            }
        }
    }
}
