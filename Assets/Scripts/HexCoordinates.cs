using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{
    [SerializeField]
    private int x, z;

    public int X
    {
        get
        {
            return x;
        }
    }

    public int Z
    {
        get
        {
            return z;
        }
    }
    public int Y
    {
        get
        {
            return -X - Z;
        }
    }

    public HexCoordinates(int x, int z) : this()
    {
        this.x = x;
        this.z = z;
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new HexCoordinates(x, z - x / 2);
    }

    public override string ToString()
    {
        return "(" +
            X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }

    public static HexCoordinates FromPosition(Vector3 position)
    {



        float x = position.x / (HexMetrics.outerRadius * 1.5f);
        float z = position.z / (HexMetrics.innerRadius * 2f);
        float y = -position.z /(HexMetrics.outerRadius * 3f);

        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(z);
        

        if (iX + iY + iZ != 0)
        {
            Debug.Log("Rounding Error: " + position.x.ToString() + ", " + position.y.ToString() + ", " + position.z.ToString() + " : " + iX + ", " + iY + ", " + iZ);
            //float dX = Mathf.Abs(x - iX);
            //float dY = Mathf.Abs(y - iY);
            //float dZ = Mathf.Abs(z - iZ);

            //if (dX > dY && dX > dZ)
            //{
            //    iX = -iY - iZ;
            //}
            //else if (dZ > dY)
            //{
            //    iZ = -iX - iY;
            //}
        }
        return new HexCoordinates(iX, iZ);
    }

    public Transform transform;
    public void TouchCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        Debug.Log("touched at " + coordinates.ToString() + " : " + position.x.ToString() + ", " + position.z.ToString());
    }

}