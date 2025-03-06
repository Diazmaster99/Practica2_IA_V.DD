using Assets.Scripts;
using Assets.Scripts.DataStructures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStartMind : AbstractPathMind
{
    public static int pathNextCell = 0;
    public CharacterBehaviour characterBehaviour;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
