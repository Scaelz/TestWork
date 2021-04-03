using UnityEngine;

public interface IFigure : IComponent
{
    FigureOwner Owner { get; }
    void Initialize(FigureOwner owner, Color color);
}
