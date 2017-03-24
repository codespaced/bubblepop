using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.VersionControl;
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
                    this.Pop();
                }
            }
            if (!Merging && !Moving)
            {
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
                bubble.transform.localScale = Vector3.Lerp(bubble.transform.localScale,
                    bubble.transform.localScale + transform.localScale * .5f, 0.25f);
                Move(neighbor);
            }
        }

        public void Pop()
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
