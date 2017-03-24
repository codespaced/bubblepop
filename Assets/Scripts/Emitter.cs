using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour
{

    public int Index;
    public HexCell Neighbor;
    private int Count = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var bubble = HexGrid.Instance.Bubbles[Neighbor.Index];

	    if (bubble == null)
	    {
	        Count++;
	        if (Count == 10)
	        {
	            HexGrid.Instance.CreateBubble(Index, 0, Index);
	            Count = 0;
	        }
	    }
	    else
	    {
	        Count = 0;
	    }
	}
}
