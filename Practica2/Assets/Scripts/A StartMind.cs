using Assets.Scripts;
using Assets.Scripts.DataStructures;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class AStartMind : AbstractPathMind
{
    public static int pathNextCell = 0;
    public CharacterBehaviour characterBehaviour;
    private PathFinding pathFinding;
    public List<CellInfo> _grid = new List<CellInfo>();
    public List<CellInfo> path = new List<CellInfo>();

    private BoardInfo BoardInfo => GameManager.instance.BoardManager.boardInfo;
    private CellInfo Exit => this.BoardInfo.Exit;
    private List<EnemyBehaviour> Enemies => this.BoardInfo.Enemies;
    private List<PlaceableItem> ItemsOnBoard => this.BoardInfo.ItemsOnBoard;

    // Start is called before the first frame update
    void Start()
    {
        pathFinding = new PathFinding(_grid, BoardInfo);
        int _object = 0;

        if (Enemies.Count != 0)
        {
            Debug.LogWarning("Enemies.Count: " + Enemies.Count);
            _object = 1;
        }
        if (ItemsOnBoard.Count != 0 && ItemsOnBoard.Count != 1) //Exit is an Item
        {
            //Debug.LogWarning("ItemCount: " + ItemsOnBoard.Count);
            _object = 2;
        }
        Debug.LogWarning("_object: " + _object);

        switch (_object)
        {
            case 1:
                PathFindingEnemies(pathFinding);
                break;
            case 2:
                //PathFindingItems(pathFinding);
                break;
            default:
                this.character.SetCurrentTarget(Exit);
                path = pathFinding.FindPath(CharacterPosition(), Exit);
                break;
        }
    }

    private void PathFindingEnemies(PathFinding pathFinding)
    {
        float _cost = 0;
        float _newCost = 0;
        int enemyNumber = 0;
        do
        {
            for (var i = 0; i < Enemies.Count; i++)
            {
                _cost = pathFinding.CalculateDistanceCost(CharacterPosition(), Enemies[i].CurrentPosition());
                if (_newCost > _cost)
                {
                    i++;
                }
                else
                {
                    _newCost = _cost;
                    enemyNumber = i;
                }
            }
            CalculateCurrentTarget(enemyNumber);
            path = pathFinding.FindPath(CharacterPosition(), Enemies[enemyNumber].CurrentPosition());
        } while (Enemies.Count == 0);
        path = pathFinding.FindPath(CharacterPosition(), Exit);
    }

    private void CalculateCurrentTarget(int targetNumber)
    {
        var enemies = BoardInfo.Enemies;
        if(enemies.Any())
        {
            //Coger al enemigo mas cercano 
            //currentTarget = ese enemigo mas cercano
            this.character.SetCurrentTarget(Enemies[targetNumber].CurrentPosition());
        }
        else
        {
            //Coger los goals (casillas que tengan llave + salida)
            //Coger el goal mas cercano
            //currentTarget = esa
        }
            
    }
    //private void PathFindingItems(PathFinding pathFinding)
    //{
    //    float _cost = 0;
    //    float _newCost = 0;
    //    int itemNumber = 0;
    //    do
    //    {
    //        for (var i = 0; i < ItemsOnBoard.Count; i++)
    //        {
    //            _cost = pathFinding.CalculateDistanceCost(_grid[0], ItemsOnBoard[i].CurrentPosition());
    //            if (_newCost > _cost)
    //            {
    //                i++;
    //            }
    //            else
    //            {
    //                _newCost = _cost;
    //                itemNumber = i;
    //            }
    //        }
    //        path = pathFinding.FindPath(_grid[0], ItemsOnBoard[itemNumber].CurrentPosition());
    //    } while (ItemsOnBoard.Count == 0);
    //    path = pathFinding.FindPath(_grid[0], Exit);
    //}

    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        int direccion = 0;   
        
        if (pathNextCell < boardInfo.path.Count) 
        {
            Debug.Log("CurrentPos" + currentPos.CellId + " Path Cell" + boardInfo.path[pathNextCell].CellId);
            if (currentPos.RowId < boardInfo.path[pathNextCell].RowId) direccion = 1;
            if (currentPos.RowId > boardInfo.path[pathNextCell].RowId) direccion = 2;
            if (currentPos.ColumnId < boardInfo.path[pathNextCell].ColumnId) direccion = 3;
            if (currentPos.ColumnId > boardInfo.path[pathNextCell].ColumnId) direccion = 4;

            pathNextCell = pathNextCell + 1;
        }
       
        switch (direccion)
        {
            case 1: return Locomotion.MoveDirection.Up;
            case 2: return Locomotion.MoveDirection.Down;
            case 3: return Locomotion.MoveDirection.Right;
            case 4: return Locomotion.MoveDirection.Left;
            default:
                print("No move");
                return Locomotion.MoveDirection.None;

        }
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
