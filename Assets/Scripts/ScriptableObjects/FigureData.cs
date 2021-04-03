using UnityEngine;
using System;
using Grid = CustomGrid.Grid;
using System.Collections.Generic;

public enum FigureOwner
{
    Pale,
    Dark
}
[Serializable]
public struct FigurePlaces
{
    public GameObject FigurePrefab;
    public FigureOwner Owner;
    public Vector2Int[] CellCoordinates;
}

[CreateAssetMenu(fileName = "FigureData", menuName = "Data/Figure")]
public class FigureData : ScriptableObject
{
    public Color Pale;
    public Color Dark;
    public FigurePlaces[] FigurePlaces;

    public Color PickColor(FigureOwner owner)
    {
        switch (owner)
        {
            case FigureOwner.Pale:
                return Pale;
            case FigureOwner.Dark:
                return Dark;
            default:
                return Dark;
        }
    }
}
