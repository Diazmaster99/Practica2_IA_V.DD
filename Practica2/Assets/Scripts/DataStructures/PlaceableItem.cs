using System;
using System.Collections.Generic;
using System.Linq; // Agrega esta línea para usar LINQ
using UnityEngine;

namespace Assets.Scripts.DataStructures
{
    public class PlaceableItem : ICloneable
    {
        private BoardInfo BoardInfo => GameManager.instance.BoardManager.boardInfo;
        private List<PlaceableItem> ItemsOnBoard => this.BoardInfo.ItemsOnBoard;
        private AStartMind aStartMind;

        public enum ItemType
        {
            Goal,
            Player,
            Enemy,
            Lever
        }

        public CellInfo GetItemsPosition()
        {
            var itemObjects = GameObject.FindGameObjectsWithTag("Item");
            var itemPositions = new List<CellInfo>();
            foreach (var item in itemObjects)
            {
                var itemLogic = item.GetComponent<ItemLogic>();
                if (itemLogic != null)
                {
                    itemPositions.Add(new CellInfo((int)item.transform.position.x, (int)item.transform.position.y));
                }
            }

            var itemPositionsArray = itemPositions.ToArray();
            Debug.Log($"Número de posiciones almacenadas: {itemPositionsArray.Length}");
            return itemPositions.FirstOrDefault();
        }
        public PlaceableItem(string tag, ItemType type)
        {
            this.Tag = tag;
            this.Activated = false;
            this.Preconditions = new List<PlaceableItem>();
            this.Type = type;
        }
        public string Tag { get; private set; }
        public ItemType Type { get; set; }
        public bool Activated { get; set; }
        public List<PlaceableItem> Preconditions { get; private set; }

        public GameObject CreateGameObject(BoardManager boardManager, Transform parent)
        {
            var tileType = boardManager.floorTile;
            switch (Type)
            {
                case ItemType.Goal:
                    tileType = boardManager.exit;
                    break;

                case ItemType.Lever:
                    tileType = boardManager.leverTile;
                    break;
                case ItemType.Player:
                case ItemType.Enemy:
                    tileType = boardManager.floorTile;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var go = GameObject.Instantiate(tileType, new Vector3(0, 0, 0f),
                Quaternion.identity, parent);
            go.transform.parent = parent;
            go.transform.localPosition = Vector3.zero;
            return go;
        }

        public object Clone()
        {
            var result = new PlaceableItem(this.Tag, this.Type) { Activated = this.Activated };
            result.Preconditions.AddRange(this.Preconditions);
            return result;
        }
    }
}