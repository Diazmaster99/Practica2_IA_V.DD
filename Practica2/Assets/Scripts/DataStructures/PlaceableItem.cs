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
            var boardManager = GameObject.FindObjectOfType<BoardManager>();
            if (boardManager == null || boardManager.boardInfo == null)
            {
                Debug.LogError("BoardManager o boardInfo no encontrado.");
                return null;
            }

            var itemObjects = GameObject.FindGameObjectsWithTag("Item");
            List<ItemLogic> itemLogics = new List<ItemLogic>();

            foreach (var item in itemObjects)
            {
                var itemLogic = item.GetComponent<ItemLogic>();
                if (itemLogic != null)
                {
                    itemLogics.Add(itemLogic);
                }
            }

            List<ItemLogic> orderedItems = new List<ItemLogic>();
            HashSet<ItemLogic> processedItems = new HashSet<ItemLogic>();

            void AddItemInOrder(ItemLogic item)
            {
                if (processedItems.Contains(item)) return;

                foreach (var precondition in item.PlaceableItem.Preconditions)
                {
                    var requiredItem = itemLogics.FirstOrDefault(i => i.PlaceableItem == precondition);
                    if (requiredItem != null)
                    {
                        AddItemInOrder(requiredItem);
                    }
                }

                processedItems.Add(item);
                orderedItems.Add(item);
            }

            foreach (var item in itemLogics)
            {
                AddItemInOrder(item);
            }

            if (orderedItems.Count == 0)
            {
                Debug.LogError("No se encontraron ítems para procesar.");
                return null;
            }

            Debug.Log("Orden de los ítems:");
            foreach (var item in orderedItems)
            {
                Debug.Log(item.Tag);
            }

            var firstItem = orderedItems.First();
            int x = (int)firstItem.transform.position.x;
            int y = (int)firstItem.transform.position.y;

            Debug.Log($"Primer ítem a recoger: {firstItem.Tag} en posición ({x}, {y})");

            return boardManager.boardInfo.CellInfos[x, y];
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