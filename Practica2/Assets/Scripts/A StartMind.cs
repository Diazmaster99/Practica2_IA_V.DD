using Assets.Scripts;
using Assets.Scripts.DataStructures;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public List<GameObject> itemsList = new List<GameObject>();
    private string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        pathFinding = new PathFinding(BoardInfo);
        //int _object = 0;
        sceneName = SceneManager.GetActiveScene().name;
        GetPath();
        Debug.LogWarning(Enemies.Count);

        PopulateItemsList();
    }
    private void GetPath()
    {
        Debug.LogWarning("sceneName: " + sceneName);
        switch (sceneName)
        {
            case "Enemies":
                PathFindingEnemies(pathFinding);
                break;
            case "Planning":
                PathFindingItems(pathFinding);
                break;
            default:
                //path = pathFinding.FindPath(this.BoardInfo._grid[0], Exit);
                path = pathFinding.FindPath_BFS(this.BoardInfo._grid[0], Exit);
                break;
        }
    }

    private void PathFindingEnemies(PathFinding pathFinding)
    {
        float _cost = 0;
        float _newCost = 0;
        int enemyNumber = 0;
        
        if(Enemies.Count != 0)
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
        }
        else
        {
            path = pathFinding.FindPath(CharacterPosition(), Exit);
        }
    }

    private void PathFindingItems(PathFinding pathFinding)
    {
        float _cost = 0;
        float _newCost = 0;
        var items = PopulateItemsList();
        int itemNumber = items.Length;

        while (itemNumber >= 0)
        {
            for (int i = 0; i < items.Length; i++)
            {
                _cost = pathFinding.CalculateDistanceCost(CharacterPosition(), ItemsOnBoard[i].GetItemsPosition());

                if (_newCost > _cost) i++;

                else
                {
                    _newCost = _cost;
                    itemNumber = i;
                }

            }
            CalculateCurrentTarget(itemNumber);
            path = pathFinding.FindPath(CharacterPosition(), Enemies[itemNumber].CurrentPosition());
        }

        path = pathFinding.FindPath(CharacterPosition(), Exit);
    }

    public Vector2[] PopulateItemsList()
    {
        return FindObjectsOfType<ItemLogic>(true).Select(x => (Vector2)x.transform.position).ToArray();
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

    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        int direccion = 0;
       
        if (path!=null && pathNextCell < path.Count)
        {
            Debug.Log("CurrentPos" + currentPos.CellId + " Path Cell" + path[pathNextCell].CellId);
            if (currentPos.RowId < path[pathNextCell].RowId) direccion = 1;
            if (currentPos.RowId > path[pathNextCell].RowId) direccion = 2;
            if (currentPos.ColumnId < path[pathNextCell].ColumnId) direccion = 3;
            if (currentPos.ColumnId > path[pathNextCell].ColumnId) direccion = 4;

            pathNextCell = pathNextCell + 1;
        }

        switch(sceneName)
        {
            case "Enemies":
                GetPath();
                Debug.LogWarning(Enemies.Count);
                break;
            default:
                break;
        }
        GetPath();
        switch (direccion)
        {
            case 1:
                return Locomotion.MoveDirection.Up; 
            case 2:
                return Locomotion.MoveDirection.Down;
            case 3: 
                return Locomotion.MoveDirection.Right;
            case 4:
                return Locomotion.MoveDirection.Left;
            default:
                print("No move");
                return Locomotion.MoveDirection.None;

        }
    }



    
}
