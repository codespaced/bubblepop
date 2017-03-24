using System;
using System.Net;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : Singleton<HexGrid>
{
    //private static HexGrid _instance;

    protected HexGrid() {}

    public int Width = 6;
    public int Height = 6;

    public Bubble BubblePrefab;

    public Bubble[] Bubbles;
    public HexCell[] Hexcells;
    public Emitter[] Emitters;
    public Text CellLabelPrefab;
    public HexCell HexcellPrefab;
    public Emitter EmitterPrefab;

    public Color[] Colors = new Color[6];
    //private Canvas _gridCanvas;

    void Awake()
    {
        //_gridCanvas = GetComponentInChildren<Canvas>();

        Bubbles = new Bubble[Height * Width];
        Hexcells = new HexCell[Height * Width];

        for (int z = 0, i = 0; z < Height; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                CreateHexcell(x, z, i);
                CreateBubble(x, z, i++);
            }
        }

        Emitters = new Emitter[Width];
        for (int x = 0; x < Width; x++)
        {
            CreateEmitter(x);
        }
    }

    void Start()
    {
        //hexMesh.Triangulate(bubbles);
    }


    private void CreateEmitter(int x)
    {
        Vector3 position = Position(x, -1);

        Emitter emitter = Emitters[x] = Instantiate(EmitterPrefab);
        emitter.transform.SetParent(transform, false);
        emitter.transform.localPosition = position;
        emitter.Index = x;
        emitter.Neighbor = Hexcells[x];
    }

    public void CreateBubble(int x, int z, int i)
    {
        Vector3 position = Position(x, z);

        Bubble bubble = Bubbles[i] = Instantiate(BubblePrefab);
        bubble.transform.SetParent(transform, false);
        bubble.transform.localPosition = position;
        bubble.Coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        bubble.Index = i;
        bubble.ChangeColor(Colors[UnityEngine.Random.Range(0, Colors.Length)]);

        //Text label = Instantiate<Text>(cellLabelPrefab);
        //label.rectTransform.SetParent(gridCanvas.transform, false);
        //label.rectTransform.anchoredPosition =
        //    new Vector2(position.x, position.z);
        //label.text = bubble.coordinates.ToStringOnSeparateLines();
    }

    private void CreateHexcell(int x, int z, int i)
    {
        Vector3 position = Position(x, z);

        HexCell hexcell = Hexcells[i] = Instantiate(HexcellPrefab);
        hexcell.transform.SetParent(transform, false);
        hexcell.transform.localPosition = position;
        hexcell.Coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        hexcell.Index = i;

        SetNeighbors(x, z, i, hexcell);
    }

    private static Vector3 Position(int x, int z)
    {
        Vector3 position;
        position.x = x * (HexMetrics.outerRadius * 1.5f);
        position.y = 0f;
        position.z = (z + x * 0.5f - x / 2) * (HexMetrics.innerRadius * 2.0f);
        return position;
    }

    private void SetNeighbors(int x, int z, int i, HexCell hexcell)
    {
        if (x > 0)
        {
            if ((x & 1) == 0)
            {
                hexcell.SetNeighbor(HexDirection.NW, Hexcells[i - 1]);
            }
            else
            {
                hexcell.SetNeighbor(HexDirection.SW, Hexcells[i - 1]);
            }
        }
        if (z > 0)
        {
            hexcell.SetNeighbor(HexDirection.S, Hexcells[i - Width]);

            if ((x & 1) == 0)
            {
                hexcell.SetNeighbor(HexDirection.SE, Hexcells[i - Width + 1]);
                if (x > 0)
                {
                    hexcell.SetNeighbor(HexDirection.SW, Hexcells[i - Width - 1]);
                }
            }
        }
    }

    //public GameObject particle;
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            HandleInput();
        }
        //for (int i = 0; i < Input.touchCount; ++i)
        //{
        //    if (Input.GetTouch(i).phase == TouchPhase.Began)
        //    {

        //        // Construct a ray from the current touch coordinates
        //        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);

        //        RaycastHit hit;
        //        if (Physics.Raycast(ray, out hit))
        //        {
        //            TouchCell(hit);
        //        }

        //        Debug.Log("touched at " + ray.origin + " : " + hit.point);
        //        Debug.DrawLine(ray.origin, hit.point, Color.red);

        //    }
        //    Console.WriteLine("Phase: " + Input.GetTouch(i).phase);
        //}
    }
    private void HandleInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            {
                TouchCell(hit);
            }
        Debug.Log("touched at " + ray.origin + " : " + hit.point);
        Debug.DrawLine(ray.origin, hit.point, Color.red);
    }

    private void TouchCell(RaycastHit hit)
    {
        if (hit.collider != null)
        {
            var obj = hit.collider.gameObject;
            obj.SendMessage("Pop");
        }
    }

}