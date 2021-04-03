using UnityEngine;

namespace CustomGrid
{
    public class Grid
    {
        private GridData _gridData;
        private ICell[,] _gridObjects;
        readonly public int Width;
        readonly public int Height;
        readonly public float CellSize;
        public Grid(GridData gridData)
        {
            _gridObjects = new ICell[gridData.Width, gridData.Height];
            _gridData = gridData;
            Width = _gridData.Width;
            Height = _gridData.Height;
            CellSize = _gridData.CellSize;
        }

        public Vector2Int GetCoordinates(Vector2 worldPosition)
        {
            var x = Mathf.FloorToInt(worldPosition.x / _gridData.CellSize);
            var y = Mathf.FloorToInt(worldPosition.y / _gridData.CellSize);

            return new Vector2Int(x, y);
        }

        public Vector2 GetWorldPostion(int x, int y)
        {
            return new Vector2(x, y) * _gridData.CellSize + new Vector2(CellSize, CellSize) * 0.5f;
        }

        public void SetValue(int x, int y, ICell value)
        {
            _gridObjects[x, y] = value;
        }

        public void SetValue(Vector2 worldPosition, ICell value)
        {
            var gridCoordinates = GetCoordinates(worldPosition);
            SetValue(gridCoordinates.x, gridCoordinates.y, value);
        }

        public ICell GetValue(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return default;
            }
            return _gridObjects[x, y];
        }

        public ICell GetValue(Vector2 worldPosition)
        {
            var coordinates = GetCoordinates(worldPosition);
            return GetValue(coordinates.x, coordinates.y);
        }
    }

}
