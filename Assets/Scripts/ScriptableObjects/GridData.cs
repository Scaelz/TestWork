using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GridData", menuName = "Data/Grid")]
public class GridData : ScriptableObject
{
    public int Width;
    public int Height;
    public float CellSize;
    public Vector2 BottomLeftCorner;
}
