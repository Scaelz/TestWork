using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Pawn : MonoBehaviour, IFigure
{
    private SpriteRenderer _spriteRenderer;

    public FigureOwner Owner { get; private set; }

    public void Initialize(FigureOwner owner, Color color)
    {
        _spriteRenderer.color = color;
        Owner = owner;
    }
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
