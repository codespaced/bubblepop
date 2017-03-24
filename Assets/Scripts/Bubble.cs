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
        private bool _moving = false;
        public float Speed = 0.5f;
        public int Index;

        private void Update()
        {
            var cell = HexGrid.Instance.Hexcells[Index];
            var northEasternHexNeighbor = cell.GetNeighbor(HexDirection.NE);
            if (northEasternHexNeighbor != null && HexGrid.Instance.Bubbles[northEasternHexNeighbor.Index].Color == Color)
            {
                Merge(northEasternHexNeighbor);
            }
            var northWesternHexNeighbor = cell.GetNeighbor(HexDirection.NW);
            if (northWesternHexNeighbor != null && HexGrid.Instance.Bubbles[northWesternHexNeighbor.Index].Color == Color)
            {
                Merge(northWesternHexNeighbor);
            }
            var northernHexNeighbor = cell.GetNeighbor(HexDirection.N);
            if (northernHexNeighbor != null && HexGrid.Instance.Bubbles[northernHexNeighbor.Index] == null)
            {
                Merge(northernHexNeighbor);
            }

            if (_moving && Target != transform.position)
            {
                transform.position = Vector3.Lerp(transform.position, Target, Speed);
            }
            else
            {
                _moving = false;
            }
        }

        public void Replace(HexCell neighbor)
        {
            Target = neighbor.transform.position;
            HexGrid.Instance.Bubbles[Index] = null;
            Index = neighbor.Index;
            HexGrid.Instance.Bubbles[Index] = this;
            Move();
        }

        public void Merge(HexCell neighbor)
        {
            Target = neighbor.transform.position;
            HexGrid.Instance.Bubbles[Index] = null;
            Index = neighbor.Index;
            HexGrid.Instance.Bubbles[Index] = this;
            Move();
        }

        public void Pop()
        {
            Destroy(this.gameObject);
        }

        public void Move()
        {
            if (!_moving)
            {
                _moving = true;
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
