using Assets.Scripts;
using Assets.Scripts.DataStructures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    public List<CellInfo> _grid;
    public static PathFinding pathFinding;
    //GameManager.instance.
    public BoardInfo BoardInfo;

    private void Start()
    {
        _grid = BoardInfo.WalkableCells;
        
        pathFinding = new PathFinding(_grid, BoardInfo);
    }

}
