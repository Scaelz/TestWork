using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Standard2dCell : MonoBehaviour, ICell
{
    private SpriteRenderer _spriteRenderer;
    public Vector2 Coordinates { get; set; }
    public IFigure Figure { get; set; }
    public Color DefaultColor { get; private set; }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Color color, Vector2 coordinates)
    {
        DefaultColor = color;
        SetColor(DefaultColor);
        Coordinates = coordinates;
    }

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }
}
