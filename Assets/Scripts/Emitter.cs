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
	void Update () {
	    if (HexGrid.Instance.Bubbles[Neighbor.Index] == null)
	    {
	        Count++;
	        if (Count == 10)
	        {
	            HexGrid.Instance.CreateBubble(Index, 0, Index);
                Count = 0;
            }
	    }
	}
}
