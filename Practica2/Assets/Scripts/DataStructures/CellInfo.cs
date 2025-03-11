using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.DataStructures
{
    public class CellInfo : ICloneable
    {
        public PlaceableItem ItemInCell { get; set; }

        public CellInfo _cameFromCell;
        public int RowId { get; private set; }
        public int ColumnId { get; private set; }

        public float _hCost { get; set; }
        public float _gCost { get; set; }  //G Cost
        public float _fCost => _hCost + _gCost;

        public string CellId { get { return "" + this.RowId + "," + this.ColumnId; } }
        
        public bool Walkable { get; set; }

        public CellInfo(int col, int row)
        {
            this._gCost = 1.0f;
            this.Walkable = true;
            this.RowId = row;
            this.ColumnId = col;
            this.ItemInCell = null;
        }

        public Vector2 GetPosition
        {
            get
            {
                return new Vector2(this.ColumnId, this.RowId);
            }
        }

        public void ChangeToNoWalkable()
        {
            this.Walkable = false;
            this._gCost = float.MaxValue;
        }
        public void SetHCost(float value)
        {
            _hCost = value;
        }

        public void SetCameFromCell(CellInfo previousCell)
        {
            _cameFromCell = previousCell;
        }

        public CellInfo GetCameFromCell()
        {
            return _cameFromCell;
        }
        public object Clone()
        {
            var result = new CellInfo(this.ColumnId, this.RowId)
            {
                Walkable = this.Walkable,
                _gCost = this._gCost
            };
            if (this.ItemInCell != null)
                ItemInCell = (PlaceableItem)this.ItemInCell.Clone();
      
            return result;
        }

        public GameObject CreateGameObject(BoardManager boardManager)
        {
            var tile = boardManager.floorTile;
            if (!this.Walkable) tile = boardManager.wallTile;
            var go =  GameObject.Instantiate(tile, new Vector3(this.ColumnId, this.RowId, 0f),
                Quaternion.identity, boardManager.transform);
            if (ItemInCell != null)
            {
                var itGO = this.ItemInCell.CreateGameObject(boardManager, go.transform);
                itGO.GetComponent<ItemLogic>().Type = this.ItemInCell.Type;
                itGO.GetComponent<ItemLogic>().PlaceableItem = this.ItemInCell;

            }
            return go;
        }

        public CellInfo[] WalkableNeighbours(BoardInfo board)
        {
            var neighbours = new CellInfo[4];
            //UP
            neighbours[0] = (this.RowId < (board.NumRows -1) && board.CellInfos[this.ColumnId, this.RowId + 1].Walkable)
                ? board.CellInfos[this.ColumnId, this.RowId + 1]
                : null;
            //Right
            neighbours[1] = (this.ColumnId < (board.NumColumns -1) && board.CellInfos[this.ColumnId+1, this.RowId].Walkable)
                ? board.CellInfos[this.ColumnId+1, this.RowId]
                : null;
            //Down
            neighbours[2] = (this.RowId > 0 && board.CellInfos[this.ColumnId, this.RowId - 1].Walkable)
                ? board.CellInfos[this.ColumnId, this.RowId - 1]
                : null;
            //Left
            neighbours[3] = (this.ColumnId > 0 && board.CellInfos[this.ColumnId - 1, this.RowId].Walkable)
                ? board.CellInfos[this.ColumnId - 1, this.RowId]
                : null;
            return neighbours;
        }
    }
}