 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour
{

    public HexCoordinates Coordinates;

    public Color Color;

    public int Index;

    public bool Moving = false;

    [SerializeField]
    HexCell[] _neighbors;

    public void ChangeColor(Color color)
    {
        this.Color = color;
    }

    public HexCell GetNeighbor(HexDirection direction)
    {

        return _neighbors[(int)direction];

    }
    public void SetNeighbor(HexDirection direction, HexCell cell)
    {

        _neighbors[(int)direction] = cell;

        cell._neighbors[(int)direction.Opposite()] = this;

    }

}
