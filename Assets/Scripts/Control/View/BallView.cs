using UnityEngine;

public class BallView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite = null;

    public void SetColor(Color color)
    {
        sprite.color = color;
    }

    public Color GetColor()
    {
        return sprite.color;
    }

    public void Setup()
    {

    }
}