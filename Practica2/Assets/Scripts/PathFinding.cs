using Assets.Scripts.DataStructures;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PathFinding
{
    private CellInfo _startCell;
    private CellInfo _endCell;
    private List<Walkable> _grid;
    public BoardInfo boardInfo;
    private List<CellInfo> grid1;
    public Vector2Int vector = new Vector2Int(2,3);

    public PathFinding(List<CellInfo> grid1, BoardInfo boardInfo)
    {
        this.grid1 = boardInfo._grid;
        this.boardInfo = boardInfo;
    }

    public PathFinding(BoardInfo boardInfo)
    {
        this.grid1 = boardInfo._grid;
        this.boardInfo = boardInfo;
    }

    public List<CellInfo> FindPath(CellInfo start, CellInfo end)
    {
        bool startLocated = false;
        bool endLocated = false;

        //Debug.Log(start.CellId);
        //Debug.Log(end.CellId);
        for (int i = 0; i < grid1.Count; i++)
        {
            if (startLocated && endLocated) break;
            if (grid1[i].RowId == start.RowId && grid1[i].ColumnId == start.ColumnId)
            {
                _startCell = grid1[i];
                startLocated = true;
            }
            if (grid1[i].RowId == end.RowId && grid1[i].ColumnId == end.ColumnId) 
            {
                _endCell = grid1[i];
                endLocated = true;
            }
        }

        //Iniciar listas y añadir la primera a la openList
        List<CellInfo> openList = new List<CellInfo> { start };
        List<CellInfo> closedList = new List<CellInfo>();

        //Setear todas las celdas y borrar caminos previos
        for (int i = 0; i < grid1.Count; i++)
        {
            grid1[i]._gCost = float.MaxValue;
            grid1[i].SetCameFromCell(null);
        }

        //Inicializar primera celda
        start._gCost = 0;
        start.SetHCost(CalculateDistanceCost(start, end));
        
        while (openList.Count > 0)
        {
            //Seleccionar la celda con menor coste F de la openList
            CellInfo currentCell = GetLowestFCostCell(openList);
            //Comprobar si la celda actual es el final del camino y si es asi generarlo
            if (currentCell == end) return CalculatePath(end);
            //Debug.Log("CurrentCell:" + currentCell.CellId);
            //Debug.Log("EndCell:" + end.CellId);

            //Mover la celda actual de la openList a la closedList
            openList.Remove(currentCell);
            closedList.Add(currentCell);

            
            //Calcular los valores de las celdas adyacentes
            foreach (CellInfo neighbourCell in GetNeighboursList(currentCell, grid1))
            {
                if (closedList.Contains(neighbourCell)) continue;

                //Setear las celdas adyacentes que se van comprobando /////////////////////////////////////
                //int newNeighborCost = current.Cost + neighbor.Weight;
                //if (newNeighborCost < neighbor.Cost)
                float tentativeCost = currentCell._gCost + 1;// CalculateDistanceCost(currentCell,neighbourCell) ; // 0 + 1 Nuevo coste G
                
                if (tentativeCost < neighbourCell._gCost) 
                {
                    neighbourCell.SetCameFromCell(currentCell);
                    neighbourCell._gCost = tentativeCost;
                    neighbourCell.SetHCost(CalculateDistanceCost(neighbourCell, end));
                }
                if (!openList.Contains(neighbourCell))
                {
                    openList.Add(neighbourCell);
                }
            }        
        }
        //Si llegamos aqui es que no se ha encontrado camino posible
        Debug.Log("No se ha encontrado camino");
        return null;
    }

    public List<CellInfo> FindPath_BFS(CellInfo start, CellInfo end)
    {


        HashSet<CellInfo> visited = new HashSet<CellInfo>();
        visited.Add(start);

        Queue<CellInfo> frontier = new Queue<CellInfo>();
        frontier.Enqueue(start);

        start.SetCameFromCell(null);

        while (frontier.Count > 0)
        {
            CellInfo current = frontier.Dequeue();


            if (current == end)
            {
                break;
            }

            foreach (var neighbor in GetNeighboursList(current, grid1))
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    frontier.Enqueue(neighbor);

                    neighbor.SetCameFromCell(current);

                }
            }
        }

        List<CellInfo> path = CalculatePath(end);

        return path;
    }
    public float CalculateDistanceCost(CellInfo actualCell, CellInfo endCell)
    {

        float xDistance = Mathf.Abs(actualCell.RowId - endCell.RowId);
        float yDistance = Mathf.Abs(actualCell.ColumnId - endCell.ColumnId);

        return xDistance + yDistance;
    }

    private CellInfo GetLowestFCostCell(List<CellInfo> openList)
    {
        CellInfo lowestFCostCell = openList[0];

        for (int i = 0; i < openList.Count; i++)
        {
            if (openList[i]._fCost < lowestFCostCell._fCost) lowestFCostCell = openList[i];
        }
        return lowestFCostCell;
    }

    private List<CellInfo> GetNeighboursList(CellInfo currentCell, List<CellInfo> grid)
    {
        List<CellInfo> neighboursList = new List<CellInfo>();
        int x = currentCell.RowId;
        int y = currentCell.ColumnId;

        for (int i = 0; i < grid.Count; i++)
        {
            //IZQ
            if (grid[i].RowId == x - 1 && grid[i].ColumnId == y && (grid[i].Walkable || grid[i].CellId == boardInfo.Exit.CellId)) neighboursList.Add(grid[i]);
            
            //DER
            if (grid[i].RowId == x + 1 && grid[i].ColumnId == y && (grid[i].Walkable || grid[i].CellId == boardInfo.Exit.CellId)) neighboursList.Add(grid[i]);
            
            //INF
            if (grid[i].RowId == x && grid[i].ColumnId == y - 1 && (grid[i].Walkable || grid[i].CellId == boardInfo.Exit.CellId)) neighboursList.Add(grid[i]);

            //SUP
            if (grid[i].RowId == x && grid[i].ColumnId == y + 1 && (grid[i].Walkable || grid[i].CellId == boardInfo.Exit.CellId)) neighboursList.Add(grid[i]);      
        }
     
        return neighboursList;
    }

    private List<CellInfo> CalculatePath(CellInfo endCell)
    {
        List<CellInfo> path = new List<CellInfo>();
        
        path.Add(endCell);
        CellInfo currentCell = endCell;
        while (currentCell.GetCameFromCell()!=null)
        {
            path.Add(currentCell.GetCameFromCell());
            currentCell = currentCell.GetCameFromCell();
        }
        path.Reverse();
        return path;
    }
}
