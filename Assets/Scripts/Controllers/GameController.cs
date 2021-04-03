using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Grid = CustomGrid.Grid;
using UnityEngine.SceneManagement;
using EventBusSystem;

public class GameController : MonoBehaviour, IGameOverHandler
{
    [SerializeField]
    private GlobalData _globalData;
    [SerializeField]
    private UIController _uiController;
    private Controllers _controllers;
    private Grid _grid;
    private Rules _gameRules = Rules.AllDirections;
    private bool _vsAi = false;
    private string _playerName;
    private void OnEnable()
    {
        _controllers = new Controllers();
        EventBus.Subscribe(this);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(this);
    }

    private void Initialize()
    {
        GenerateBoard();
        SetupPlayerInput();
        var possibleMoveController = new PossibleMoveController(_grid, _gameRules);
        _controllers.Add(possibleMoveController);
        SetupFigureAndCellInteractions(possibleMoveController);
        var initialCells = GetFiguresInitialCells();
        var gameoverController = new GameOverController(initialCells.dark, initialCells.pale);
        EnableAI(initialCells.pale, initialCells.dark, possibleMoveController);
        _controllers.Add(gameoverController);
        SetupTurnChange();
    }

    private void EnableAI(List<ICell> pale, List<ICell> dark,
                                IPossiblemoveChecker possibleMoveController)
    {
        if (_vsAi)
        {
            var aiController = new AIController(new List<ICell>(pale),
                                                new List<ICell>(dark),
                                                possibleMoveController);
            _controllers.Add(aiController);
        }
    }

    private (List<ICell> pale, List<ICell> dark) GetFiguresInitialCells()
    {
        var _paleCells = _globalData.FigureData.FigurePlaces
                                    .FirstOrDefault(x => x.Owner == FigureOwner.Pale)
                                    .CellCoordinates
                                    .Select(cell => _grid.GetValue(cell.x, cell.y))
                                    .ToList();
        var _darkCells = _globalData.FigureData.FigurePlaces
                                    .FirstOrDefault(x => x.Owner == FigureOwner.Dark)
                                    .CellCoordinates
                                    .Select(cell => _grid.GetValue(cell.x, cell.y))
                                    .ToList();
        return (_paleCells, _darkCells);
    }

    private void SetupTurnChange()
    {
        var turnController = new TurnController();
        _controllers.Add(turnController);
    }

    private void SetupFigureAndCellInteractions(IPossiblemoveChecker possibleMoveController)
    {
        var figurePicker = new FigureInteractionController(_grid, possibleMoveController);
        _controllers.Add(figurePicker);
        var cellHighlight = new CellHighlightController(_globalData.CellData, possibleMoveController);
        _controllers.Add(cellHighlight);
    }

    private void SetupPlayerInput()
    {
        var userInput = new PlayerInputController(Camera.main);
        _controllers.Add(userInput);
    }

    private void Update()
    {
        _controllers.Execute(Time.deltaTime);
    }

    void GenerateBoard()
    {
        _grid = new Grid(_globalData.GridData);
        var cellInstancer = new CellInstancer(_grid, _globalData.CellData);
        cellInstancer.InstantiateCells();
        var figureInstancer = new MiniGameFigureInstancer(_globalData.FigureData, _grid);
        figureInstancer.Instantiate();
    }

    public void ChangeRules(int id)
    {
        _gameRules = (Rules)id;
        _uiController.HidePreGameUI();
        Initialize();
    }

    public void RestartGame()
    {
        _controllers.Cleanup();
        SceneManager.LoadScene(0);
    }

    public void ToggleAiMode()
    {
        _vsAi = !_vsAi;
    }

    public void SetPlayerName()
    {
        _playerName = _uiController.GetUserName();
    }

    public void GameOverHandler(GameOverArgs args)
    {
        if (_playerName == "")
        {
            _playerName = "Pale";
        }
        if (args.IsPaleWon)
        {
            _uiController.ShowEndGameText($"{_playerName} is won!");
        }
        else
        {
            _uiController.ShowEndGameText("Dark is won!");
        }
    }
}
