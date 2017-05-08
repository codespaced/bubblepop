using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : Singleton<HexGrid>
{

    protected HexGrid() { }

    public int Width = 6;
    public int Height = 6;

    public Bubble BubblePrefab;

    public Bubble[] Bubbles;
    public HexCell[] Hexcells;
    public Emitter[] Emitters;
    public Text CellLabelPrefab;
    public HexCell HexcellPrefab;
    public Emitter EmitterPrefab;
    public int Score = 0;
    public int Moves = 0;
    public ParticleSystem ParticlePrefab;
    public Queue<Point> Points;
    private ParticleSystem _particleSystem;

    public Color[] Colors = new Color[6];
    private Canvas _gridCanvas;
    private Text _popText;
    private Vector3 _popTextPosition;

    void Awake()
    {
        _gridCanvas = GetComponentInChildren<Canvas>();
        _popText = _gridCanvas.GetComponentInChildren<Text>();

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

        _particleSystem = Instantiate(ParticlePrefab, new Vector3(0, 15, 0), Quaternion.identity) as ParticleSystem;
        //_particleSettings = _particleSystem.main;
    }

    public void DoParticles(Transform tf, Color color)
    {
        var old = GetComponent(typeof(ParticleSystem));
        if (old != null)
        {
            old.SendMessage("Die");
        }
        _particleSystem.transform.position = tf.position + new Vector3(0, 25, 0);
        _particleSystem.transform.localScale = transform.localScale;

        _particleSystem.Play();
    }

    public void DoScoring(Transform tf, Color color, string points)
    {
        StartCoroutine(ShowMessage(points, color, tf, 1));
    }

    IEnumerator ShowMessage(string message, Color color, Transform tf, float delay)
    {

        _popTextPosition = tf.position + new Vector3(0, 25, 0);
        _popText.text = message;
        _popText.material.color = color;
        _popText.transform.position = _popTextPosition;
        _popText.enabled = true;
        yield return new WaitForSeconds(delay);
        if (_popText.transform.position == _popTextPosition)
        {
            _popText.enabled = false;
        }
        Console.WriteLine(_popTextPosition.ToString());

    }

    void Start()
    {
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
        //Debug.Log("touched at " + ray.origin + " : " + hit.point);
        //Debug.DrawLine(ray.origin, hit.point, Color.red);
    }

    private void TouchCell(RaycastHit hit)
    {
        if (hit.collider != null)
        {
            var obj = hit.collider.gameObject;
            obj.SendMessage("Pop");
        }
    }


    public class Point
    {
        public Transform Transform;
        public String Text;
        public Color Color;
    }

}