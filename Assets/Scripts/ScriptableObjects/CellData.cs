using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CellData", menuName = "Data/Cell")]
public class CellData : ScriptableObject
{
    public GameObject CellPrefab;
    public Color Pale;
    public Color Dark;
    public Color Hover;
    public Color CanMove;
    public Color ErrorMove;
}
