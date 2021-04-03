using UnityEngine;

public interface ICell : IComponent
{
    Vector2 Coordinates { get; set; }
    IFigure Figure { get; set; }

    Color DefaultColor { get; }
    void Initialize(Color color, Vector2 coordinates);
    void SetColor(Color color);
}
