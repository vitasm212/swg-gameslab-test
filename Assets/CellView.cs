using System;
using UnityEngine;

public class CellView : MonoBehaviour
{
    public Action<int, int> onClickCell;
    [SerializeField] private int x;
    [SerializeField] private int y;

    public void Setup(int x, int y)
    {
        this.x = x;
        this.y = y;
        transform.localPosition = new Vector3(x * 2.56f, y * 2.56f, 0);
    }
    private void OnMouseDown()
    {
        onClickCell?.Invoke(x, y);
    }

}
