using UnityEngine;

public static class HexMetrics
{

    public const float outerRadius = 10f;

    public const float innerRadius = outerRadius * 0.866025404f;

    static Vector3[] corners = {
        new Vector3(-outerRadius, 0f, 0f),
        new Vector3(0.5f * -outerRadius, 0f, innerRadius),
        new Vector3(0.5f * outerRadius, 0f, innerRadius),
        new Vector3(outerRadius, 0f, 0f),
        new Vector3(0.5f * outerRadius, 0f, -innerRadius),
        new Vector3(0.5f * -outerRadius, 0f, -innerRadius),
        new Vector3(-outerRadius, 0f, 0f)
    };

        public static Vector3 GetFirstCorner(HexDirection direction)
        {
            return corners[(int)direction];
        }

        public static Vector3 GetSecondCorner(HexDirection direction)
        {
            return corners[(int)direction + 1];
        }
}