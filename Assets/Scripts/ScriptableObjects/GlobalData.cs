using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalData", menuName = "Data/Global")]
public class GlobalData : ScriptableObject
{
    public string GridDataPath;
    private GridData _gridData;
    public GridData GridData
    {
        get
        {
            if (_gridData == null)
            {
                _gridData = Load<GridData>("Data/" + GridDataPath);
            }
            return _gridData;
        }
    }

    public string CellDataPath;
    private CellData _cellData;
    public CellData CellData
    {
        get
        {
            if (_cellData == null)
            {
                _cellData = Load<CellData>("Data/" + CellDataPath);
            }
            return _cellData;
        }
    }
    public string FigureDataPath;
    private FigureData _figureData;
    public FigureData FigureData
    {
        get
        {
            if (_figureData == null)
            {
                _figureData = Load<FigureData>("Data/" + FigureDataPath);
            }
            return _figureData;
        }
    }

    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(Path.ChangeExtension(path, null));
    }
}
