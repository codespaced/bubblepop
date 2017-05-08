using System;
using UnityEngine;

namespace Assets.Scripts
{
  public class Bubble : MonoBehaviour
  {

    public HexCoordinates Coordinates;

    public Color Color;
    private static readonly Vector3 Origin = new Vector3(0, 0, 0);
    public Vector3 Target = Origin;
    public bool Moving = false;
    public bool Merging = false;
    public float Speed = 0.5f;
    public int Index;
    public int Count = 1;

    private void Update()
    {
      var cell = HexGrid.Instance.Hexcells[Index];
      if (Moving && Target != transform.position)
      {
        transform.position = Vector3.Lerp(transform.position, Target, Speed);
      }
      else
      {
        Moving = false;
        cell.Moving = false;
        if (Merging)
        {
          Merging = false;
          HexGrid.Instance.Bubbles[Index] = null;
          this.Destroy();
        }
      }
      if (Merging || Moving) return;
      var northernHexNeighbor = cell.GetNeighbor(HexDirection.N);
      var northEasternHexNeighbor = cell.GetNeighbor(HexDirection.NE);
      var northWesternHexNeighbor = cell.GetNeighbor(HexDirection.NW);
      if (northernHexNeighbor != null && HexGrid.Instance.Bubbles[northernHexNeighbor.Index] == null)
      {
        Replace(northernHexNeighbor);
      }
      else if (northEasternHexNeighbor != null &&
               HexGrid.Instance.Bubbles[northEasternHexNeighbor.Index] != null &&
               HexGrid.Instance.Bubbles[northEasternHexNeighbor.Index].Color == Color &&
               !northEasternHexNeighbor.Moving
      )
      {
        Merge(northEasternHexNeighbor);
      }
      else if (northWesternHexNeighbor != null &&
               HexGrid.Instance.Bubbles[northWesternHexNeighbor.Index] &&
               HexGrid.Instance.Bubbles[northWesternHexNeighbor.Index].Color == Color &&
               !northWesternHexNeighbor.Moving
      )
      {
        Merge(northWesternHexNeighbor);
      }
      else if (northernHexNeighbor != null &&
               HexGrid.Instance.Bubbles[northernHexNeighbor.Index] &&
               HexGrid.Instance.Bubbles[northernHexNeighbor.Index].Color == Color &&
               !northernHexNeighbor.Moving)
      {
        Merge(northernHexNeighbor);
      }
    }

    public void Replace(HexCell neighbor)
    {
      Target = neighbor.transform.position;
      HexGrid.Instance.Bubbles[Index] = null;
      Index = neighbor.Index;
      HexGrid.Instance.Bubbles[Index] = this;
      Move(neighbor);
    }

    public void Merge(HexCell neighbor)
    {
      if (!Merging)
      {
        Merging = true;
        Target = neighbor.transform.position;
        var bubble = HexGrid.Instance.Bubbles[neighbor.Index];
        var volume = (Count + bubble.Count) * 0.523598776f;
        var diameter = (float)Math.Pow(volume * 0.75 / Math.PI, .3333333) * 2;
        var scale = diameter * 15;
        bubble.transform.localScale = new Vector3(scale, scale, scale);
        bubble.Count += Count;
        Move(neighbor);
      }
    }

    public void Pop()
    {
      var points = (int) Math.Pow(Count, 2) * 100;
      HexGrid.Instance.Score += points;
      HexGrid.Instance.Moves += 1;
      HexGrid.Instance.DoParticles(transform, Color);
      HexGrid.Instance.DoScoring(transform, Color, points.ToString());
      this.Destroy();
    }

    public void Destroy()
    {
      Destroy(this.gameObject);
    }

    public void Move(HexCell neighbor)
    {
      if (!Moving)
      {
        Moving = true;
        neighbor.Moving = true;
      }
    }

    public void ChangeColor(Color color)
    {
      this.Color = color;
      var localRenderer = GetComponent<Renderer>();
      localRenderer.material.color = color;
    }

  }
}
