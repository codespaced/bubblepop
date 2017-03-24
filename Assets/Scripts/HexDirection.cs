using System.Collections.Generic;
using UnityEngine;

public enum HexDirection
{

    N, NE, SE, S, SW, NW

}

public static class HexDirectionExtensions
{
    private static Dictionary<HexDirection, Vector3> dict = new Dictionary<HexDirection, Vector3>();

    private static void Init()
    {
        dict.Add(HexDirection.N, new Vector3(0, 0, 1));
        dict.Add(HexDirection.NE, new Vector3(1, 0, 0));
        dict.Add(HexDirection.SE, new Vector3(0, -1, 0));
        dict.Add(HexDirection.S, new Vector3(0, 0, -1));
        dict.Add(HexDirection.SW, new Vector3(-1, 0, 0));
        dict.Add(HexDirection.NW, new Vector3(0, 1, 0));
    }

    public static Vector3 Move(this HexDirection direction)
    {
        return dict[direction];
    }

    public static HexDirection Opposite(this HexDirection direction)
    {

        return (int)direction < 3 ? (direction + 3) : (direction - 3);

    }

}